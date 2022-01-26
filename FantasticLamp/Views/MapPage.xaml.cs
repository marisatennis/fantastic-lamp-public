using FantasticLamp.Models;
using FantasticLamp.Services;
using FantasticLamp.Storage;
using FantasticLamp.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;

namespace FantasticLamp.Views
{
    public partial class MapPage : ContentPage
    {
        MapViewModel _viewModel;
        LocationStore locationStore => DependencyService.Get<LocationStore>();
        PlayStore playStore => DependencyService.Get<PlayStore>();
        CircleForLocationCalculator circleForLocationCalculator => DependencyService.Get<CircleForLocationCalculator>();

        Circle Circle { get; set; }
        Button Button { get; set; }
        Position Position { get; set;  }
        public MapPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MapViewModel();

            Circle = new Circle();
            Button = new Button();
            Position = new Position();

            var getPosition = playStore.GetLatestPlayPosition();
            if (getPosition == null )
            {
                Position = new Position(-2.167070, -80.827840);
            }
            else
            {
                Position = (Position)getPosition;
            }
            
            map.MoveToRegion(MapSpan.FromCenterAndRadius(Position, Distance.FromKilometers(1)));

            IEnumerable<Location> locations = this.locationStore.GetLocations();
            foreach (var location in locations)
            {
                Circle circle = new Circle();
                circle.Center = location.Pin;
                circle.Radius = location.Radius;
                circle.FillColor = (Color)Application.Current.Resources["Secondary"];
                circle.StrokeColor = (Color)Application.Current.Resources["LightBackground"];
                map.MapElements.Add(circle);
                if (location.MaxLat != 0 )
                {
                    Button buttonLocation = new Button
                    {
                        Text = location.Name
                    };
                    buttonLocation.Clicked += OnButtonClicked;
                    buttonLocation.Style = (Style)Application.Current.Resources["FilterOptionButton"];
                    stacklayout.Children.Add(buttonLocation);
                }
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        private void Pin_MarkerClickedAsync(object sender, Xamarin.Forms.Maps.PinClickedEventArgs e)
        {
            Position pin = ((Pin)sender).Position;
            Shell.Current.GoToAsync($"{nameof(UpdatePinPage)}?{nameof(UpdatePinViewModel.Lat)}={pin.Latitude}&{nameof(UpdatePinViewModel.Lng)}={pin.Longitude}");
        }

        async void OnButtonClicked(object sender, EventArgs args)
        {
            if (Button != null)
            {
                VisualStateManager.GoToState(Button, "Normal");
            }
            Button = (sender as Button);
            VisualStateManager.GoToState(Button, "UnSelectedCategory");

            if (Circle != null)
            {
                Circle.FillColor = (Color)Application.Current.Resources["Secondary"];
            }
            Location location = await locationStore.GetLocationByNameAsync(Button.Text);
            Circle = map.MapElements.OfType<Circle>().Where(x => x.Center == location.Pin).FirstOrDefault();
            Circle.FillColor = (Color)Application.Current.Resources["Fourth"];

            //If the circle is not in view move the map to it
            if( Distance.BetweenPositions(Position,location.Pin).Meters >= 100 )
            {
                map.MoveToRegion(MapSpan.FromCenterAndRadius(location.Pin, Distance.FromMeters(100)));
            }
            Position = location.Pin;
        }
    }
}
