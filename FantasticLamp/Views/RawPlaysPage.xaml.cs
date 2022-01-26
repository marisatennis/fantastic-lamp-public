using FantasticLamp.ViewModels;
using Xamarin.Forms;

namespace FantasticLamp.Views
{
    public partial class RawPlaysPage : ContentPage
    {
        RawPlaysViewModel _viewModel;

        public RawPlaysPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RawPlaysViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}