using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public class AlbumCategory : ICategoryFilter
    {
        PlayStore playStore => DependencyService.Get<PlayStore>();

        public string GetName()
        {
            return "Albums";
        }

        public async Task<IEnumerable<FilterOption>> getOptions()
        {
            Collection<FilterOption> options = new Collection<FilterOption>();

            var albums = await playStore.GetAlbumsSearch("");
            var id = 1;
            foreach(var album in albums)
            {
                FilterOption option = new FilterOption();
                option.Category = this.GetName();
                option.Id = id;
                option.Name = album;
                options.Add(option);
                id += 1;
            }

            return options;
        }

        public void SelectOption(PlayGroupSelection selection, FilterOption option)
        {
            var album = option.Name;
            selection.Albums.Add(album);
        }

        public void RemoveOption(PlayGroupSelection selection, FilterOption option)
        {
            var album = option.Name;
            selection.Albums.Remove(album);
        }
        public void ClearSelection(PlayGroupSelection selection)
        {
            selection.Albums.Clear();
        }
    }
}
