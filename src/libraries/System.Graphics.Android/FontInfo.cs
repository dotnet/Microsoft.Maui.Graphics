namespace System.Graphics.Android
{
    public class FontInfo
    {
        public FontInfo(string aPath, string aFamily, string aStyle, string aFullName)
        {
            Path = aPath;
            Family = aFamily;
            Style = aStyle;
            FullName = aFullName;
        }

        public string Path { get; }
        public string Family { get; }
        public string Style { get; }
        public string FullName { get; }
    }
}