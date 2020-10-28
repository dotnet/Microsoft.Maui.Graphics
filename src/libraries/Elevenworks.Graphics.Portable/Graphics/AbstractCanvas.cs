using System;
using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public abstract class AbstractCanvas<TState> : EWCanvas, IDisposable where TState : CanvasState
    {
        private readonly Stack<object> _figureStack = new Stack<object>();
        private readonly Stack<TState> _stateStack = new Stack<TState>();
        private readonly Func<object, TState> _createNew;
        private readonly Func<TState, TState> _createCopy;

        private TState _currentState;
        private bool _limitStrokeScaling;
        private float _strokeLimit = 1;
        private bool _strokeDashPatternDirty;

        protected abstract float NativeStrokeSize { set; }
        protected abstract void NativeSetStrokeDashPattern(float[] pattern, float strokeSize);
        protected abstract void NativeDrawLine(float x1, float y1, float x2, float y2);
        protected abstract void NativeDrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed);
        protected abstract void NativeDrawRectangle(float x, float y, float width, float height);
        protected abstract void NativeDrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius);
        protected abstract void NativeDrawOval(float x, float y, float width, float height);
        protected abstract void NativeDrawPath(EWPath path, float ppu);
        protected abstract void NativeRotate(float degrees, float radians, float x, float y);
        protected abstract void NativeRotate(float degrees, float radians);
        protected abstract void NativeScale(float fx, float fy);
        protected abstract void NativeTranslate(float tx, float ty);
        protected abstract void NativeConcatenateTransform(EWAffineTransform transform);

        protected AbstractCanvas(Func<object, TState> createNew, Func<TState, TState> createCopy)
        {
            _createCopy = createCopy;
            _createNew = createNew;
            _currentState = createNew(this);
        }

        public TState CurrentState
        {
            get => _currentState;
            protected set => _currentState = value;
        }

        public virtual void Dispose()
        {
            if (_currentState != null)
            {
                _currentState.Dispose();
                _currentState = null;
            }
        }

        public override object CurrentFigure => _figureStack.Count > 0 ? _figureStack.Peek() : null;

        public override void StartFigure(object figure)
        {
            _figureStack.Push(figure);
        }

        public override void EndFigure()
        {
            _figureStack.Pop();
        }

        public override bool LimitStrokeScaling
        {
            set => _limitStrokeScaling = value;
        }

        protected bool LimitStrokeScalingEnabled => _limitStrokeScaling;

        public override float StrokeLimit
        {
            set => _strokeLimit = value;
        }

        protected float AssignedStrokeLimit => _strokeLimit;

        public override float StrokeSize
        {
            set
            {
                var size = value;

                if (_limitStrokeScaling)
                {
                    var scale = _currentState.Scale;
                    var scaledStrokeSize = scale * value;
                    if (scaledStrokeSize < _strokeLimit)
                    {
                        size = _strokeLimit / scale;
                    }
                }

                _currentState.StrokeSize = size;
                NativeStrokeSize = size;
            }
        }
        
        public override float[] StrokeDashPattern
        {
            set
            {
                if (!ReferenceEquals(value, _currentState.StrokeDashPattern))
                {
                    _currentState.StrokeDashPattern = value;
                    _strokeDashPatternDirty = true;
                }
            }
        }

        public void EnsureStrokePatternSet()
        {
            if (_strokeDashPatternDirty)
            {
                NativeSetStrokeDashPattern(_currentState.StrokeDashPattern, _currentState.StrokeSize);
                _strokeDashPatternDirty = false;
            }
        }

        public override EWStrokeLocation StrokeLocation
        {
            set => _currentState.StrokeLocation = value;
        }
        
        public override void DrawLine(float x1, float y1, float x2, float y2)
        {
            EnsureStrokePatternSet();
            NativeDrawLine(x1, y1, x2, y2);
        }

        public override void DrawArc(float x, float y, float width, float height, float startAngle, float endAngle, bool clockwise, bool closed)
        {
            EnsureStrokePatternSet();
            NativeDrawArc(x, y, width, height, startAngle, endAngle, clockwise, closed);
        }

        private void DrawBrushLine(Action action)
        {
            var dashPattern = _currentState.StrokeDashPattern;
            NativeSetStrokeDashPattern(null, 1);
            action();
            NativeSetStrokeDashPattern(dashPattern, _currentState.StrokeSize);
            _strokeDashPatternDirty = false;
        }

        public override void DrawRectangle(float x, float y, float width, float height)
        {
            EnsureStrokePatternSet();
            NativeDrawRectangle(x, y, width, height);
        }

        public override void DrawRoundedRectangle(float x, float y, float width, float height, float cornerRadius)
        {
            var halfHeight = Math.Abs(height / 2);
            if (cornerRadius > halfHeight)
                cornerRadius = halfHeight;

            var halfWidth = Math.Abs(width / 2);
            if (cornerRadius > halfWidth)
                cornerRadius = halfWidth;
            
            EnsureStrokePatternSet();
            NativeDrawRoundedRectangle(x, y, width, height, cornerRadius);
        }

        public override void DrawOval(float x, float y, float width, float height)
        {
            EnsureStrokePatternSet();
            NativeDrawOval(x, y, width, height);
        }

        public override void DrawPath(EWPath path, float ppu)
        {
            EnsureStrokePatternSet();
            NativeDrawPath(path, ppu);
        }
        
        public override void ResetState()
        {
            while (_stateStack.Count > 0)
            {
                if (_currentState != null)
                {
                    _currentState.Dispose();
                    _currentState = null;
                }

                _currentState = _stateStack.Pop();
                StateRestored(_currentState);
            }

            if (_currentState != null)
            {
                _currentState.Dispose();
                _currentState = null;
            }

            _currentState = _createNew(this);
            _figureStack.Clear();
        }

        public override bool RestoreState()
        {
            _strokeDashPatternDirty = true;

            if (_currentState != null)
            {
                _currentState.Dispose();
                _currentState = null;
            }

            if (_stateStack.Count > 0)
            {
                _currentState = _stateStack.Pop();
                StateRestored(_currentState);
                return true;
            }

            _currentState = _createNew(this);
            return false;
        }

        protected virtual void StateRestored(TState state)
        {
            // Do nothing
        }

        public override void SaveState()
        {
            var previousState = _currentState;
            _stateStack.Push(previousState);
            _currentState = _createCopy(previousState);
            _strokeDashPatternDirty = true;
        }

        public override void Rotate(float degrees, float x, float y)
        {
            var radians = Geometry.DegreesToRadians(degrees);

            var transform = _currentState.Transform;
            transform.Translate(x, y);
            transform.Rotate(radians);
            transform.Translate(-x, -y);

            NativeRotate(degrees, radians, x, y);
        }

        public override void Rotate(float degrees)
        {
            var radians = Geometry.DegreesToRadians(degrees);
            _currentState.Transform.Rotate(radians);

            NativeRotate(degrees, radians);
        }

        public override void Scale(float fx, float fy)
        {
            _currentState.Scale *= fx;
            _currentState.Transform.Scale(fx, fy);

            NativeScale(fx, fy);
        }

        public override void Translate(float tx, float ty)
        {
            _currentState.Transform.Translate(tx, ty);
            NativeTranslate(tx, ty);
        }

        public override void ConcatenateTransform(EWAffineTransform transform)
        {
            _currentState.Scale *= transform.GetScaleX();
            _currentState.Transform.Concatenate(transform);
            NativeConcatenateTransform(transform);
        }
    }
}