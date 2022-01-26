using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public class MoodCategory : ICategoryFilter
    {
        MoodStore moodStore => DependencyService.Get<MoodStore>();

        public string GetName()
        {
            return "Moods";
        }

        public async Task<IEnumerable<FilterOption>> getOptions()
        {
            Collection<FilterOption> options = new Collection<FilterOption>();

            var moods = await moodStore.GetMoodsAsync();
            foreach (var mood in moods)
            {
                FilterOption option = new FilterOption();
                option.Category = this.GetName();
                option.Id = mood.Id;
                option.Name = mood.Name;
                options.Add(option);
            }

            return options;
        }

        public void SelectOption(PlayGroupSelection selection, FilterOption option)
        {
            var mood = new Mood();
            mood.Id = option.Id;
            selection.Moods.Add(mood);
        }

        public void RemoveOption(PlayGroupSelection selection, FilterOption option)
        {
            var mood = selection.Moods.Where(m => m.Id == option.Id).FirstOrDefault();
            if (mood != null)
            {
                selection.Moods.Remove(mood);
            }
        }

        public void ClearSelection(PlayGroupSelection selection)
        {
            selection.Moods.Clear();
        }
    }
}
