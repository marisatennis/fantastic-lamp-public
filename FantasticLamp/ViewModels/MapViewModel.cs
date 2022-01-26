using FantasticLamp.Models;
using FantasticLamp.Services;
using FantasticLamp.Storage;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace FantasticLamp.ViewModels
{
    public class MapViewModel : BaseViewModel
    {
        PlayStore PlayStore => DependencyService.Get<PlayStore>();
        public ObservableCollection<Play> Plays { get; }
        public Command LoadPlaysCommand { get; }

        public MapViewModel()
        {
            Title = "Listening Locations";
            Plays = new ObservableCollection<Play>();
        }

        async void ExecuteLoadPlaysCommand()
        {
            IsBusy = true;
            try
            {
                Plays.Clear();
                var plays = await PlayStore.GetUnlocatedPlaysAsync();
                foreach (var play in plays)
                {
                    Plays.Add(play);
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
            ExecuteLoadPlaysCommand();
        }
    }
}
