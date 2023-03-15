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

            MessagingCenter.Subscribe<ContentPage>(this, Constants.MessagingCenter_HistoricalSettingsClosed, (sender) =>
            {
                viewModel.RefreshData(false);
            });
        }
       

    }
}