using System;
using System.Collections.Generic;
using System.Linq;
using Pango;

namespace Microsoft.Maui.Graphics.Native.Gtk {

	public class NativeFontService : AbstractFontService {

		static Pango.Context? _systemContext;

		Pango.Context SystemContext => _systemContext ??= Gdk.PangoHelper.ContextGet();

		public override IFontFamily[] GetFontFamilies() {
			throw new NotImplementedException();
		}

		private IEnumerable<(Pango.FontFamily family, Pango.FontDescription description)> GetAvailableFamilyFaces(Pango.FontFamily family)
		{

			if (family != default)
			{
				foreach (var face in family.Faces)
					yield return (family, face.Describe());
			}

			yield break;
		}

		private FontDescription[] GetAvailableFontStyles()
		{
			var fontFamilies = SystemContext.FontMap?.Families.ToArray();

			var styles = new List<FontDescription>();

			if (fontFamilies != null)
			{
				styles.AddRange(fontFamilies.SelectMany(GetAvailableFamilyFaces).Select(font => font.description)
				   .OrderBy(d=>d.Family));
			}


			return styles.ToArray();
		}

	}

}
