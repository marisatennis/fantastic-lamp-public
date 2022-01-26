using FantasticLamp.Models;
using FantasticLamp.Services;
using FantasticLamp.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FantasticLamp.ViewModels
{
    [QueryProperty(nameof(Lat), nameof(Lat))]
    [QueryProperty(nameof(Lng), nameof(Lng))]
    public class UpdatePinViewModel : BaseViewModel
    {
        RawPlayStore RawPlayStore => DependencyService.Get<RawPlayStore>();
        PlayStore PlayStore => DependencyService.Get<PlayStore>();
        LocationStore LocationStore => DependencyService.Get<LocationStore>();
        CircleForLocationCalculator circleForLocationCalculator => DependencyService.Get<CircleForLocationCalculator>();
        public ObservableCollection<Location> Locations { get; }
        public Location SelectedLocation { get; set; }
        private Location location;
        private string lat;
        private string lng;

        public UpdatePinViewModel()
        {
            SaveCommand = new Command(OnSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            Locations = new ObservableCollection<Location>();
            LoadLocationsCommand = new Command(async () => await ExecuteLoadLocationsCommandAsync());
        }

        public Location Location
        {
            get => location;
            set => SetProperty(ref location, value);
        }

        public string Lat
        {
            get => lat;
            set => SetProperty(ref lat, value);
        }

        public string Lng
        {
            get => lng;
            set => SetProperty(ref lng, value);
        }


        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command LoadLocationsCommand { get; }

        async Task ExecuteLoadLocationsCommandAsync()
        {
            IsBusy = true;
            location = new Location();
            try
            {
                Locations.Clear();
                var locations = await LocationStore.GetLocationsAsync();
                foreach (var location in locations)
                {
                    Locations.Add(location);
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

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            if (SelectedLocation != null)
            {
                IEnumerable<Play> plays = await PlayStore.GetPlaysFromLatLong(Convert.ToDouble(lat), Convert.ToDouble(lng));
                foreach (var play in plays)
                {
                    play.Location = SelectedLocation.Id;
                    await PlayStore.UpdatePlayAsync(play);
                    circleForLocationCalculator.UpdateLocationsCircleAsync(SelectedLocation);

                }
            }
            await Shell.Current.GoToAsync("..");
        }
    }
}