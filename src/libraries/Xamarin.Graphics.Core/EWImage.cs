using System;
using System.IO;
using System.Threading.Tasks;

namespace Xamarin.Graphics
{
    public enum ResizeMode
    {
        Fit,
        Bleed,
        Stretch
    }

    public interface EWImage : EWDrawable, IDisposable
    {
        float Width { get; }
        float Height { get; }
        EWImage Downsize(float maxWidthOrHeight, bool disposeOriginal = false);
        EWImage Downsize(float maxWidth, float maxHeight, bool disposeOriginal = false);
        EWImage Resize(float width, float height, ResizeMode resizeMode = ResizeMode.Fit, bool disposeOriginal = false);
        void Save(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1);
        Task SaveAsync(Stream stream, EWImageFormat format = EWImageFormat.Png, float quality = 1);
    }
}