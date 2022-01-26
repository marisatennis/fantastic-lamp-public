using FantasticLamp.Models;
using FantasticLamp.Storage;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FantasticLamp.ViewModels
{
    public class RawPlaysViewModel : BaseViewModel
    {
        RawPlayStore RawPlayStore => DependencyService.Get<RawPlayStore>();

        public ObservableCollection<RawPlay> RawPlays { get; }
        public Command LoadRawPlaysCommand { get; }

        public RawPlaysViewModel()
        {
            Title = "RawPlays";
            RawPlays = new ObservableCollection<RawPlay>();
            LoadRawPlaysCommand = new Command(async () => await ExecuteLoadRawPlaysCommand());
        }

        async Task ExecuteLoadRawPlaysCommand()
        {
            IsBusy = true;

            try
            {
                RawPlays.Clear();
                var rawplays = await RawPlayStore.GetUnprocessedPlaysAsync();
                foreach (var rawplay in rawplays.OrderByDescending(o => o.Id))
                {
                    RawPlays.Add(rawplay);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }
    }
}