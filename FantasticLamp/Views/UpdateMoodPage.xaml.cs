using FantasticLamp.ViewModels;
using Xamarin.Forms;

namespace FantasticLamp.Views
{
    public partial class UpdateMoodPage : ContentPage
    {
        public UpdateMoodPage()
        {
            InitializeComponent();
            BindingContext = new UpdateMoodViewModel();
        }
    }
}