using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using FantasticLamp.Models;
using FantasticLamp.Storage;
using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public class WeekDayCategory : ICategoryFilter
    {
        PlayGrouper playGrouper => DependencyService.Get<PlayGrouper>();
        PlayGroupSelection PlayGroupSelection = new PlayGroupSelection();

        public string GetName()
        {
            return "WeekDays";
        }

        public async Task<IEnumerable<FilterOption>> getOptions()
        {
            Collection<FilterOption> options = new Collection<FilterOption>();
            var weekDays = await playGrouper.getPlaysByWeekDay(PlayGroupSelection);
            foreach (var weekDay in weekDays)
            {
                FilterOption option = new FilterOption();
                option.Category = this.GetName();
                option.Id = int.Parse(weekDay.Name);
                option.Name = PlayGroupSelection.convertFromWeekDayNumberToName(int.Parse(weekDay.Name));
                options.Add(option);
            }
            return options;
        }

        public void SelectOption(PlayGroupSelection selection, FilterOption option)
        {
            var weekDay = option.Id;
            selection.WeekDays.Add(weekDay.ToString());
        }

        public void RemoveOption(PlayGroupSelection selection, FilterOption option)
        {
            var weekDay = option.Id;
            selection.WeekDays.Remove(weekDay.ToString());
        }
        public void ClearSelection(PlayGroupSelection selection)
        {
            selection.WeekDays.Clear();
        }
    }
}