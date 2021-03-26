using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using static System.Boolean;

namespace CryptOverseeMobileApp.ViewModels.Settings
{

    public class SettingItemViewModel : ViewModelBase
    {
        private bool _enabled;

        public SettingItemViewModel(string pageName, string exchange)
        {
            PageName = pageName;
            Name = exchange.Trim();
            AllowedToSavePreference = true;
                
            var key = Key;
            TryParse(Preferences.Get(key, "True"), out var enabled);
            Enabled = enabled;
        }

        public string PageName { get; set; }
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

        private string Key => $"{PageName}_{Name}";

        public ICommand SavePreferencesCommand => new Command(x => { SavePrefs(); });

        public void SavePrefs()
        {
            if (!AllowedToSavePreference) return;
            Preferences.Set(Key, Enabled.ToString());
            Console.WriteLine($"Saved preference to {Enabled} for key {Key}");
        }

        public void Toggle()
        {
            if (!AllowedToSavePreference) return;
            Enabled = !Enabled;
            Preferences.Set(Key, Enabled.ToString());
            Console.WriteLine($"Saved preference to {Enabled} for key {Key}");
        }

        public override string ToString()
        {
            return $"{Name} Enabled: {Enabled}, {PageName}";
        }
    }
}