using System;
using System.IO;

namespace Microsoft.Maui.Graphics
{
	public interface IFontStyle : IComparable<IFontStyle>
	{
		string Id { get; }
		string Name { get; }
		string FullName { get; }
		int Weight { get; }
		FontSlant StyleType { get; }
		IFontFamily FontFamily { get; }
		Stream OpenStream();
	}

	public enum FontSlant
	{
		Default,
		Italic,
		Oblique
	}
}
