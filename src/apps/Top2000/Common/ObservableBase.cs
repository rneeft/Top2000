﻿#nullable disable

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chroomsoft.Top2000.Apps.Common
{
    public abstract class ObservableBase : INotifyPropertyChanged
    {
        private readonly PropertyHelper propertyHelper;

        protected ObservableBase()
        {
            propertyHelper = new PropertyHelper(NotifyOfPropertyChange, this.GetType());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyOfPropertyChange([CallerMemberName] string propertyName = null)
        {
            _ = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected T GetPropertyValue<T>([CallerMemberName] string propertyName = null) => propertyHelper.GetPropertyValue<T>(propertyName);

        protected bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null) => propertyHelper.SetPropertyValue(newValue, propertyName);

        protected bool SetPropertyValueSilent<T>(T newValue, string propertyName = null) => propertyHelper.SetPropertyValueSilent(newValue, propertyName);
    }
}
