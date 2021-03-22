using System.Collections.Generic;
using System.Collections.ObjectModel;
using Reactive.Bindings;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class ObjectSelectorViewModel : ViewModelBase
    {
        public ObjectSelectorViewModel()
        {
            Values = new ReactiveProperty<ObservableCollection<SettingItemViewModel>>(new ObservableCollection<SettingItemViewModel>());

        }

        public ReactiveProperty<ObservableCollection<SettingItemViewModel>> Values { get; }

        public void UpdateMarkets(IEnumerable<SettingItemViewModel> values)
        {
            Values.Value = new ObservableCollection<SettingItemViewModel>(values);
        }

        public IEnumerable<SettingItemViewModel> GetValues()
        {
            return Values.Value;
        }

    }
}