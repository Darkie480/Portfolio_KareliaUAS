using GestationTracker.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GestationTracker.Services
{
    public class LocalStorageService
    {
        private readonly string _fileName = "sample_cats.json";
        private readonly string _filePath;

        public LocalStorageService()
        {
            // FileSystem.AppDataDirectory is where you can store persistent data.
            // Android: /data/user/0/com.companyname.gestationtracker/files/
            // iOS: /var/mobile/Containers/Data/Application/{GUID}/Documents/
            // Windows: C:\Users\{Username}\AppData\Local\Packages\{PackageId}\LocalState\
            _filePath = Path.Combine(FileSystem.AppDataDirectory, _fileName);
        }

        public async Task SaveCatAsync(List<Cats> cats)
        {
            try
            {
                var json = JsonSerializer.Serialize(cats, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });

                await File.WriteAllTextAsync(_filePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Virhe kissaa tallentaessa: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Cats>> LoadCatsAsync()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    System.Diagnostics.Debug.WriteLine($"Tallennettua tiedostoa ei löytynyt tiedostopolussa {_filePath}");
                    return new List<Cats> { };
                }

                var json = await File.ReadAllTextAsync(_filePath);

                var cats = JsonSerializer.Deserialize<List<Cats>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return cats ?? new List<Cats>();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Virhe kissoja ladattaessa: {ex.Message}");
                return new List<Cats>();
            }
        }

        public bool HasSavedData()
        {
            return File.Exists(_filePath);
        }

        public void ClearData()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    File.Delete(_filePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Virhe dataa poistettaessa: {ex.Message}");
            }
        }
        public string GetFilePath() => _filePath;

    }

}
