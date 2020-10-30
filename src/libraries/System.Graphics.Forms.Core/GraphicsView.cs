using Xamarin.Forms;

namespace System.Graphics.Forms
{
    public class GraphicsView : View
    {
        public static readonly BindableProperty DrawableProperty = BindableProperty.Create(
            nameof(Drawable),
            typeof(EWDrawable),
            typeof(GraphicsView),
            null);

        public EWDrawable Drawable
        {
            get => (EWDrawable) GetValue(DrawableProperty);
            set => SetValue(DrawableProperty, value);
        }
    }
}