using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;
using FantasticLamp.Services;
using System;

namespace FantasticLamp.Services.Filters
{
    public class YearCategory : ICategoryFilter
    {
        PlayStore playStore => DependencyService.Get<PlayStore>();
        PlayGrouper playGrouper => DependencyService.Get<PlayGrouper>();
        PlayGroupSelection PlayGroupSelection = new PlayGroupSelection();

        public string GetName()
        {
            return "Years";
        }

        public async Task<IEnumerable<FilterOption>> getOptions()
        {
            Collection<FilterOption> options = new Collection<FilterOption>();
            var years = await playGrouper.getPlaysByYear(PlayGroupSelection);
            foreach (var year in years)
            {
                FilterOption option = new FilterOption();
                option.Category = this.GetName();
                option.Id = int.Parse(year.Name);
                option.Name = year.Name;
                options.Add(option);
            }
            return options;
        }

        public void SelectOption(PlayGroupSelection selection, FilterOption option)
        {
            var year = option.Name;
            selection.Years.Add(year);
        }

        public void RemoveOption(PlayGroupSelection selection, FilterOption option)
        {
            var year = option.Name;
            selection.Years.Remove(year);
        }
        public void ClearSelection(PlayGroupSelection selection)
        {
            selection.Years.Clear();
        }
    }
}
