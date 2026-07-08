using GestationTracker.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GestationTracker.Services
{
    public class CatService : ICatService
    {
        private List<Cats> _cats = new();
        private bool _isInitialized = false;

        private readonly string _sampleFileName = "sample_cats.json";
        private readonly LocalStorageService _storageService;

        public CatService()
        {
           _storageService = new LocalStorageService();
        }

        /// <summary>
        /// Runs _isInitialized once in order to make the program run smoother.
        /// Reads Json-file and deserializes the list to more readable form.
        /// </summary>
        /// <returns></returns>
        /// 

        private async Task InitializeAsync()
        {
            if (_isInitialized) return;

            try
            {
                if (_storageService.HasSavedData())
                {
                    _cats = await _storageService.LoadCatsAsync();
                }
                else
                {
                    using var stream = await FileSystem.OpenAppPackageFileAsync("sample_cats.json");
                    using var reader = new StreamReader(stream);
                    var json = await reader.ReadToEndAsync();

                    _cats = JsonSerializer.Deserialize<List<Cats>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<Cats>();

                    await _storageService.SaveCatAsync(_cats);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ongelma ohjelman käynnistyksessä: {ex.Message}");
                _cats = new List<Cats>();
            }
        }

        public async Task <List<Cats>> GetCatsAsync()
        {
            await InitializeAsync();
            return new List<Cats>( _cats );
        }

        public async Task<Cats?> GetCatsByIdAsync(string id)
        {
            await InitializeAsync();
            return _cats.FirstOrDefault( c => c.Id == id );
        }

        public async Task<bool> AddCatAsync(Cats cat)
        {
            await InitializeAsync();
            if (string.IsNullOrWhiteSpace(cat.Id))
                cat.Id = Guid.NewGuid().ToString();
            _cats.Add(cat);

            await _storageService.SaveCatAsync(_cats);
            return true;
        }

        public async Task<bool> UpdateCatAsync(Cats cat)
        {
            await InitializeAsync();
            var existingCat = _cats.FirstOrDefault (c => c.Id == cat.Id);
            if (existingCat != null)
            {
                _cats.Remove(existingCat);
               
            }
            _cats.Add(cat);

            await _storageService.SaveCatAsync(_cats);
            return true;
        }

        public async Task<bool> DeleteCatAsync(string id)
        {
            await InitializeAsync();
            var cat = _cats.FirstOrDefault (c => c.Id == id);
            if (cat == null) return false;
            _cats.Remove(cat);

            await _storageService.SaveCatAsync(_cats);

            return true;
        }



    }
}
