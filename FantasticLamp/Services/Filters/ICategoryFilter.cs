using System.Collections.Generic;
using System.Threading.Tasks;
using FantasticLamp.Models;
using Xamarin.Forms;

namespace FantasticLamp.Services.Filters
{
    public interface ICategoryFilter
    {

        string GetName();

        Task<IEnumerable<FilterOption>> getOptions();

        void SelectOption(PlayGroupSelection selection, FilterOption option);

        void RemoveOption(PlayGroupSelection selection, FilterOption option);

        void ClearSelection(PlayGroupSelection selection);
    }
}
