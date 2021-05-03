using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeFontFamily : IFontFamily, IComparable<IFontFamily>, IComparable {

		private readonly string _name;
		private IFontStyle[] _fontStyles = new IFontStyle[0];

		public NativeFontFamily(string name) {
			_name = name;

		}

		public string Name => _name;

		public IFontStyle[] GetFontStyles() {
			return _fontStyles ??= GetAvailableFontStyles();
		}

		private IFontStyle[] GetAvailableFontStyles() {
			throw new NotImplementedException();
		}

		public override bool Equals(object obj) {
			if (obj == null)
				return false;

			if (ReferenceEquals(this, obj))
				return true;

			if (obj.GetType() != typeof(NativeFontFamily))
				return false;

			var other = (NativeFontFamily) obj;

			return _name == other._name;
		}

		public override int GetHashCode() {
			return (_name != null ? _name.GetHashCode() : 0);
		}

		public override string ToString() {
			return Name;
		}

		public int CompareTo(IFontFamily other) {
			return string.Compare(_name, other.Name, StringComparison.Ordinal);
		}

		public int CompareTo(object obj) {
			if (obj is IFontFamily other)
				return CompareTo(other);

			return -1;
		}

	}

}