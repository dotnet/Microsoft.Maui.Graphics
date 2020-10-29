using System.Globalization;
using System.IO;

namespace Xamarin.Graphics
{
    public static class EWPathExtensions
    {
        public static string ToDefinitionString(this EWPath path, float ppu = 1)
        {
            var writer = new StringWriter();

            for (var i = 0; i < path.SegmentCount; i++)
            {
                var type = path.GetSegmentType(i);
                var points = path.GetPointsForSegment(i);

                if (type == PathOperation.MOVE_TO)
                {
                    writer.Write("M");
                    WritePoint(writer, points[0], ppu);
                }
                else if (type == PathOperation.LINE)
                {
                    writer.Write(" L");
                    WritePoint(writer, points[0], ppu);
                }
                else if (type == PathOperation.QUAD)
                {
                    writer.Write(" Q");
                    WritePoint(writer, points[0], ppu);
                    writer.Write(" ");
                    WritePoint(writer, points[1], ppu);
                }
                else if (type == PathOperation.CUBIC)
                {
                    writer.Write(" C");
                    WritePoint(writer, points[0], ppu);
                    writer.Write(" ");
                    WritePoint(writer, points[1], ppu);
                    writer.Write(" ");
                    WritePoint(writer, points[2], ppu);
                }
                else if (type == PathOperation.CLOSE)
                {
                    writer.Write(" Z ");
                }
            }

            return writer.ToString();
        }

        private static void WritePoint(StringWriter writer, EWImmutablePoint point, float ppu)
        {
            float x = point.X * ppu;
            float y = point.Y * ppu;

            string cx = x.ToString(CultureInfo.InvariantCulture);
            string cy = y.ToString(CultureInfo.InvariantCulture);

            writer.Write(cx);
            writer.Write(" ");
            writer.Write(cy);
        }

        public static EWPath AsScaledPath(
            this EWPath target,
            float scale)
        {
            var scaledPath = new EWPath(target);
            var transform = EWAffineTransform.GetScaleInstance(scale, scale);
            scaledPath.Transform(transform);
            return scaledPath;
        }

        public static bool IsPolyline(this EWPath target)
        {
            foreach (var segment in target.SegmentTypes)
            {
                switch (segment)
                {
                    case PathOperation.ARC:
                    case PathOperation.CUBIC:
                    case PathOperation.QUAD:
                        return false;
                }
            }

            return true;
        }
    }
}