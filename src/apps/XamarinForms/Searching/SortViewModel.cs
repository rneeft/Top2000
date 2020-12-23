﻿using Chroomsoft.Top2000.Features.Searching;

namespace Chroomsoft.Top2000.Apps.Searching
{
    public class SortViewModel
    {
        public SortViewModel(ISort value, string name)
        {
            this.Value = value;
            this.Name = name;
        }

        public ISort Value { get; }

        public string Name { get; }

        public override bool Equals(object obj)
        {
            return obj is SortViewModel svm && svm.Name == this.Name;
        }

        public override int GetHashCode() => this.Name.GetHashCode();
    }
}
