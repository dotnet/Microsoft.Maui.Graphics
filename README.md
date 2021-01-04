# System.Graphics

System.Graphics is a cross-platform graphics library for iOS, Android, Windows, macOS and Linux completely in C#.

Platform               | Supported Abstractions |
-----------------------|-------------------------------------------|
Xamarin.iOS            |  CoreGraphics & SkiaSharp | 
Xamarin.Android        | Android.Graphics & SkiaSharp |
Xamarin.Mac            | CoreGraphics & SkiaSharp |
WPF                    | SharpDX, SkiaSharp & GDI |
WinForms               | SharpDX, SkiaSharp & GDI |
Linux | SkiaSharp |
Xamarin.Forms          | Dependent on native platform support (noted above) |

Supported Abstractions
* Drawing Canvas - You can draw to a any of the supported abstractions with a common drawing canvas API
* Images - You can manage, load and create images with a common API
* Fonts - You can access platform font information with a 
* Attributed text - You can draw attributed text with a common API
