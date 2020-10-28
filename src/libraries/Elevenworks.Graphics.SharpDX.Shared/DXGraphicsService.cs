using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.Mathematics.Interop;
using SharpDX.WIC;
using Factory = SharpDX.Direct2D1.Factory;
using FactoryType = SharpDX.Direct2D1.FactoryType;

namespace Elevenworks.Graphics.SharpDX
{
    public class DXGraphicsService : IGraphicsService
    {
        private const float Dip = 96f / 72f;

        public static Factory SharedFactory = new Factory(FactoryType.MultiThreaded);

        public static readonly ThreadLocal<Factory> CurrentFactory = new ThreadLocal<Factory>();
        public static readonly ThreadLocal<RenderTarget> CurrentTarget = new ThreadLocal<RenderTarget>();
        public static ImagingFactory2 FactoryImaging = new ImagingFactory2();

        public static global::SharpDX.DirectWrite.Factory FactoryDirectWrite = new global::SharpDX.DirectWrite.Factory(global::SharpDX.DirectWrite.FactoryType.Shared);

        public static readonly DXGraphicsService Instance = new DXGraphicsService();

        private DXGraphicsService()
        {
        }

        public bool IsRetina => false;

        public string SystemFontName => "Arial";
        public string BoldSystemFontName => "Arial-Bold";

        public EWSize GetStringSize(string value, string fontName, float textSize)
        {
            if (value == null) return new EWSize();

            float fontSize = textSize;
            float factor = 1;
            while (fontSize > 14)
            {
                fontSize /= 14;
                factor *= 14;
            }

            if (fontName == null)
                fontName = "Arial";

            var size = new EWSize();

            var textFormat = new TextFormat(FactoryDirectWrite, fontName, fontSize);
            textFormat.TextAlignment = TextAlignment.Leading;
            textFormat.ParagraphAlignment = ParagraphAlignment.Near;

            var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512, 512);
            size.Width = textLayout.Metrics.Width;
            size.Height = textLayout.Metrics.Height;

            size.Width *= factor;
            size.Height *= factor;

            return size;
        }

        public List<EWPath> ConvertToPaths(EWPath aPath, string text, ITextAttributes textAttributes, float aPPU, float aZoom)
        {
            throw new NotImplementedException();
        }

        public void LayoutText(EWPath path, string text, ITextAttributes textAttributes, LayoutLine callback)
        {
            float fontSize = textAttributes.FontSize;
            float factor = 1;
            while (fontSize > 14)
            {
                fontSize /= 14;
                factor *= 14;
            }

            var horizontalAlignment = textAttributes.HorizontalAlignment;
            var verticalAlignment = textAttributes.VerticalAlignment;

            var textFormat = new TextFormat(FactoryDirectWrite, SystemFontName, FontWeight.Regular, FontStyle.Normal, fontSize);

            if (horizontalAlignment == EWHorizontalAlignment.LEFT)
            {
                textFormat.TextAlignment = TextAlignment.Leading;
            }
            else if (horizontalAlignment == EWHorizontalAlignment.CENTER)
            {
                textFormat.TextAlignment = TextAlignment.Center;
            }
            else if (horizontalAlignment == EWHorizontalAlignment.RIGHT)
            {
                textFormat.TextAlignment = TextAlignment.Trailing;
            }
            else if (horizontalAlignment == EWHorizontalAlignment.JUSTIFIED)
            {
                textFormat.TextAlignment = TextAlignment.Justified;
            }

            if (verticalAlignment == EWVerticalAlignment.TOP)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Near;
            }
            else if (verticalAlignment == EWVerticalAlignment.CENTER)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Center;
            }
            else if (verticalAlignment == EWVerticalAlignment.BOTTOM)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Far;
            }

            var textLayout = new TextLayout(FactoryDirectWrite, text, textFormat, 512f, 512f, Dip, false);

            var bounds = path.Bounds;
            var startPosition = 0;
            var x = bounds.MinX;
            var y = bounds.MinY;

            foreach (var metric in textLayout.GetLineMetrics())
            {
                y += metric.Height - fontSize * .3f;

                var lineText = text.Substring(startPosition, metric.Length);

                var actualX = x * factor;
                var actualY = y * factor;

                callback(new EWPoint(actualX, actualY), textAttributes, lineText, 0, 0, 0);
                startPosition += metric.Length;
            }
        }

        public EWSize GetStringSize(
            string value,
            string fontName,
            float textSize,
            EWHorizontalAlignment horizontalAlignment,
            EWVerticalAlignment verticalAlignment)
        {
            if (value == null) return new EWSize();

            float fontSize = textSize;
            float factor = 1;
            while (fontSize > 14)
            {
                fontSize /= 14;
                factor *= 14;
            }

            var size = new EWSize();

            var textFormat = new TextFormat(FactoryDirectWrite, SystemFontName, FontWeight.Regular, FontStyle.Normal,
                fontSize);
            if (horizontalAlignment == EWHorizontalAlignment.LEFT)
            {
                textFormat.TextAlignment = TextAlignment.Leading;
            }
            else if (horizontalAlignment == EWHorizontalAlignment.CENTER)
            {
                textFormat.TextAlignment = TextAlignment.Center;
            }
            else if (horizontalAlignment == EWHorizontalAlignment.RIGHT)
            {
                textFormat.TextAlignment = TextAlignment.Trailing;
            }
            else if (horizontalAlignment == EWHorizontalAlignment.JUSTIFIED)
            {
                textFormat.TextAlignment = TextAlignment.Justified;
            }

            if (verticalAlignment == EWVerticalAlignment.TOP)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Near;
            }
            else if (verticalAlignment == EWVerticalAlignment.CENTER)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Center;
            }
            else if (verticalAlignment == EWVerticalAlignment.BOTTOM)
            {
                textFormat.ParagraphAlignment = ParagraphAlignment.Far;
            }

            var textLayout = new TextLayout(FactoryDirectWrite, value, textFormat, 512f, 512f, Dip, false);
            size.Width = textLayout.Metrics.Width;
            size.Height = textLayout.Metrics.Height;


            size.Width *= factor;
            size.Height *= factor;

            return size;
        }

        public EWRectangle GetPathBounds(EWPath path)
        {
            if (path == null) return null;

            if (path.NativePath is PathGeometry nativePath)
            {
                return nativePath.GetBounds().AsEWRectangle();
            }

            if (CurrentFactory.Value != null && path.Closed)
            {
                if (CurrentFactory.Value != SharedFactory)
                {
                    nativePath = path.AsDxPath(CurrentFactory.Value);
                    if (nativePath != null)
                    {
                        path.NativePath = nativePath;
                        return nativePath.GetBounds().AsEWRectangle();
                    }
                }
            }

            /*
            if (SharedFactory != null)
            {
                try
                {
                    var vGeometry = path.AsDxPath(SharedFactory);
                    var vBounds = vGeometry.GetBounds();
                    var vRect = vBounds.AsEWRectangle();
                    vGeometry.Dispose();
                    return vRect;
                }
                catch (Exception exc)
                {
                    Logger.Debug(exc);
                }
            }*/

            var bounds = path.GetBoundsByFlattening();
            return bounds;
        }

        public EWRectangle GetPathBoundsWhenRotated(EWImmutablePoint center, EWPath path, float angle)
        {
            if (path == null) return null;
            var rotatedPath = path.Rotate(angle, center);
            var bounds = GetPathBounds(rotatedPath);
            rotatedPath.Dispose();
            return bounds;
        }

        public bool PathContainsPoint(EWPath path, EWImmutablePoint point, float ppu, float zoom, float strokeWidth)
        {
            if (path == null) return false;

            var nativePoint = new RawVector2
            {
                X = point.X,
                Y = point.Y
            };

            if (path.NativePath is PathGeometry nativePath)
            {
                return nativePath.FillContainsPoint(nativePoint);
            }

            if (CurrentFactory.Value != null)
            {
                nativePath = path.AsDxPath(1, 1, CurrentFactory.Value);
                if (nativePath != null)
                {
                    path.NativePath = nativePath;
                    return nativePath.FillContainsPoint(nativePoint);
                }
            }

            if (SharedFactory != null)
            {
                try
                {
                    nativePath = path.AsDxPath(1, 1, SharedFactory);
                    var pathContainsPoint = nativePath.FillContainsPoint(nativePoint);
                    //nativePath.Dispose();
                    return pathContainsPoint;
                }
                catch (Exception exc)
                {
                    Logger.Debug(exc);
                }
            }

            return Geometry.FlattenedPathContains(path, point);
        }


        public bool PointIsOnPath(EWPath path, EWImmutablePoint point, float ppu, float zoom, float strokeWidth)
        {
            var nativePoint = new RawVector2
            {
                X = point.X * ppu,
                Y = point.Y * ppu
            };
            var widthToTest = (10f / zoom) + strokeWidth;

            PathGeometry nativePath;
            if (path.NativePathPPU == ppu)
            {
                nativePath = path.NativePathAtPPU as PathGeometry;
                if (nativePath != null)
                {
                    return nativePath.StrokeContainsPoint(nativePoint, widthToTest);
                }
            }

            if (CurrentFactory.Value != null)
            {
                nativePath = path.AsDxPath(ppu, 1, CurrentFactory.Value);
                if (nativePath != null)
                {
                    path.NativePathAtPPU = nativePath;
                    path.NativePathPPU = ppu;
                    return nativePath.StrokeContainsPoint(nativePoint, widthToTest);
                }
            }

            if (SharedFactory != null)
            {
                nativePath = path.AsDxPath(ppu, 1, SharedFactory);
                var pointIsOnPath = nativePath.StrokeContainsPoint(nativePoint, widthToTest);
                //nativePath.Dispose();
                return pointIsOnPath;
            }

            return false;
        }

        public bool PointIsOnPathSegment(EWPath path, int segmentIndex, EWImmutablePoint point, float ppu, float zoom, float strokeWidth)
        {
            var vFactory = CurrentFactory.Value ?? SharedFactory;

            try
            {
                var nativePath = path.AsDxPathFromSegment(segmentIndex, ppu, 1, vFactory);
                var nativePoint = new RawVector2
                {
                    X = point.X * ppu,
                    Y = point.Y * ppu
                };
                var widthToTest = (10f / zoom) + strokeWidth;
                var pointIsOnPath = nativePath.StrokeContainsPoint(nativePoint, widthToTest);
                //nativePath.Dispose();
                return pointIsOnPath;
            }
            catch (Exception exc)
            {
                Logger.Debug(exc);
                throw;
            }
        }

        public EWImage LoadImageFromStream(Stream stream, string hash = null, EWImageFormat format = EWImageFormat.Png)
        {
            var bitmap = CurrentTarget.Value.LoadBitmap(stream);
            return new DXImage(bitmap, hash);
        }

        public BitmapExportContext CreateBitmapExportContext(int width, int height, float displayScale = 1)
        {
            return new DXBitmapExportContext(width, height, displayScale, 72, false);
        }
    }
}