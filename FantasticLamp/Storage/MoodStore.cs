using FantasticLamp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FantasticLamp.Storage
{
    public class MoodStore
    {
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();
        public async Task<bool> AddMoodAsync(Mood mood)
        {
            await Database.InsertAsync(mood);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateMoodAsync(Mood mood)
        {
            await Database.UpdateAsync(mood);
            return await Task.FromResult(true);
        }
        public async Task<IEnumerable<Mood>> GetMoodsAsync()
        {
            return await Database.Table<Mood>().ToListAsync();
        }
        public async Task<Mood> GetMoodAsync(int id)
        {
            return await Database.Table<Mood>().Where((Mood mood) => mood.Id == id).FirstOrDefaultAsync();
        }
    }
}
