using System.Collections.Generic;
using CryptOverseeMobileApp.Models;

namespace CryptOverseeMobileApp.ViewModels.Settings
{
    public interface ISettings
    {
        void InitialiseSettings(List<ISpread> spreads);
        //IEnumerable<Spread> ApplySettings(IEnumerable<Spread> spreads);
    }
}