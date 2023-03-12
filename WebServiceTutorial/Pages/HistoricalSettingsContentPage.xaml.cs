using System;
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
            _viewmodel.AvailableExchanges.SetAllowedToSavePreference(true);
            _viewmodel.AvailableMarkets.SetAllowedToSavePreference(true);

            base.OnDisappearing();
        }

        protected override void OnDisappearing()
        {
            _viewmodel.AvailableExchanges.SetAllowedToSavePreference(false);
            _viewmodel.AvailableMarkets.SetAllowedToSavePreference(false);

            base.OnDisappearing();
        }
    }
}