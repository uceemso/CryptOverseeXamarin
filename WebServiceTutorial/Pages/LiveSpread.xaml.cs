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
    public partial class LiveSpread : ContentPage
    {
        public LiveSpread()
        {
            InitializeComponent();
            BindingContext = new LiveSpreadViewModel();

        }
    }
}