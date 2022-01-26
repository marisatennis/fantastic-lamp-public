using FantasticLamp.ViewModels;
using Xamarin.Forms;

namespace FantasticLamp.Views
{
    public partial class MoodsPage : ContentPage
    {
        MoodsViewModel _viewModel;

        public MoodsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new MoodsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}