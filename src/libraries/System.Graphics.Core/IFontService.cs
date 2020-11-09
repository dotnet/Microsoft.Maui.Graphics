namespace System.Graphics
{
    public interface IFontService
    {
        FontFamily[] GetFontFamilies();
        IFontStyle GetFontStyleById(string id);
        IFontStyle GetDefaultFontStyle();
    }
}