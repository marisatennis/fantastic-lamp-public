using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FantasticLamp.Services
{
    public class CircleForLocationCalculator
    {
        PlayStore playStore => DependencyService.Get<PlayStore>();
        SQLiteAsyncConnection Database => DependencyService.Get<DatabaseFactory>().GetDatabase();
        LocationStore locationStore => DependencyService.Get<LocationStore>();

        public async void UpdateLocationsCircleAsync(Location selectedLocation)
        {
            int parameters = selectedLocation.Id;
            var query = "" +
                        "SELECT " +
                            "a.Id " +
                            ",a.Name " +
                            ",a.Emoji " +
                            ",b.MaxLat " +
                            ",b.MinLat " +
                            ",b.MaxLong " +
                            ",b.MinLong " +
                        "FROM Location a " +
                        "JOIN ( " +
                           "SELECT " +
                                "Location " +
                                ",MAX(Lat) AS MaxLat " +
                                ",MIN(Lat) AS MinLat " +
                                ",MAX(Long) AS MaxLong " +
                                ",MIN(Long) AS MinLong " +
                           "FROM Play " +
                           "WHERE Location = ? " +
                           "GROUP BY " +
                                "Location) b " +
                        "ON a.Id = b.Location; ";

            var location = await Database.QueryAsync<Location>(query,parameters);

            await locationStore.UpdateLocationAsync(location.First());
        }
    }
}
