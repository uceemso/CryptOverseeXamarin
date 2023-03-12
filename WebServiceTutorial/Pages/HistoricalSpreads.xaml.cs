using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptOverseeMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoricalSpreads : ContentPage
    {

        public HistoricalSpreads()
        {
            InitializeComponent();
            var viewModel = (HistoricalSpreadsViewModel) BindingContext;
            viewModel.Navigation = Navigation;

            MessagingCenter.Subscribe<ContentPage>(this, "popped2", (sender) =>
            {
                viewModel.RefreshData(false);
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }



        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }
        

    }
}