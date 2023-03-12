using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CryptOverseeMobileApp.Models;
using Reactive.Bindings;
using Xamarin.Forms;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class SettingsViewModel: ViewModelBase
    {


        public SettingsViewModel()
        {

        }

        public ReactiveProperty<bool> PremiumMembership { get; set; }

        public ICommand TapOnExchangeElement =>
            new Command(selectedItem =>
            {
                try
                {
                    if (PremiumMembership.Value)
                    {
                        var item = (Element)selectedItem;
                        item.Toggle();
                    }
                }
                catch (Exception ex)
                {

                }
            });

    }


}