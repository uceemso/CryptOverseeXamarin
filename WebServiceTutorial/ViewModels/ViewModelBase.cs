using Reactive.Bindings;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CryptOverseeMobileApp.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            DebugMode = new ReactiveProperty<bool>();

            bool isDebug = false;
            IsDebugCheck(ref isDebug);
            DebugMode.Value = isDebug;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public ReactiveProperty<bool> DebugMode { get; set; }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [Conditional("DEBUG")]
        private void IsDebugCheck(ref bool isDebug)
        {
            isDebug = true;
        }
    }
}