using System.Collections.Generic;
using CoreText;
using UIKit;

namespace System.Graphics.CoreGraphics
{
    public class MTFontFamily : FontFamily, IComparable<FontFamily>, IComparable
    {
        private readonly string _name;
        private IFontStyle[] _fontStyles;

        public MTFontFamily(string name)
        {
            _name = name;
        }

        public string Name => _name;

        public IFontStyle[] GetFontStyles()
        {
            return _fontStyles ?? (_fontStyles = InitializeFontStyles());
        }

        private IFontStyle[] InitializeFontStyles()
        {
            var ids = UIFont.FontNamesForFamilyName(_name);

            var styles = new List<IFontStyle>();

            if (ids != null)
            {
                var names = GetStyleNames(ids);

                for (int i = 0; i < ids.Length; i++)
                {
                    var id = ids[i];
                    var name = names[i];
                    var weight = FontUtils.GetFontWeight(name);
                    var styleType = FontUtils.GetStyleType(name);

                    var fullName = _name;
                    if (i > 0)
                        fullName = $"{_name} {name}";

                    styles.Add(new MTFontStyle(this, id, name, fullName, weight, styleType));
                }
            }

            styles.Sort();
            return styles.ToArray();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof(MTFontFamily))
                return false;
            MTFontFamily other = (MTFontFamily) obj;
            return _name == other._name;
        }

        public override int GetHashCode()
        {
            return (_name != null ? _name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name;
        }

        private string[] GetStyleNames(string[] fontIds)
        {
            string[] styleNames = new string[fontIds.Length];

            for (int i = 0; i < fontIds.Length; i++)
            {
                var font = new CTFont(fontIds[i], UIFont.SystemFontSize);
                styleNames[i] = font.GetName(CTFontNameKey.Style);
                font.Dispose();
            }

            return styleNames;
        }

        public int CompareTo(FontFamily other)
        {
            return string.Compare(_name, other.Name, StringComparison.Ordinal);
        }

        public int CompareTo(object obj)
        {
            if (obj is FontFamily other)
                return CompareTo(other);

            return -1;
        }
    }
}