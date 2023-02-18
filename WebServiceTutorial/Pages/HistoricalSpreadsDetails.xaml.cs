using CryptOverseeMobileApp.Models;
using CryptOverseeMobileApp.ViewModels;
using CryptOverseeMobileApp.ViewModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoricalSpreadsDetails : ContentPage
    {
        private HistoricalSpreadsDetailsViewModel _viewModel;
        public HistoricalSpreadModel HistoricalSpreadModel { get; set; }
        public HistoricalSettingsViewModel HistoricalSettingsViewModel { get; set; }

        public HistoricalSpreadsDetails()
        {
            InitializeComponent();
            _viewModel = new HistoricalSpreadsDetailsViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnDisappearing()
        {
            _viewModel.Dispose();
            //back button logic here
        }

        protected override void OnAppearing()
        {
            _viewModel.StartLiveFeed(HistoricalSpreadModel, HistoricalSettingsViewModel);
        }
    }
}