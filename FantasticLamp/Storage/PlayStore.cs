using FantasticLamp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FantasticLamp.Storage
{
    public class PlayStore
    {
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();
        public async Task<bool> AddPlayAsync(Play play)
        {
            await Database.InsertAsync(play);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdatePlayAsync(Play play)
        {
            await Database.UpdateAsync(play);
            return await Task.FromResult(true);
        }
        public async Task<IEnumerable<Play>> GetUnlocatedPlaysAsync()
        {
            return await Database.Table<Play>().Where(row => row.Location == 0).ToListAsync();
        }
        public async Task<Play> GetPlayAsync(int Id)
        {
            return await Database.Table<Play>().Where(row => row.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Play>> GetPlaysFromLatLong(double Lat, double Long)
        {
            return await Database.Table<Play>().Where(row => row.Lat == Lat && row.Long == Long).ToListAsync();
        }
        public Position? GetLatestPlayPosition()
        {
            var play = Database.Table<Play>().OrderByDescending(row => row.Id).Take(1).FirstOrDefaultAsync().Result;
            if (play == null)
            {
                return null;
            }
            return play.Pin;
        }
        public async Task<IEnumerable<Play>> GetPlaysFromLocationAsync(Location location)
        {
            return await Database.Table<Play>().Where(row => row.Location == location.Id).ToListAsync();
        }
        public async Task<List<string>> GetArtistsSearch(string item)
        {
            item = item.ToLower();
            string query = "SELECT DISTINCT Artist AS Name FROM Play WHERE lower(Artist) LIKE '%" + item + "%';";

            return await Database.QueryScalarsAsync<string>(query);
        }
        public async Task<List<string>> GetAlbumsSearch(string item)
        {
            item = item.ToLower();
            string query = "SELECT DISTINCT Album AS Name FROM Play WHERE lower(Album) LIKE '%" + item + "%';";

            return await Database.QueryScalarsAsync<string>(query);
        }
        public async Task<List<string>> GetSongsSearch(string item)
        {
            item = item.ToLower();
            string query = "SELECT DISTINCT Song AS Name FROM Play WHERE lower(Song) LIKE '%" + item + "%';";

            return await Database.QueryScalarsAsync<string>(query);
        }
    }
}
