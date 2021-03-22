using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptOverseeMobileApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveSpreadsBitcoin : ContentPage
    {
        public LiveSpreadsBitcoin()
        {
            InitializeComponent();
            var viewModel = (LiveSpreadsBitcoinViewModel)BindingContext;
            viewModel.Navigation = Navigation;

            MessagingCenter.Subscribe<ContentPage>(this, "popped", (sender) =>
            {
                viewModel.RefreshData();
            });
            
        }

        void OnCollectionViewScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            //Debug.WriteLine("HorizontalDelta: " + e.HorizontalDelta);
            //Debug.WriteLine("VerticalDelta: " + e.VerticalDelta);
            //Debug.WriteLine("HorizontalOffset: " + e.HorizontalOffset);
            //Debug.WriteLine("VerticalOffset: " + e.VerticalOffset);
            //Debug.WriteLine("FirstVisibleItemIndex: " + e.FirstVisibleItemIndex);
            //Debug.WriteLine("CenterItemIndex: " + e.CenterItemIndex);
            //Debug.WriteLine("LastVisibleItemIndex: " + e.LastVisibleItemIndex);
        }


        private void OnButtonClicked(object sender, EventArgs e)
        {

        }
    }
}