using FantasticLamp.Services;
using FantasticLamp.Storage;
using Xamarin.Forms;

namespace FantasticLamp
{
    public partial class App : Application
    {

        public App()
        {
            Device.SetFlags(new string[] { "Shapes_Experimental" });
            InitializeComponent();
            DependencyService.Register<CircleForLocationCalculator>();
            DependencyService.Register<DatabaseFactory>();
            DependencyService.Register<ImageStore>();
            DependencyService.Register<LocationMapper>();
            DependencyService.Register<LocationStore>();
            DependencyService.Register<MoodLogStore>();
            DependencyService.Register<MoodStore>();
            DependencyService.Register<PlayGrouper>();
            DependencyService.Register<PlayLogger>();
            DependencyService.Register<PlayPositionCalculator>();
            DependencyService.Register<PlayStore>();
            DependencyService.Register<RawPlayStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
