using FantasticLamp.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FantasticLamp.Storage
{
    public class LocationStore
    {
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();
        public static List<Location> predefineLocations = new List<Location>()
            {
                {new Location { Name = "Home", Emoji = "🏠"} },
                { new Location { Name = "Work", Emoji = "🏢" } },
                { new Location { Name = "Gym", Emoji = "💪" } },
                { new Location { Name = "Walking", Emoji = "🚶" } },
                { new Location { Name = "Running", Emoji = "🏃" } },
                { new Location { Name = "Cycling", Emoji = "🚲" } },
                { new Location { Name = "Car", Emoji = "🚗" } },
                { new Location { Name = "Park", Emoji = "🌳" } }
            };
public async Task<bool> AddLocationAsync(Location location)
        {
            var existingLocation = await Database.Table<Location>().Where((row) => row.Name.ToLower() == location.Name.ToLower()).FirstOrDefaultAsync();
            if (existingLocation == null)
            {
                await Database.InsertAsync(location);
            }
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            await Database.UpdateAsync(location);
            return await Task.FromResult(true);
        }
        public async Task<IEnumerable<Location>> GetLocationsAsync()
        {
            return await Database.Table<Location>().ToListAsync();
        }
        public IEnumerable<Location> GetLocations()
        {
            return Database.Table<Location>().ToListAsync().Result;
        }
        public async Task<Location> GetLocationAsync(int id)
        {
            return await Database.Table<Location>().Where((Location location) => location.Id == id).FirstOrDefaultAsync();
        }
        public async Task<Location> GetLocationByNameAsync(string name)
        {
            return await Database.Table<Location>().Where((Location location) => location.Name == name).FirstOrDefaultAsync();
        }
    }
}
