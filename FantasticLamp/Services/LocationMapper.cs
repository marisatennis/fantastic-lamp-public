using System.Collections.Generic;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using SQLite;
using Xamarin.Forms;

namespace FantasticLamp.Services
{
    public class LocationMapper
    {
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();

        public async Task<Location> GetLocationFromLatLong(double Lat, double Long)
        {
            object[] parameters = {Lat, Lat, Long, Long };

            var query = "SELECT * FROM Location " +
                        "WHERE MinLat <= ? AND MaxLat >= ? " +
                         "   AND MinLong <= ? AND MaxLong >= ?";


            var locations = await Database.QueryAsync<Location>(query, parameters);

            if (locations.Count > 0)
            {
                return locations[0];
            }

            return null;
        }
    }
}
