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
            PremiumMembership = new ReactiveProperty<bool>();

            MessagingCenter.Subscribe<ViewModelBase>(this, Constants.MessagingCenter_PremiumOn, (sender) =>
            {
                PremiumMembership.Value = true;
            });
            MessagingCenter.Subscribe<ViewModelBase>(this, Constants.MessagingCenter_PremiumOff, (sender) =>
            {
                PremiumMembership.Value = false;
            });
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

        public ICommand PurchaseCommand => new Command(x => { Purchase(); });

        private async Task Purchase()
        {
            var purchased = await PurchasesHelper.PurchaseItem(PurchasesHelper.ProductCode);
            if (purchased)
            {
                var premium = await PurchasesHelper.WasItemPurchased(PurchasesHelper.ProductCode);
                MessagingCenter.Send<ViewModelBase>(this, premium ? Constants.MessagingCenter_PremiumOn : Constants.MessagingCenter_PremiumOff);
            }
        }

    }


}