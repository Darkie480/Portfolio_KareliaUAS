using GestationTracker.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestationTracker.Services
{
    public class PregnancyService
    {
        private List<PregnancyStage> _cachedStages;

        public async Task<List<PregnancyStage>> GetStagesAsync()
        {
            if (_cachedStages != null)
                return _cachedStages;

            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("pregnancystagedata.json");
                using var reader = new StreamReader(stream);

                var json = await reader.ReadToEndAsync();

                System.Diagnostics.Debug.WriteLine(json);

                var root = JsonSerializer.Deserialize<PregnancyStageRoot>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (root == null)
                {
                    System.Diagnostics.Debug.WriteLine("ROOT IS NULL → JSON structure mismatch");
                    return new List<PregnancyStage>();
                }

                if (root.Stages == null)
                {
                    System.Diagnostics.Debug.WriteLine("STAGES IS NULL → property mismatch in JSON");
                    return new List<PregnancyStage>();
                }

                _cachedStages = root?.Stages ?? new List<PregnancyStage>();

                System.Diagnostics.Debug.WriteLine($"Loaded stages: {_cachedStages.Count}");

                return _cachedStages;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Virhe tiedostoja lukiessa: {ex.Message}");
                return new List<PregnancyStage>();

            }

        }

    }
}