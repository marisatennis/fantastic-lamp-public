using FantasticLamp.Models;
using FantasticLamp.ViewModels;
using Xamarin.Forms;

namespace FantasticLamp.Views
{
    public partial class NewMoodPage : ContentPage
    {
        public Mood Mood { get; set; }

        public NewMoodPage()
        {
            InitializeComponent();
            BindingContext = new NewMoodViewModel();
        }
    }
}