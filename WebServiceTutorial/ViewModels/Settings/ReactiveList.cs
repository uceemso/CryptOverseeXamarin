using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Reactive.Bindings;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public class ReactiveList
    {
        private readonly ReactiveProperty<ObservableCollection<Element>> _values = new(new ObservableCollection<Element>());

        public ReactiveList()
        {

        }

        public ObservableCollection<Element> ValuesForUI => _values.Value;

        public void SetValues(IEnumerable<Element> values)
        {
            _values.Value = new ObservableCollection<Element>(values);
        }

        public void SetAllowedToSavePreference(bool b)
        {
            foreach (var value in _values.Value)
            {
                value.AllowedToSavePreference = b;
            }
        }

        public List<Element> GetValues()
        {
            return _values.Value.ToList();
        }

        public bool IsEmpty()
        {
            return !_values.Value.ToList().Any();
        }

    }
}