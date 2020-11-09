namespace System.Graphics
{
    public class VirtualFontService : AbstractFontService
    {
        private static readonly FontFamily[] EmptyArray = { };

        public override FontFamily[] GetFontFamilies()
        {
            return EmptyArray;
        }
    }
}