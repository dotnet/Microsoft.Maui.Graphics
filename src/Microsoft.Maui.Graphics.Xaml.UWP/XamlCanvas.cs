using System;
using System.Collections.Generic;
using System.Numerics;

using Microsoft.Maui.Graphics.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Maui.Graphics.Xaml
{
	public enum ItemType
	{
		DrawLine,
		DrawRectangle,
		DrawRoundedRectangle,
		DrawEllipse,
		DrawArc,
		DrawPath,
		DrawText,
		DrawTextInRect,
		DrawImage,
		FillRectangle,
		FillRoundedRectangle,
		FillEllipse,
		FillArc,
		FillPath
	}

	public class Item
	{
		public ItemType Type;
		public UIElement Element;
	}

	public class XamlCanvas : AbstractCanvas<XamlCanvasState>
	{
		private readonly List<Item> _items = new List<Item>();

		private Canvas _canvas;
		private int _index;
		private float _rectX;
		private float _rectY;
		private float _rectWidth;
		private float _rectHeight;
		private float _rectCornerRadius;

		public XamlCanvas()
			: base(CreateNewState, CreateStateCopy)
		{
		}

		public Canvas Canvas
		{
			get => _canvas;
			set
			{
				_canvas = value;
				_index = 0;
				Trim();
			}
		}

		public void SetCanvas(Canvas value, bool trim = true)
		{
			_canvas = value;
			_index = 0;
			if (trim) Trim();
		}

		private static XamlCanvasState CreateNewState(object context)
		{
			return new XamlCanvasState();
		}

		private static XamlCanvasState CreateStateCopy(XamlCanvasState prototype)
		{
			return new XamlCanvasState(prototype);
		}

		public XamlCanvasSession CreateSession()
		{
			return new XamlCanvasSession(this);
		}

		internal void BeginDrawing()
		{
			ResetState();
			_index = 0;
		}

		internal void EndDrawing()
		{
			Trim();
		}

		private void Trim()
		{
			if (_index < _items.Count)
			{
				var n = _items.Count - _index;
				for (var i = 0; i < n; i++)
				{
					_canvas.Children.Remove(_items[_index + i].Element);
				}
				_items.RemoveRange(_index, n);
			}
		}

		private Item GetOrCreateItem(ItemType type)
		{
			Item item;

			if (_index >= _items.Count)
			{
				System.Diagnostics.Debug.WriteLine("Creating item of type {0}", type);

				item = CreateItem(type);
				_items.Add(item);
				_canvas.Children.Add(item.Element);
			}
			else
			{
				item = _items[_index];

				if (item.Type != type)
				{
					System.Diagnostics.Debug.WriteLine("Item types diverge at index {0}.  Wanted {1}, but found {2}", index, type, items[index].Type);

					Trim();
					item = CreateItem(type);
					_items.Add(item);
					_canvas.Children.Add(item.Element);
				}
			}


			_index++;
			return item;
		}

		private Item CreateItem(ItemType type)
		{
			var element = CreateElement(type);
			var item = new Item
			{
				Type = type,
				Element = element
			};

			return item;
		}

		private UIElement CreateElement(ItemType type)
		{
			switch (type)
			{
				case ItemType.DrawLine:
					return new Line();
				case ItemType.DrawRectangle:
				case ItemType.DrawRoundedRectangle:
				case ItemType.FillRectangle:
				case ItemType.FillRoundedRectangle:
					return new global::Windows.UI.Xaml.Shapes.Rectangle();
				case ItemType.DrawEllipse:
				case ItemType.FillEllipse:
					return new Ellipse();
				case ItemType.DrawArc:
				case ItemType.FillArc:
					var figure = new PathFigure();
					figure.Segments.Add(new ArcSegment());
					var geometry = new PathGeometry();
					geometry.Figures.Add(figure);
					var path = new Path
					{
						Data = geometry
					};
					return path;
				case ItemType.DrawPath:
				case ItemType.FillPath:
					return new Path();
				case ItemType.DrawText:
					return new TextBlock();
				case ItemType.DrawTextInRect:
					var textBlock = new TextBlock();
					var border = new Border {Child = textBlock};
					return border;
			}

			return new global::Windows.UI.Xaml.Shapes.Rectangle();
		}

		private void CreateFillRect(float x, float y, float width, float height)
		{
			_rectX = x;
			_rectY = y;
			_rectWidth = width;
			_rectHeight = height;
		}

		private void CreateStrokeRoundedRect(float x, float y, float width, float height, float cornerRadius)
		{
			CreateStrokeRect(x,y,width,height);

			if (cornerRadius <= 0)
			{
				_rectCornerRadius = 0;
				return;
			}

			_rectCornerRadius = cornerRadius;
		}

		private void CreateStrokeRect(float x, float y, float width, float height)
		{
			var strokeSize = CurrentState.StrokeSize;
			var halfStroke = strokeSize/2;

			_rectX = x - halfStroke;
			_rectY = y - halfStroke;
			_rectWidth = width + strokeSize;
			_rectHeight = height + strokeSize;
		}

		private void CreateArcStrokeRect(float x, float y, float width, float height)
		{
			_rectX = x;
			_rectY = y;
			_rectWidth = width;
			_rectHeight = height;

		}

		protected override void PlatformConcatenateTransform(Matrix3x2 transform)
		{
			CurrentState.XamlConcatenateTransform(transform);
		}

		protected override void PlatformTranslate(float tx, float ty)
		{
			CurrentState.XamlTranslate(tx, ty);
		}

		protected override void PlatformScale(float fx, float fy)
		{
			CurrentState.XamlScale(fx, fy);
		}

		protected override void PlatformRotate(float degrees, float radians)
		{
			CurrentState.XamlRotate(degrees, radians);
		}

		protected override void PlatformRotate(float degrees, float radians, float x, float y)
		{
			CurrentState.XamlRotate(degrees, radians, x, y);
		}

		protected override void PlatformDrawPath(PathF path)
		{
			var item = GetOrCreateItem(ItemType.DrawPath);
			var element = (Path)item.Element;
			var transformedPath = path;
			if (!CurrentState.Transform.IsIdentity)
			{
				transformedPath = new PathF(path);
				transformedPath.Transform(CurrentState.Transform);
			}
			var geometry = transformedPath.AsPathGeometry();
			element.Data = geometry;

			element.Stroke = CurrentState.XamlStrokeBrush;
			element.StrokeThickness = CurrentState.StrokeSize;
			element.StrokeDashArray = CurrentState.XamlDashArray;
			element.StrokeDashOffset = CurrentState.StrokeDashOffset;
			element.StrokeEndLineCap = CurrentState.XamlLineCap;
			element.StrokeStartLineCap = CurrentState.XamlLineCap;
			element.StrokeMiterLimit = CurrentState.MiterLimit;
			element.StrokeLineJoin = CurrentState.XamlLineJoin;
			element.Opacity = CurrentState.Alpha;
		}

		protected override void PlatformDrawEllipse(float x, float y, float width, float height)
		{
			var item = GetOrCreateItem(ItemType.DrawEllipse);
			var element = (Ellipse)item.Element;

			CreateStrokeRect(x, y, width, height);

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);
			element.Width = _rectWidth;
			element.Height = _rectHeight;

			element.Stroke = CurrentState.XamlStrokeBrush;
			element.StrokeThickness = CurrentState.StrokeSize;
			element.StrokeDashArray = CurrentState.XamlDashArray;
			element.StrokeDashOffset = CurrentState.StrokeDashOffset;
			element.StrokeEndLineCap = CurrentState.XamlLineCap;
			element.StrokeStartLineCap = CurrentState.XamlLineCap;
			element.StrokeMiterLimit = CurrentState.MiterLimit;
			element.StrokeLineJoin = CurrentState.XamlLineJoin;
			element.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		protected override void PlatformDrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
		{
			var item = GetOrCreateItem(ItemType.DrawRoundedRectangle);
			var element = (global::Windows.UI.Xaml.Shapes.Rectangle)item.Element;

			CreateStrokeRoundedRect(x, y, width, height, cornerRadius);

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);
			element.Width = _rectWidth;
			element.Height = _rectHeight;
			element.RadiusX = _rectCornerRadius;
			element.RadiusY = _rectCornerRadius;

			element.Stroke = CurrentState.XamlStrokeBrush;
			element.StrokeThickness = CurrentState.StrokeSize;
			element.StrokeDashArray = CurrentState.XamlDashArray;
			element.StrokeDashOffset = CurrentState.StrokeDashOffset;
			element.StrokeEndLineCap = CurrentState.XamlLineCap;
			element.StrokeStartLineCap = CurrentState.XamlLineCap;
			element.StrokeMiterLimit = CurrentState.MiterLimit;
			element.StrokeLineJoin = CurrentState.XamlLineJoin;
			element.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		protected override void PlatformDrawRectangle(float x, float y, float width, float height)
		{
			var item = GetOrCreateItem(ItemType.DrawRectangle);
			var element = (global::Windows.UI.Xaml.Shapes.Rectangle)item.Element;

			CreateStrokeRect(x,y,width,height);

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);
			element.Width = _rectWidth;
			element.Height = _rectHeight;
			element.RadiusX = 0;
			element.RadiusY = 0;

			element.Stroke = CurrentState.XamlStrokeBrush;
			element.StrokeThickness = CurrentState.StrokeSize;
			element.StrokeDashArray = CurrentState.XamlDashArray;
			element.StrokeDashOffset = CurrentState.StrokeDashOffset;
			element.StrokeEndLineCap = CurrentState.XamlLineCap;
			element.StrokeStartLineCap = CurrentState.XamlLineCap;
			element.StrokeMiterLimit = CurrentState.MiterLimit;
			element.StrokeLineJoin = CurrentState.XamlLineJoin;
			element.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		protected override void PlatformDrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
		{
			while (startAngle < 0)
			{
				startAngle += 360;
			}

			while (endAngle < 0)
			{
				endAngle += 360;
			}

			var item = GetOrCreateItem(ItemType.DrawArc);
			var element = (Path)item.Element;

			CreateArcStrokeRect(x, y, width, height);

			var geometry = (PathGeometry)element.Data;
			var figure = geometry.Figures[0];
			var arcSegment = (ArcSegment)figure.Segments[0];

			var sweep = GeometryUtil.GetSweep(startAngle,endAngle,clockwise);
			var absSweep = Math.Abs(sweep);
			var startPoint = GeometryUtil.EllipseAngleToPoint(_rectX, _rectY, _rectWidth, _rectHeight, -startAngle);
			var endPoint = GeometryUtil.EllipseAngleToPoint(_rectX, _rectY, _rectWidth, _rectHeight, -endAngle);

			figure.StartPoint = new global::Windows.Foundation.Point(startPoint.X, startPoint.Y);
			arcSegment.Point = new global::Windows.Foundation.Point(endPoint.X, endPoint.Y);
			arcSegment.Size = new global::Windows.Foundation.Size(_rectWidth / 2, _rectHeight /2);
			arcSegment.SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
			arcSegment.IsLargeArc = absSweep >= 180;

			if (closed)
			{
				LineSegment lineSegment;

				if (figure.Segments.Count == 1)
				{
					lineSegment = new LineSegment();
					figure.Segments.Add(lineSegment);
				}
				else
				{
					lineSegment = (LineSegment)figure.Segments[1];
				}

				lineSegment.Point = new global::Windows.Foundation.Point(startPoint.X, startPoint.Y);
			}
			else if (figure.Segments.Count > 1)
			{
				while (figure.Segments.Count > 1)
				{
					figure.Segments.RemoveAt(1);
				}
			}

			element.Stroke = CurrentState.XamlStrokeBrush;
			element.StrokeThickness = CurrentState.StrokeSize;
			element.StrokeDashArray = CurrentState.XamlDashArray;
			element.StrokeDashOffset = CurrentState.StrokeDashOffset;
			element.StrokeEndLineCap = CurrentState.XamlLineCap;
			element.StrokeStartLineCap = CurrentState.XamlLineCap;
			element.StrokeMiterLimit = CurrentState.MiterLimit;
			element.Opacity = CurrentState.Alpha;

			var pathX = geometry.Bounds.Left;
			var pathY = geometry.Bounds.Top;
			element.RenderTransform = CurrentState.GetXamlTransform(pathX, pathY);
		}

		protected override void PlatformDrawLine(float x1, float y1, float x2, float y2)
		{
			var item = GetOrCreateItem(ItemType.DrawLine);
			var element = (Line)item.Element;

			var p1 = Vector2.Transform(new Vector2(x1, y1), CurrentState.Transform);
			var p2 = Vector2.Transform(new Vector2(x2, y2), CurrentState.Transform);
			element.X1 = p1.X;
			element.Y1 = p1.Y;
			element.X2 = p2.X;
			element.Y2 = p2.Y;

			element.Stroke = CurrentState.XamlStrokeBrush;
			element.StrokeThickness = CurrentState.StrokeSize;
			try
			{
				element.StrokeDashArray = CurrentState.XamlDashArray;
			}
			catch (Exception exc)
			{
				System.Diagnostics.Debug.WriteLine(exc);
			}
			element.StrokeDashOffset = CurrentState.StrokeDashOffset;
			element.StrokeEndLineCap = CurrentState.XamlLineCap;
			element.StrokeStartLineCap = CurrentState.XamlLineCap;
			element.StrokeMiterLimit = CurrentState.MiterLimit;
			element.Opacity = CurrentState.Alpha;
		}

		protected override void PlatformSetStrokeDashPattern(float[] strokePattern, float strokeDashOffset, float strokeSize)
		{
			CurrentState.XamlDashArray = null;
		}

		protected override float PlatformStrokeSize
		{
			set {  }
		}

		public override void DrawImage(IImage image, float x, float y, float width, float height)
		{

		}
		public override void SetFillPaint(Paint paint, RectF rectangle)
		{
			if (paint is SolidPaint solidPaint)
				FillColor = solidPaint.Color;

			CurrentState.SetFillPaint(paint, rectangle);
		}

		public override void SetShadow(SizeF offset, float blur, Color color)
		{
			CurrentState.SetShadow(offset, blur, color);
		}

		public override void DrawString(
			string value,
			float x,
			float y,
			float width,
			float height,
			HorizontalAlignment horizontalAlignment,
			VerticalAlignment verticalAlignment,
			TextFlow textFlow = TextFlow.ClipBounds,
			float lineAdjustment = 0)
		{
			if (string.IsNullOrWhiteSpace(value))
				return;

			CreateFillRect(x, y, width, height);

			var item = GetOrCreateItem(ItemType.DrawTextInRect);
			var element = (Border)item.Element;
			var block = (TextBlock) element.Child;

			block.Text = value;

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);
			element.Width = _rectWidth;
			element.Height = _rectHeight;

			switch (horizontalAlignment)
			{
				case HorizontalAlignment.Left:
					block.TextAlignment = TextAlignment.Left;
					break;
				case HorizontalAlignment.Center:
					block.TextAlignment = TextAlignment.Center;
					break;
				case HorizontalAlignment.Right:
					block.TextAlignment = TextAlignment.Right;
					break;
				case HorizontalAlignment.Justified:
					block.TextAlignment = TextAlignment.Justify;
					break;
			}

			switch (verticalAlignment)
			{
				case VerticalAlignment.Top:
					block.VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Top;
					break;
				case VerticalAlignment.Center:
					block.VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Center;
					break;
				case VerticalAlignment.Bottom:
					block.VerticalAlignment = global::Windows.UI.Xaml.VerticalAlignment.Bottom;
					break;
			}

			block.Foreground = CurrentState.XamlFontBrush;
			block.FontSize = CurrentState.FontSize;
			block.FontFamily = CurrentState.FontFamily;
			block.FontStyle = CurrentState.FontStyle;
			block.FontWeight = CurrentState.FontWeight;
			block.TextTrimming = TextTrimming.None;
			block.TextWrapping = TextWrapping.Wrap;
			block.Padding = new Thickness();
			block.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		public override void DrawString(string value, float x, float y, HorizontalAlignment horizontalAlignment)
		{
			var item = GetOrCreateItem(ItemType.DrawText);
			var element = (TextBlock) item.Element;
			element.Text = value;

			switch (horizontalAlignment)
			{
				case HorizontalAlignment.Left:
					element.TextAlignment = TextAlignment.Left;
					break;
				case HorizontalAlignment.Center:
					element.TextAlignment = TextAlignment.Center;
					break;
				case HorizontalAlignment.Right:
					element.TextAlignment = TextAlignment.Right;
					break;
				case HorizontalAlignment.Justified:
					element.TextAlignment = TextAlignment.Justify;
					break;
			}

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);

			element.FontSize = CurrentState.FontSize;
			element.FontFamily = CurrentState.FontFamily;
			element.FontStyle = CurrentState.FontStyle;
			element.FontWeight = CurrentState.FontWeight;
			element.TextTrimming = TextTrimming.None;
			element.TextWrapping = TextWrapping.NoWrap;
			element.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		public override void DrawText(IAttributedText value, float x, float y, float width, float height)
		{
			System.Diagnostics.Debug.WriteLine("XamlCanvas.DrawText not yet implemented.");
			DrawString(value?.Text, x, y, width, height, HorizontalAlignment.Left, VerticalAlignment.Top);
		}

		public override void FillEllipse(float x, float y, float width, float height)
		{
			var item = GetOrCreateItem(ItemType.FillEllipse);
			var element = (Ellipse)item.Element;

			CreateFillRect(x, y, width, height);

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);
			element.Width = _rectWidth;
			element.Height = _rectHeight;

			element.Fill = CurrentState.XamlFillBrush;
			element.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		public override void FillRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
		{
			var item = GetOrCreateItem(ItemType.FillRoundedRectangle);
			var element = (global::Windows.UI.Xaml.Shapes.Rectangle)item.Element;

			CreateFillRect(x, y, width, height);

			var halfHeight = Math.Abs(_rectHeight / 2);
			if (cornerRadius > halfHeight)
			{
				cornerRadius = halfHeight;
			}

			var halfWidth = Math.Abs(_rectWidth / 2);
			if (cornerRadius > halfWidth)
			{
				cornerRadius = halfWidth;
			}

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);
			element.Width = _rectWidth;
			element.Height = _rectHeight;
			element.RadiusX = cornerRadius;
			element.RadiusY = cornerRadius;

			element.Fill = CurrentState.XamlFillBrush;
			element.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		public override void FillRectangle(float x, float y, float width, float height)
		{
			var item = GetOrCreateItem(ItemType.FillRectangle);
			var element = (global::Windows.UI.Xaml.Shapes.Rectangle)item.Element;

			CreateFillRect(x, y, width, height);

			Canvas.SetLeft(element, _rectX);
			Canvas.SetTop(element, _rectY);
			element.Width = _rectWidth;
			element.Height = _rectHeight;
			element.RadiusX = 0;
			element.RadiusY = 0;

			element.Fill = CurrentState.XamlFillBrush;
			element.Opacity = CurrentState.Alpha;
			element.RenderTransform = CurrentState.GetXamlTransform(_rectX, _rectY);
		}

		public override float MiterLimit
		{
			set => CurrentState.MiterLimit = value;
		}

		public override Color StrokeColor
		{
			set => CurrentState.StrokeColor = value;
		}

		public override LineCap StrokeLineCap
		{
			set => CurrentState.StrokeLineCap = value;
		}

		public override LineJoin StrokeLineJoin
		{
			set => CurrentState.StrokeLineJoin = value;
		}

		public override Color FillColor
		{
			set => CurrentState.FillColor = value;
		}

		public override Color FontColor
		{
			set => CurrentState.FontColor = value;
		}

		public override IFont Font
		{
			set => CurrentState.Font = value;
		}

		public override float FontSize
		{
			set => CurrentState.FontSize = value;
		}

		public override float Alpha
		{
			set => CurrentState.Alpha = value;
		}

		public override bool Antialias
		{
			set { }
		}

		public override BlendMode BlendMode
		{
			set {  }
		}

		public override void FillPath(PathF path, WindingMode windingMode)
		{
			var item = GetOrCreateItem(ItemType.FillPath);
			var element = (Path)item.Element;
			var transformedPath = path;
			if (!CurrentState.Transform.IsIdentity)
			{
				transformedPath = new PathF(path);
				transformedPath.Transform(CurrentState.Transform);
			}
			var geometry = transformedPath.AsPathGeometry();
			element.Data = geometry;

			element.Fill = CurrentState.XamlFillBrush;
			element.Opacity = CurrentState.Alpha;
			geometry.FillRule = windingMode == WindingMode.NonZero ? FillRule.Nonzero : FillRule.EvenOdd;

		}

		public override void SubtractFromClip(float x, float y, float width, float height)
		{

		}

		public override void ClipPath(PathF path, WindingMode windingMode = WindingMode.NonZero)
		{

		}

		public override void ClipRectangle(float x, float y, float width, float height)
		{

		}

		public override void FillArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise)
		{
			while (startAngle < 0)
			{
				startAngle += 360;
			}

			while (endAngle < 0)
			{
				endAngle += 360;
			}

			var item = GetOrCreateItem(ItemType.FillArc);
			var element = (Path)item.Element;

			CreateFillRect(x, y, width, height);

			var geometry = (PathGeometry)element.Data;
			var figure = geometry.Figures[0];
			var arcSegment = (ArcSegment)figure.Segments[0];

			var sweep = GeometryUtil.GetSweep(startAngle, endAngle, clockwise);
			var absSweep = Math.Abs(sweep);
			var startPoint = GeometryUtil.EllipseAngleToPoint(_rectX, _rectY, _rectWidth, _rectHeight, -startAngle);
			var endPoint = GeometryUtil.EllipseAngleToPoint(_rectX, _rectY, _rectWidth, _rectHeight, -endAngle);

			figure.StartPoint = new global::Windows.Foundation.Point(startPoint.X, startPoint.Y);
			arcSegment.Point = new global::Windows.Foundation.Point(endPoint.X, endPoint.Y);
			arcSegment.Size = new global::Windows.Foundation.Size(_rectWidth / 2, _rectHeight / 2);
			arcSegment.SweepDirection = clockwise ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
			arcSegment.IsLargeArc = absSweep >= 180;

			element.Fill = CurrentState.XamlFillBrush;
			element.Opacity = CurrentState.Alpha;

			var pathX = geometry.Bounds.Left;
			var pathY = geometry.Bounds.Top;
			element.RenderTransform = CurrentState.GetXamlTransform(pathX, pathY);
		}

		public override SizeF GetStringSize(string value, IFont font, float fontSize)
			=> GetStringSize(value, font, fontSize, HorizontalAlignment.Left, VerticalAlignment.Top);
		
		public override SizeF GetStringSize(string aString, IFont font, float aFontSize, HorizontalAlignment aHorizontalAlignment, VerticalAlignment aVerticalAlignment)
		{
			var tb = new TextBlock
			{
				HorizontalTextAlignment = aHorizontalAlignment switch
				{
					HorizontalAlignment.Left => TextAlignment.Left,
					HorizontalAlignment.Center => TextAlignment.Center,
					HorizontalAlignment.Right => TextAlignment.Right,
					HorizontalAlignment.Justified => TextAlignment.Justify,
					_ => TextAlignment.Left,
				},
				VerticalAlignment = aVerticalAlignment switch
				{
					VerticalAlignment.Top => Windows.UI.Xaml.VerticalAlignment.Top,
					VerticalAlignment.Center => Windows.UI.Xaml.VerticalAlignment.Center,
					VerticalAlignment.Bottom => Windows.UI.Xaml.VerticalAlignment.Bottom,
					_ => Windows.UI.Xaml.VerticalAlignment.Top,
				},
				Text = aString,
				FontSize = (double)aFontSize,
				FontWeight = new Windows.UI.Text.FontWeight { Weight = (ushort)font.Weight },
				FontFamily = string.IsNullOrEmpty(font?.Name) ? FontFamily.XamlAutoFontFamily : new FontFamily(font.Name),
			};
			tb.Measure(new Windows.Foundation.Size(Double.PositiveInfinity, Double.PositiveInfinity));

			var s = tb.DesiredSize;

			return new SizeF((float)s.Width, (float)s.Height);
		}
	}
}
