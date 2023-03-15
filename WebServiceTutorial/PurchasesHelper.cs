using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Plugin.InAppBilling;

namespace CryptOverseeMobileApp
{
    public static class PurchasesHelper
    {
        public static string ProductCode = "cryptoverseeproductid";
        private static readonly object Locker = new object();

        public static async Task<bool> PurchaseItem(string productId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();
                if (!connected)
                {
                    //we are offline or can't connect, don't try to purchase
                    return false;
                }

                //check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.Subscription);

                //possibility that a null came through.
                if (purchase == null)
                {
                    //did not purchase
                    return false;
                }
                else
                {
                    return true;
                    //purchased!
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Console.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                Console.WriteLine("Issue connecting: " + ex);
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return true;
        }


        public static bool WasPurchased(string productId)
        {
            lock (Locker)
            {
                return WasItemPurchased(productId).Result;
            }
            
        }

        public static async Task<bool> WasItemPurchased(string productId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect
                    return false;
                }

                //check purchases
                var purchases = (await billing.GetPurchasesAsync(ItemType.Subscription)).ToList();

                var p = purchases.FirstOrDefault();

                if (purchases.Any(p => p.ProductId == productId))
                {
                    //Purchase restored
                    return true;
                }
                else
                {
                    //no purchases found
                    return false;
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Console.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something has gone wrong
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return false;
        }

        public static async Task<bool> PurchaseItem()
        {
            var productId = "cryptoverseeproductid";
            var billing = CrossInAppBilling.Current;
            try
            {

                var googlePlayConnected = await billing.ConnectAsync();

                if (!googlePlayConnected)
                {
                    //we are offline or can't connect, don't try to purchase
                    return false;
                }

                //check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.Subscription);

                //possibility that a null came through.
                if (purchase == null)
                {
                    //did not purchase
                }
                else if (purchase.State == PurchaseState.Purchased)
                {
                    //only needed on android unless you turn off auto finalize
                    var ack = await CrossInAppBilling.Current.FinalizePurchaseAsync(purchase.TransactionIdentifier);

                    // Handle if acknowledge was successful or not
                }
                else
                {
                    var x = 1; //something else?
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Debug.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                Debug.WriteLine("Issue connecting: " + ex);
            }
            finally
            {
                await billing.DisconnectAsync();
            }

            return false;
        }
    }
}