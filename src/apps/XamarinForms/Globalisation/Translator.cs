﻿using System.ComponentModel;

namespace Chroomsoft.Top2000.Apps.Globalisation
{
    public class Translator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public static Translator Instance { get; } = new Translator();

        public string this[string text]
        {
            get
            {
                var value = AppResources.ResourceManager.GetString(text, AppResources.Culture);

                return string.IsNullOrWhiteSpace(value)
                    ? "%" + text + "%"
                    : value;
            }
        }

        public void Invalidate()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
