using FantasticLamp.Views;
using Xamarin.Forms;

namespace FantasticLamp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NewMoodPage), typeof(NewMoodPage));
            Routing.RegisterRoute(nameof(UpdateMoodPage), typeof(UpdateMoodPage));
            Routing.RegisterRoute(nameof(UpdatePinPage), typeof(UpdatePinPage));
        }
    }
}
