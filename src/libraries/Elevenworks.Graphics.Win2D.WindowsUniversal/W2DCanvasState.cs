﻿using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.Graphics.Canvas.Geometry;
using Xamarin.Graphics;


namespace Elevenworks.Graphics.Win2D
{
    public class W2DCanvasState : CanvasState
    {
        private static FontWeight _nativeFontWeight = new FontWeight();

        private readonly W2DCanvas _owner;
        private readonly W2DCanvasState _parentState;

        private float _alpha = 1;
        private float[] _dashes;

        private ICanvasBrush _fillBrush;
        private bool _fillBrushValid;
        private CanvasSolidColorBrush _fontBrush;
        private bool _fontBrushValid;
        private float _fontSize;
        private Vector2 _gradientPoint1;
        private Vector2 _gradientPoint2;
        //private GradientStopCollection _gradientStopCollection;
        private CanvasGeometry _layerBounds;
        private CanvasGeometry _layerClipBounds;
        private CanvasGeometry _layerMask;
        private CanvasActiveLayer _layer;
        private bool _needsStrokeStyle;
        private float _scale;

        private Color _shadowColor;
        private bool _shadowColorValid;
        private EWColor _sourceFillColor;
        private EWPaint _sourceFillpaint;

        private EWColor _sourceFontColor;
        private EWColor _sourceShadowColor;
        private EWColor _sourceStrokeColor;
        private CanvasSolidColorBrush _strokeBrush;
        private bool _strokeBrushValid;
        private CanvasStrokeStyle _strokeStyle;
        private float _miterLimit;
        private CanvasCapStyle _lineCap;
        private CanvasLineJoin _lineJoin;
        //private CanvasStrokeStyleProperties strokeStyleProperties;

        private int _layerCount = 0;
        private readonly float _dpi = 96;

        public String FontName { get; set; }
        public int FontWeight { get; set; }
        public FontStyle FontStyle { get; set; }

        public FontWeight NativeFontWeight
        {
            get
            {
                _nativeFontWeight.Weight = (ushort)FontWeight;
                return _nativeFontWeight;
            }

            set => FontWeight = value.Weight;
        }

        public float BlurRadius { get; private set; }
        public bool IsShadowed { get; private set; }
        public bool IsBlurred { get; private set; }
        public Vector2 ShadowOffset { get; private set; }
        public float ShadowBlur { get; set; }
        public Matrix3x2 Matrix { get; private set; }

        public W2DCanvasState(W2DCanvas owner)
        {
            _owner = owner;
            _parentState = null;
            SetToDefaults();
        }

        public W2DCanvasState(W2DCanvasState prototype) : base(prototype)
        {
            _dpi = prototype.Dpi;
            _owner = prototype._owner;
            _parentState = prototype;

            _strokeBrush = prototype._strokeBrush;
            _fillBrush = prototype._fillBrush;
            _fontBrush = prototype._fontBrush;
            _shadowColor = prototype._shadowColor;

            _sourceStrokeColor = prototype._sourceStrokeColor;
            _sourceFillpaint = prototype._sourceFillpaint;
            _sourceFillColor = prototype._sourceFillColor;
            _sourceFontColor = prototype._sourceFontColor;
            _sourceShadowColor = prototype._sourceShadowColor;

            _strokeBrushValid = prototype._strokeBrushValid;
            _fillBrushValid = prototype._fillBrushValid;
            _fontBrushValid = prototype._fontBrushValid;
            _shadowColorValid = prototype._shadowColorValid;

            _dashes = prototype._dashes;
            _strokeStyle = prototype._strokeStyle;
            _lineJoin = prototype._lineJoin;
            _lineCap = prototype._lineCap;
            _miterLimit = prototype._miterLimit;
            _needsStrokeStyle = prototype._needsStrokeStyle;

            IsShadowed = prototype.IsShadowed;
            ShadowOffset = prototype.ShadowOffset;
            ShadowBlur = prototype.ShadowBlur;

            Matrix = new Matrix3x2(prototype.Matrix.M11, prototype.Matrix.M12, prototype.Matrix.M21, prototype.Matrix.M22, prototype.Matrix.M31, prototype.Matrix.M32);

            FontName = prototype.FontName;
            FontSize = prototype.FontSize;
            FontWeight = prototype.FontWeight;
            FontStyle = prototype.FontStyle;

            _alpha = prototype._alpha;
            _scale = prototype._scale;

            IsBlurred = prototype.IsBlurred;
            BlurRadius = prototype.BlurRadius;
        }

        public void SetToDefaults()
        {
            _sourceStrokeColor = StandardColors.Black;
            _strokeBrushValid = false;
            _needsStrokeStyle = false;
            _strokeStyle = null;

            _dashes = null;
            _miterLimit = EWCanvas.DefaultMiterLimit;
            _lineCap = CanvasCapStyle.Flat;
            _lineJoin = CanvasLineJoin.Miter;
            
            _sourceFillpaint = StandardColors.White.AsPaint();
            _fillBrushValid = false;

            Matrix = Matrix3x2.Identity;

            IsShadowed = false;
            _sourceShadowColor = EWCanvas.DefaultShadowColor;

            FontName = "Arial";
            FontSize = 12;
            FontWeight = 200;
            FontStyle = FontStyle.Normal;
            _sourceFontColor = StandardColors.Black;
            _fontBrushValid = false;

            _alpha = 1;
            _scale = 1;

            IsBlurred = false;
            BlurRadius = 0;
        }

        public float FontSize
        {
            get => _fontSize;
            set => _fontSize = value;
        }

        public float Dpi => _dpi;

        public float Alpha
        {
            get => _alpha;

            set
            {
                if (_alpha != value)
                {
                    _alpha = value;
                    InvalidateBrushes();
                }
            }
        }

        public EWColor StrokeColor
        {
            set
            {
                var finalValue = value ?? StandardColors.Black;

                if (!finalValue.Equals(_sourceStrokeColor))
                {
                    _sourceStrokeColor = finalValue;
                    _strokeBrushValid = false;
                }
            }
        }

        public float MiterLimit
        {
            set
            {
                _miterLimit = value;
                InvalidateStrokeStyle();
                _needsStrokeStyle = true;
            }
        }

        public EWLineCap StrokeLineCap
        {
            set
            {
                switch (value)
                {
                    case EWLineCap.BUTT:
                        _lineCap = CanvasCapStyle.Flat;
                        break;
                    case EWLineCap.ROUND:
                        _lineCap = CanvasCapStyle.Round;
                        break;
                    default:
                        _lineCap = CanvasCapStyle.Square;
                        break;
                }

                InvalidateStrokeStyle();
                _needsStrokeStyle = true;
            }
        }

        public EWLineJoin StrokeLineJoin
        {
            set
            {
                switch (value)
                {
                    case EWLineJoin.BEVEL:
                        _lineJoin = CanvasLineJoin.Bevel;
                        break;
                    case EWLineJoin.ROUND:
                        _lineJoin = CanvasLineJoin.Round;
                        break;
                    default:
                        _lineJoin = CanvasLineJoin.Miter;
                        break;
                }

                InvalidateStrokeStyle();
                _needsStrokeStyle = true;
            }
        }

        public void SetStrokeDashPattern(float[] pattern, float strokeSize)
        {
            if (pattern == null || pattern.Length == 0)
            {
                if (_needsStrokeStyle == false) return;
                _dashes = null;
            }
            else
            {
                _dashes = pattern;
            }

            InvalidateStrokeStyle();
            _needsStrokeStyle = true;
        }

        public EWColor FillColor
        {
            set
            {
                ReleaseFillBrush();
                _fillBrushValid = false;
                _sourceFillColor = value;
                _sourceFillpaint = null;
            }
        }

        public void SetLinearGradient(EWPaint aPaint, Vector2 aPoint1, Vector2 aPoint2)
        {
            ReleaseFillBrush();
            _fillBrushValid = false;
            _sourceFillColor = null;
            _sourceFillpaint = aPaint;
            _gradientPoint1 = aPoint1;
            _gradientPoint2 = aPoint2;
        }

        public void SetRadialGradient(EWPaint aPaint, Vector2 aPoint1, Vector2 aPoint2)
        {
            ReleaseFillBrush();
            _fillBrushValid = false;
            _sourceFillColor = null;
            _sourceFillpaint = aPaint;
            _gradientPoint1 = aPoint1;
            _gradientPoint2 = aPoint2;
        }

        public void SetBitmapBrush(CanvasImageBrush bitmapBrush)
        {
            _fillBrush = bitmapBrush;
            _fillBrushValid = true;
        }

        public ICanvasBrush NativeFillBrush
        {
            get
            {
                if (_fillBrush == null || !_fillBrushValid)
                {
                    if (_sourceFillColor != null)
                    {
                        _fillBrush = new CanvasSolidColorBrush(_owner.Session, _sourceFillColor.AsColor(_alpha));
                        _fillBrushValid = true;
                    }
                    else if (_sourceFillpaint != null)
                    {
                        if (_sourceFillpaint.PaintType == EWPaintType.LINEAR_GRADIENT)
                        {
                            var gradientStops = new CanvasGradientStop[_sourceFillpaint.Stops.Length];
                            for (int i = 0; i < _sourceFillpaint.Stops.Length; i++)
                            {
                                gradientStops[i] = new CanvasGradientStop()
                                {
                                    Position = _sourceFillpaint.Stops[i].Offset,
                                    Color = _sourceFillpaint.Stops[i].Color.AsColor(StandardColors.White, _alpha)
                                };
                            }

                            _fillBrush = new CanvasLinearGradientBrush(_owner.Session, gradientStops);
                            ((CanvasLinearGradientBrush)_fillBrush).StartPoint = _gradientPoint1;
                            ((CanvasLinearGradientBrush)_fillBrush).EndPoint = _gradientPoint2;
                        }
                        else
                        {
                            float radius = Geometry.GetDistance(_gradientPoint1.X, _gradientPoint1.Y, _gradientPoint2.X, _gradientPoint2.Y);

                            var gradientStops = new CanvasGradientStop[_sourceFillpaint.Stops.Length];
                            for (int i = 0; i < _sourceFillpaint.Stops.Length; i++)
                            {
                                gradientStops[i] = new CanvasGradientStop
                                {
                                    Position = _sourceFillpaint.Stops[i].Offset,
                                    Color = _sourceFillpaint.Stops[i].Color.AsColor(StandardColors.White, _alpha)
                                };
                            }
                            _fillBrush = new CanvasRadialGradientBrush(_owner.Session, gradientStops);
                            ((CanvasRadialGradientBrush)_fillBrush).Center = _gradientPoint1;
                            ((CanvasRadialGradientBrush)_fillBrush).RadiusX = radius;
                            ((CanvasRadialGradientBrush)_fillBrush).RadiusY = radius;
                        }
                        _fillBrushValid = true;
                    }
                    else
                    {
                        _fillBrush = new CanvasSolidColorBrush(_owner.Session, Colors.White);
                        _fillBrushValid = true;
                    }
                }

                return _fillBrush;
            }
        }

        public EWColor FontColor
        {
            set
            {
                var finalValue = value ?? StandardColors.Black;

                if (!finalValue.Equals(_sourceFontColor))
                {
                    _sourceFontColor = finalValue;
                    _fontBrushValid = false;
                }
            }
        }

        public void SetShadow(EWSize aOffset, float aBlur, EWColor aColor, float aZoom)
        {
            if (aOffset != null)
            {
                IsShadowed = true;
                ShadowOffset = new Vector2(aOffset.Width, aOffset.Height);
                ShadowBlur = aBlur;
                _sourceShadowColor = aColor;
                _shadowColorValid = false;
            }
            else
            {
                IsShadowed = false;
            }
        }

        public Color ShadowColor
        {
            get
            {
                if (!_shadowColorValid)
                {
                    _shadowColor = _sourceShadowColor?.AsColor(_alpha) ?? EWCanvas.DefaultShadowColor.AsColor(_alpha);
                }

                return _shadowColor;
            }
        }

        public void SetBlur(float aRadius)
        {
            if (aRadius > 0)
            {
                IsBlurred = true;
                BlurRadius = aRadius;
            }
            else
            {
                IsBlurred = false;
                BlurRadius = 0;
            }
        }

        public float ActualScale => _scale;

        public float ActualShadowBlur => ShadowBlur * Math.Abs(_scale);

        public Matrix3x2 AppendTranslate(float tx, float ty)
        {
            //Matrix = Matrix * Matrix3x2.Translation(tx, ty);
            Matrix = Matrix.Translate(tx, ty);
            return Matrix;
        }

        public Matrix3x2 AppendConcatenateTransform(EWAffineTransform transform)
        {
            var values = new float[6];
            transform.GetMatrix(values);
            Matrix = Matrix3x2.Multiply(Matrix, new Matrix3x2(values[0],values[1],values[2],values[3],values[4],values[5]));
            return Matrix;
        }

        public Matrix3x2 AppendScale(float tx, float ty)
        {
            _scale *= tx;
            Matrix = Matrix.Scale(tx, ty);
            return Matrix;
        }

        public Matrix3x2 AppendRotate(float aAngle)
        {
            float radians = Geometry.DegreesToRadians(aAngle);
            Matrix = Matrix.Rotate(radians);
            return Matrix;
        }

        public Matrix3x2 AppendRotate(float aAngle, float x, float y)
        {
            float radians = Geometry.DegreesToRadians(aAngle);
            Matrix = Matrix.Translate(x, y);
            Matrix = Matrix.Rotate(radians);
            Matrix = Matrix.Translate(-x, -y);
            return Matrix;
        }

        public void ClipPath(EWPath path, EWWindingMode windingMode)
        {
            if (_layerMask != null)
                throw new Exception("Only one clip operation currently supported.");

            var layerRect = new Rect(0, 0, _owner.CanvasSize.Width, _owner.CanvasSize.Height);
            _layerBounds = CanvasGeometry.CreateRectangle(_owner.Session, layerRect);
            var clipGeometry = path.AsPath(_owner.Session, windingMode == EWWindingMode.NonZero ? CanvasFilledRegionDetermination.Winding : CanvasFilledRegionDetermination.Alternate);

            _layerMask = _layerBounds.CombineWith(clipGeometry, Matrix3x2.Identity, CanvasGeometryCombine.Intersect);

            _layer = _owner.Session.CreateLayer(1, _layerMask);
            _layerCount++;
        }

        public void ClipRectangle(float x, float y, float width, float height)
        {
            var path = new EWPath();
            path.AppendRectangle(x, y, width, height);
            ClipPath(path, EWWindingMode.NonZero);
        }

        public void SubtractFromClip(float x, float y, float width, float height)
        {
            if (_layerMask != null)
                throw new Exception("Only one subtraction currently supported.");

            var layerRect = new Rect(0, 0, _owner.CanvasSize.Width, _owner.CanvasSize.Height);
            _layerBounds = CanvasGeometry.CreateRectangle(_owner.Session, layerRect);

            var boundsToSubtract = new Rect(x, y, width, height);
            _layerClipBounds = CanvasGeometry.CreateRectangle(_owner.Session, boundsToSubtract);

            _layerMask = _layerBounds.CombineWith(_layerClipBounds, Matrix3x2.Identity, CanvasGeometryCombine.Exclude);

            _layer = _owner.Session.CreateLayer(1, _layerMask);
            _layerCount++;
        }
        public void SaveRenderTargetState()
        {
            
        }

        public void RestoreRenderTargetState()
        {
            _owner.Session.Transform = Matrix;
            //needsStrokeStyle = true;
        }

        public ICanvasBrush NativeFontBrush
        {
            get
            {
                if (_fontBrush == null || (!_fontBrushValid && _parentState != null && _fontBrush == _parentState._fontBrush))
                    _fontBrush = new CanvasSolidColorBrush(_owner.Session, _sourceFontColor.AsColor(StandardColors.Black, _alpha));
                else if (!_fontBrushValid)
                    _fontBrush.Color = _sourceFontColor.AsColor(StandardColors.Black, _alpha);

                return _fontBrush;
            }
        }

        public ICanvasBrush NativeStrokeBrush
        {
            get
            {
                if (_strokeBrush == null || (!_strokeBrushValid && _parentState != null && _strokeBrush == _parentState._strokeBrush))
                {
                    _strokeBrush = new CanvasSolidColorBrush(_owner.Session, _sourceStrokeColor.AsColor(StandardColors.Black, _alpha));
                    _strokeBrushValid = true;
                }
                else if (!_strokeBrushValid)
                {
                    _strokeBrush.Color = _sourceStrokeColor.AsColor(StandardColors.Black, _alpha);
                    _strokeBrushValid = true;
                }

                return _strokeBrush;
            }
        }

        public CanvasStrokeStyle NativeStrokeStyle
        {
            get
            {
                if (_needsStrokeStyle)
                {
                    if (_strokeStyle == null)
                    {
                        _strokeStyle = new CanvasStrokeStyle();
                    }

                    if (_dashes != null)
                    {
                        _strokeStyle.CustomDashStyle = _dashes;
                        _strokeStyle.DashCap = _lineCap;
                    }
                    else
                    {
                        _strokeStyle.CustomDashStyle = null;
                    }

                    _strokeStyle.MiterLimit = _miterLimit;
                    _strokeStyle.StartCap = _lineCap;
                    _strokeStyle.EndCap = _lineCap;
                    _strokeStyle.LineJoin = _lineJoin;


                    return _strokeStyle;
                }

                return _strokeStyle;
            }
        }

        private void InvalidateBrushes()
        {
            _strokeBrushValid = false;
            _fillBrushValid = false;
            _shadowColorValid = false;
            _fontBrushValid = false;
        }

        private void ReleaseFillBrush()
        {
            if (_fillBrush != null)
            {
                if (_parentState == null || _fillBrush != _parentState._fillBrush)
                {
                    _fillBrush.Dispose();
                }
                _fillBrush = null;
            }
        }

        private void InvalidateStrokeStyle()
        {
            if (_strokeStyle != null)
            {
                if (_parentState == null || _strokeStyle != _parentState._strokeStyle)
                {
                    _strokeStyle.Dispose();
                }
                _strokeStyle = null;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_layer != null)
            {
                _layer.Dispose();
                _layer = null;
            }

            if (_layerMask != null)
            {
                _layerMask.Dispose();
                _layerMask = null;
            }

            if (_layerBounds != null)
            {
                _layerBounds.Dispose();
                _layerBounds = null;
            }

            if (_layerClipBounds != null)
            {
                _layerClipBounds.Dispose();
                _layerClipBounds = null;
            }           
        }
    }
}