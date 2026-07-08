using GestationTracker.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestationTracker.Services
{
    public interface ICatService
    {
        Task<List<Cats>> GetCatsAsync();
        Task<Cats?> GetCatsByIdAsync(string id);
        Task<bool> AddCatAsync(Cats cat);
        Task<bool> UpdateCatAsync(Cats cat);
        Task<bool> DeleteCatAsync(string id);

    }
}
