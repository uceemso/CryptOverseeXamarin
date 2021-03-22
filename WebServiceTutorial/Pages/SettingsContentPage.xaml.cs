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
    public partial class SettingsContentPage : ContentPage
    {
        public SettingsContentPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        
        async void OnDismissButtonClicked(object sender, EventArgs args)
        {
            MessagingCenter.Send<ContentPage>(this, "popped");
            await Navigation.PopModalAsync();
        }
    }
}