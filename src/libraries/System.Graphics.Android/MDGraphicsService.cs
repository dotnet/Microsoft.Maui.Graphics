using System.IO;
using Android.Graphics;
using Android.Text;

namespace System.Graphics.Android
{
    public class MDGraphicsService : IGraphicsService
    {
        private const int White = unchecked((int) 0xFFFFFFFF);
        private const int Black = unchecked((int) 0xFF000000);
        public static readonly MDGraphicsService Instance = new MDGraphicsService();

        private static string _systemFontName;
        private static string _boldSystemFontName;

        private static global::Android.Graphics.Paint _strokePaint;
        private static global::Android.Graphics.Paint _fillPaint;
        private static Bitmap _b;

        private static float _pixelDensityFactor = 1;

        private MDGraphicsService()
        {
            if (_strokePaint == null)
            {
                _strokePaint = new global::Android.Graphics.Paint();
                _strokePaint.SetARGB(255, 255, 255, 255);
                _strokePaint.SetStyle(global::Android.Graphics.Paint.Style.Stroke);

                _fillPaint = new global::Android.Graphics.Paint();
                _fillPaint.SetARGB(255, 255, 255, 255);
                _fillPaint.SetStyle(global::Android.Graphics.Paint.Style.Fill);

                _b = Bitmap.CreateBitmap(1, 1, Bitmap.Config.Argb8888);
            }
        }

        public bool IsRetina => false;

        public IImage LoadImageFromStream(Stream stream, ImageFormat formatHint = ImageFormat.Png)
        {
            var bitmap = BitmapFactory.DecodeStream(stream);
            return new MDImage(bitmap);
        }

        public static float PixelDensityFactor
        {
            set => _pixelDensityFactor = value;
        }

        public string SystemFontName
        {
            get
            {
                if (_systemFontName == null)
                {
                    _systemFontName = MDFontService.SystemFont;
                    _boldSystemFontName = MDFontService.SystemBoldFont;
                }

                return _systemFontName;
            }
        }

        public string BoldSystemFontName
        {
            get
            {
                if (_boldSystemFontName == null)
                {
                    _systemFontName = MDFontService.SystemFont;
                    _boldSystemFontName = MDFontService.SystemBoldFont;
                }

                return _boldSystemFontName;
            }
        }

        #region GraphicsPlatform Members

        public string GetFont(string aFontName, bool aBold, bool aItalic)
        {
            return aFontName;
        }

        public string GetFontName(string aFontName)
        {
            return aFontName;
        }

        public string GetFontWeight(string aFontName)
        {
            return "normal";
        }

        public string GetFontStyle(string aFontName)
        {
            return "normal";
        }

        public EWSize GetStringSize(string value, string fontName, float fontSize)
        {
            if (value == null) return new EWSize();

            var textPaint = new TextPaint {TextSize = fontSize};
            textPaint.SetTypeface(MDFontService.Instance.GetTypeface(fontName));

            var staticLayout = MDTextLayout.CreateLayout(value, textPaint, null, Layout.Alignment.AlignNormal);
            var size = staticLayout.GetTextSizeAsEWSize(false);
            staticLayout.Dispose();
            return size;
        }

       

        public EWSize GetStringSize(string aString, string aFontName, float aFontSize, HorizontalAlignment aHorizontalAlignment, VerticalAlignment aVerticalAlignment)
        {
            if (aString == null) return new EWSize();

            var vTextPaint = new TextPaint {TextSize = aFontSize};
            vTextPaint.SetTypeface(MDFontService.Instance.GetTypeface(aFontName));

            Layout.Alignment vAlignment;
            switch (aHorizontalAlignment)
            {
                case HorizontalAlignment.Center:
                    vAlignment = Layout.Alignment.AlignCenter;
                    break;
                case HorizontalAlignment.Right:
                    vAlignment = Layout.Alignment.AlignOpposite;
                    break;
                default:
                    vAlignment = Layout.Alignment.AlignNormal;
                    break;
            }

            StaticLayout vLayout = MDTextLayout.CreateLayout(aString, vTextPaint, null, vAlignment);
            EWSize vSize = vLayout.GetTextSizeAsEWSize(false);
            vLayout.Dispose();
            return vSize;
        }
        
        #endregion
        
        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
        {
            return new MDBitmapExportContext(width, height, displayScale, 72, false, true);
        }
    }
}