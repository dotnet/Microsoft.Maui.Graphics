﻿using System;

namespace Xamarin.Graphics.Mac
{
    public class MMFontStyle : IFontStyle
    {
        private readonly MMFontFamily _family;

        public MMFontStyle(MMFontFamily family, string id, string name, string fullName, int weight, FontStyleType styleType)
        {
            _family = family;
            Id = id;
            Name = name;
            FullName = fullName;
            Weight = weight;
            StyleType = styleType;
        }

        public IFontFamily FontFamily => _family;

        public string Id { get; }

        public string Name { get; }

        public string FullName { get; }

        public int Weight { get; }

        public FontStyleType StyleType { get; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(MMFontStyle))
                return false;
            MMFontStyle other = (MMFontStyle) obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id != null ? Id.GetHashCode() : 0);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(IFontStyle other)
        {
            if (Name.Equals("Regular") || Name.Equals("Plain") || Name.Equals("Normal"))
            {
                return -1;
            }
            else if (other.Name.Equals("Regular") || other.Name.Equals("Plain") || other.Name.Equals("Normal"))
            {
                return 1;
            }

            return String.Compare(Name, other.Name, StringComparison.Ordinal);
        }

        public System.IO.Stream OpenStream()
        {
            throw new NotImplementedException();
        }
    }
}