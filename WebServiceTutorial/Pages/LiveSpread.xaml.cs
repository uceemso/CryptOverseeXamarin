using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptOverseeMobileApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveSpread : ContentPage
    {
        private readonly LiveSpreadViewModel _vm;
        public LiveSpread()
        {
            InitializeComponent();
            _vm = (LiveSpreadViewModel)BindingContext;
            _vm.Navigation = Navigation;

            MessagingCenter.Subscribe<ContentPage>(this, "live_spread_settings_popped", (sender) =>
            {
                _vm.RefreshGrid();
            });

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            WarningConnection();
        }

        private async void WarningConnection()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Trouble accessing Internet", "You need an internet connection to see these spreads!", "OK");
            }
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.ApplySearchBar(e.NewTextValue);
        }
    }
}