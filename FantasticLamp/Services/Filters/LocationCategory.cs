using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public class LocationCategory : ICategoryFilter
    {
        LocationStore locationStore => DependencyService.Get<LocationStore>();

        public string GetName()
        {
            return "Locations";
        }

        public async Task<IEnumerable<FilterOption>> getOptions()
        {
            Collection<FilterOption> options = new Collection<FilterOption>();

            var locations = await locationStore.GetLocationsAsync();
            foreach(var location in locations.Where(x => x.MaxLat != 0))
            {
                FilterOption option = new FilterOption();
                option.Category = this.GetName();
                option.Id = location.Id;
                option.Name = location.Name;
                options.Add(option);
            }

            return options;
        }

        public void SelectOption(PlayGroupSelection selection, FilterOption option)
        {
            var location = new Location();
            location.Id = option.Id;
            selection.Locations.Add(location);
        }

        public void RemoveOption(PlayGroupSelection selection, FilterOption option)
        {
            var location = selection.Locations.Where(l => l.Id == option.Id).FirstOrDefault();
            if (location != null)
            {
                selection.Locations.Remove(location);
            }
        }

        public void ClearSelection(PlayGroupSelection selection)
        {
            selection.Locations.Clear();
        }
    }
}
