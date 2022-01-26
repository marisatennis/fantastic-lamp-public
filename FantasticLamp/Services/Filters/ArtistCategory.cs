using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public class ArtistCategory : ICategoryFilter
    {
        PlayStore playStore => DependencyService.Get<PlayStore>();

        public string GetName()
        {
            return "Artists";
        }

        public async Task<IEnumerable<FilterOption>> getOptions()
        {
            Collection<FilterOption> options = new Collection<FilterOption>();

            var artists = await playStore.GetArtistsSearch("");
            var id = 1;
            foreach(var artist in artists)
            {
                FilterOption option = new FilterOption();
                option.Category = this.GetName();
                option.Id = id;
                option.Name = artist;
                options.Add(option);
                id += 1;
            }

            return options;
        }

        public void SelectOption(PlayGroupSelection selection, FilterOption option)
        {
            var artist = option.Name;
            selection.Artists.Add(artist);
        }

        public void RemoveOption(PlayGroupSelection selection, FilterOption option)
        {
            var artist = option.Name;
            selection.Artists.Remove(artist);
        }
        public void ClearSelection(PlayGroupSelection selection)
        {
            selection.Artists.Clear();
        }
    }
}
