using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptOverseeMobileApp.ViewModels;
using CryptOverseeMobileApp.ViewModels.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoricalSettingsContentPage : ContentPage
    {
        private readonly HistoricalSettingsViewModel _viewmodel;
        public HistoricalSettingsContentPage(HistoricalSettingsViewModel viewModel)
        {
            InitializeComponent();
            _viewmodel = viewModel;
            BindingContext = viewModel;
        }

        
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            MessagingCenter.Send<ContentPage>(this, "popped2");
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            foreach (var value in _viewmodel.ExchangesVM.Values.Value)
            {
                value.AllowedToSavePreference = true;
            }
            foreach (var value in _viewmodel.MarketsVM.Values.Value)
            {
                value.AllowedToSavePreference = true;
            }

            base.OnDisappearing();
        }

        protected override void OnDisappearing()
        {
            foreach (var value in _viewmodel.ExchangesVM.Values.Value)
            {
                value.AllowedToSavePreference = false;
            }
            foreach (var value in _viewmodel.MarketsVM.Values.Value)
            {
                value.AllowedToSavePreference = false;
            }

            base.OnDisappearing();
        }
    }
}