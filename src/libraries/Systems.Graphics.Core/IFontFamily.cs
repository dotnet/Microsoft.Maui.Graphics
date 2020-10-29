namespace Xamarin.Graphics
{
    public interface IFontFamily
    {
        string Name { get; }
        IFontStyle[] GetFontStyles();
    }
}