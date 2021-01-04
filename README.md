# System.Graphics

System.Graphics is a cross-platform graphics library for iOS, Android, Windows, macOS and Linux completely in C#.  With this library you can use a common API to target multiple abstractions allowing you to share your drawing code between platforms, or mix and match graphics implentations within a singular application.

# Motivation

Within the dotnet ecosystem there are multiple graphics libraries available depending on your target platforms; however, if you are doing cross-platform development there is not a unified graphics abstraction.  Some legacy API's (System.Drawing, I'm looking at you) only have limited support/usefulness on non-Windows platforms.

# Goals
* No dependencies on System.Drawing
* Support all graphics operations within an abstraction that the underlying abstraction supports.

# Status
This is an experimental library; however it's based on code that's been in use in production applications for over 10 years.  Because it was refactored out of another code base, some things may be broken, and or still include unreachable/non-applicable code that was part of that application.

Platform               | Supported Abstractions |
-----------------------|-------------------------------------------|
Xamarin.iOS            | CoreGraphics & SkiaSharp | 
Xamarin.Android        | Android.Graphics & SkiaSharp |
Xamarin.Mac            | CoreGraphics & SkiaSharp |
WPF                    | SharpDX, SkiaSharp & GDI |
UWP                    | SharpDX, Win2D, SkiaSharp |
WinForms               | SharpDX, SkiaSharp & GDI |
Linux | SkiaSharp |
Xamarin.Forms          | Dependent on native platform support (noted above) |

# Main Abstractions
* Canvas - You can draw to a any of the supported abstractions with a common drawing canvas API
* Images - You can manage, load and create images with a common API
* Fonts - You can access platform font information with a 
* Attributed text - You can draw attributed text with a common API
* PDF - You can create PDF's using the canvas abstraction in a cross-platform manner.

# Known Limitations
* Attributed text is not currently supported with SkiaSharp.
* The included Blazor (Canvas) implementation no longer compiles, but is included as a reminder to get it working again.
