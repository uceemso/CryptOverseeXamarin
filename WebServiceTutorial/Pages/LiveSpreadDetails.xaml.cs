using CryptOverseeMobileApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChocoExchangesApi.Models;
using CryptOverseeMobileApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveSpreadDetails : ContentPage
    {
        private LiveSpreadDetailsViewModel _viewModel;
        private SpreadModel _item;
        public SpreadModel SpreadModel { get; set; }
        public List<SpreadNote> Notes { get; set; }

        public LiveSpreadDetails()
        {
            InitializeComponent();
            _viewModel = new LiveSpreadDetailsViewModel();
            BindingContext = _viewModel;
            Title = "Live Spreads - Details";
        }

        protected override void OnDisappearing() //back button logic here
        {
            _viewModel.Dispose();
            
        }

        protected override void OnAppearing()
        {
            //_viewModel.IsLoading.Value = true;
            _viewModel.StartLiveFeed(SpreadModel);
        }

    }
}