using FantasticLamp.Models;
using FantasticLamp.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace FantasticLamp.Views
{
    public partial class UpdatePinPage : ContentPage
    {
        UpdatePinViewModel _viewModel;
        public UpdatePinPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new UpdatePinViewModel();
        }
        protected override void OnAppearing()
        {
            _viewModel.OnAppearing();
        }
    }
}