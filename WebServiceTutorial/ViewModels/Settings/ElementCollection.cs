using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Reactive.Bindings;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class ElementCollection : ViewModelBase
    {
        public ElementCollection()
        {
            Values = new ReactiveProperty<ObservableCollection<Element>>(new ObservableCollection<Element>());

        }

        public ReactiveProperty<ObservableCollection<Element>> Values { get; }

        public void SetValues(IEnumerable<Element> values)
        {
            Values.Value = new ObservableCollection<Element>(values);
        }

        public List<Element> GetValues()
        {
            return Values.Value.ToList();
        }

        public bool IsEmpty()
        {
            return !Values.Value.ToList().Any();
        }

    }
}