using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Chroomsoft.Top2000.WindowsApp.Common
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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected T GetPropertyValue<T>([CallerMemberName] string propertyName = null) => propertyHelper.GetPropertyValue<T>(propertyName);

        protected bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null) => propertyHelper.SetPropertyValue(newValue, propertyName);

        protected bool SetPropertyValueSilent<T>(T newValue, string propertyName = null) => propertyHelper.SetPropertyValueSilent(newValue, propertyName);
    }

    /// <Remarks>
    /// All credits goes to: http://www.codecadwallader.com/2013/04/08/inotifypropertychanged-3-of-3-without-the-reversed-notifications/
    /// </Remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NotifiesOnAttribute : Attribute
    {
        public NotifiesOnAttribute(string name)
        {
            this.Name = name;
        }

        public string Name { get; }
    }

    /// <Remarks>
    /// All credits for the PropertyBackingDictionary goes to: http://www.codecadwallader.com/2013/04/06/inotifypropertychanged-2-of-3-without-the-backing-fields/
    /// </remarks>
    public class PropertyHelper
    {
        private readonly Dictionary<string, object> propertyBackingDictionary = new Dictionary<string, object>();
        private readonly Action<string> notifyOfPropertyChange;
        private readonly Type type;

        private ILookup<string, string> _dependentLookup;

        public PropertyHelper(Action<string> notifyOfPropertyChange, Type type)
        {
            this.notifyOfPropertyChange = notifyOfPropertyChange;
            this.type = type;
        }

        private ILookup<string, string> DependentLookup
        {
            get
            {
                return _dependentLookup ?? (_dependentLookup = (from p in type.GetProperties()
                                                                let attrs = p.GetCustomAttributes(typeof(NotifiesOnAttribute), false)
                                                                from NotifiesOnAttribute a in attrs
                                                                select new { Independent = a.Name, Dependent = p.Name }).ToLookup(i => i.Independent, d => d.Dependent));
            }
        }

        public T GetPropertyValue<T>([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            if (propertyBackingDictionary.TryGetValue(propertyName, out object value))
                return (T)value;

            return default(T);
        }

        public bool SetPropertyValue<T>(T newValue, [CallerMemberName] string propertyName = null)
        {
            var set = SetPropertyValueSilent(newValue, propertyName);
            if (set)
                RaisePropertyChanged(propertyName);

            return set;
        }

        public bool SetPropertyValueSilent<T>(T newValue, string propertyName)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            if (EqualityComparer<T>.Default.Equals(newValue, GetPropertyValue<T>(propertyName))) return false;

            propertyBackingDictionary[propertyName] = newValue;
            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

            if (notifyOfPropertyChange != null)
            {
                notifyOfPropertyChange(propertyName);
                foreach (var dependentPropertyName in DependentLookup[propertyName])
                    RaisePropertyChanged(dependentPropertyName);
            }
        }
    }
}
