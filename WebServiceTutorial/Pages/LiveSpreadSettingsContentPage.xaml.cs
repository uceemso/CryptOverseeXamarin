using System;
using CryptOverseeMobileApp.ViewModels.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveSpreadSettingsContentPage : ContentPage
    {
        private readonly LiveSpreadSettingsViewModel _viewmodel;
        public LiveSpreadSettingsContentPage(LiveSpreadSettingsViewModel viewModel)
        {
            InitializeComponent();
            _viewmodel = viewModel;
            BindingContext = viewModel;
        }

        
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            MessagingCenter.Send<ContentPage>(this, "live_spread_settings_popped");
            await Navigation.PopModalAsync();
        }

        protected override void OnAppearing()
        {
            _viewmodel.AvailableExchanges.SetAllowedToSavePreference(true);

            //foreach (var value in _viewmodel.MarketsVM.Values.Value)
            //{
            //    value.AllowedToSavePreference = true;
            //}

            base.OnDisappearing();
        }

        protected override void OnDisappearing()
        {
            _viewmodel.AvailableExchanges.SetAllowedToSavePreference(false);
            //foreach (var value in _viewmodel.MarketsVM.Values.Value)
            //{
            //    value.AllowedToSavePreference = false;
            //}

            base.OnDisappearing();
        }
    }
}