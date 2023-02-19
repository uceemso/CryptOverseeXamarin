using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Boolean;

namespace CryptOverseeMobileApp.ViewModels.Settings
{

    public class Element : ViewModelBase
    {
        private bool _enabled;
        private readonly string _key;

        public Element(string pageName, string exchange)
        {
            Name = exchange.Trim();
            AllowedToSavePreference = true;

            _key = $"{pageName}_{Name}";
            TryParse(Preferences.Get(_key, "True"), out var enabled);
            Enabled = enabled;
        }

        public bool PremiumMembership { get; set; }
        public string Name { get; set; }
        public bool AllowedToSavePreference { get; set; }

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }

        public ICommand SavePreferencesCommand => new Command(x => { SavePrefs(); });

        public void SavePrefs()
        {
            if (!AllowedToSavePreference) return;
            Preferences.Set(_key, Enabled.ToString());
            Console.WriteLine($"Saved preference to {Enabled} for key {_key}");
        }

        public void Toggle()
        {
            if (!AllowedToSavePreference) return;
            Enabled = !Enabled;
            Preferences.Set(_key, Enabled.ToString());
            Console.WriteLine($"Saved preference to {Enabled} for key {_key}");
        }

        public override string ToString()
        {
            return $"{_key} Enabled: {Enabled}";
        }
    }
}