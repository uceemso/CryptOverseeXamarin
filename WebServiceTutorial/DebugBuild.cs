using System.Diagnostics;
using Xamarin.Forms;


namespace CryptOverseeMobileApp
{
    public class DebugBuild : ContentView
    {
        private bool _visibility;

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            bool isDebug = false;
            IsDebugCheck(ref isDebug);
            if (!isDebug)
            {
               // this.Content = null;
                this.IsVisible = false;
            }

        }

        public bool Visibility2
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged();
            }
        }

        [Conditional("DEBUG")]
        private void IsDebugCheck(ref bool isDebug)
        {
            isDebug = true;
        }
    }
}