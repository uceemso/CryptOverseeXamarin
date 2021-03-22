using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptOverseeMobileApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CryptOverseeMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPage1 : TabbedPage
    {
        public TabbedPage1()
        {
            InitializeComponent();
            //this.Children.Add(new LiveSpread());

        }
    }
}