namespace Elevenworks.Graphics
{
    public static class EWDrawableExtensions
    {
        public static EWImage ToImage(this EWDrawable drawable, int width, int height, float scale = 1)
        {
            if (drawable == null) return null;

            using (var context = GraphicsPlatform.CurrentService.CreateBitmapExportContext(width, height))
            {
                context.Canvas.Scale(scale, scale);
                drawable.Draw(context.Canvas, new EWRectangle(0, 0, (float) width / scale, (float) height / scale));
                return context.Image;
            }
        }
    }
}