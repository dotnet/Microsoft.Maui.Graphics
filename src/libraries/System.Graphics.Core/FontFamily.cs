namespace System.Graphics
{
    public interface FontFamily
    {
        string Name { get; }
        IFontStyle[] GetFontStyles();
    }
}