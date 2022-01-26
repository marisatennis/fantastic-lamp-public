using FantasticLamp.Models;
using SQLite;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;

namespace FantasticLamp.Storage
{
    public class RawPlayStore
    {
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();

        public async Task<bool> AddRawPlayAsync(RawPlay play)
        {
            await Database.InsertAsync(play);
            return await Task.FromResult(true);
        }
        public Task<bool> AddPlay(RawPlay play)
        {
            Database.InsertAsync(play);
            return Task.FromResult(true);
        }

        public async Task<bool> UpdatePlayAsync(RawPlay play)
        {
            await Database.UpdateAsync(play);
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<RawPlay>> GetUnprocessedPlaysAsync()
        {
            return await Database.Table<RawPlay>().Where(row => row.Play == 0).ToListAsync();
        }
        public async Task<IEnumerable<RawPlay>> GetForPlayAsync(Play play)
        {
            return await Database.Table<RawPlay>().Where(row => row.Play == play.Id).ToListAsync();
        }
    }
}
