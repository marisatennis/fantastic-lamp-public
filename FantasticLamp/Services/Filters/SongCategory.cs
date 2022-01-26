using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public class SongCategory : ICategoryFilter
    {
        PlayStore playStore => DependencyService.Get<PlayStore>();

        public string GetName()
        {
            return "Songs";
        }

        public async Task<IEnumerable<FilterOption>> getOptions()
        {
            Collection<FilterOption> options = new Collection<FilterOption>();

            var songs = await playStore.GetSongsSearch("");
            var id = 1;
            foreach(var song in songs)
            {
                FilterOption option = new FilterOption();
                option.Category = this.GetName();
                option.Id = id;
                option.Name = song;
                options.Add(option);
                id += 1;
            }

            return options;
        }

        public void SelectOption(PlayGroupSelection selection, FilterOption option)
        {
            var song = option.Name;
            selection.Songs.Add(song);
        }

        public void RemoveOption(PlayGroupSelection selection, FilterOption option)
        {
            var song = option.Name;
            selection.Songs.Remove(song);
        }
        public void ClearSelection(PlayGroupSelection selection)
        {
            selection.Songs.Clear();
        }
    }
}
