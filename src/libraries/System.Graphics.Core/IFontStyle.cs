using System.IO;

namespace System.Graphics
{
    public interface IFontStyle : IComparable<IFontStyle>
    {
        string Id { get; }
        string Name { get; }
        string FullName { get; }
        int Weight { get; }
        FontStyleType StyleType { get; }
        FontFamily FontFamily { get; }
        Stream OpenStream();
    }

    public enum FontStyleType
    {
        Normal,
        Italic,
        Oblique
    }
}