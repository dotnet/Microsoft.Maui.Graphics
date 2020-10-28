using System.Collections.Generic;

namespace Elevenworks.Graphics
{
    public class EWAffineTransformStack
    {
        private readonly List<EWAffineTransform> _transforms = new List<EWAffineTransform>();
        private int _index = -1;

        public int Count => _transforms.Count;

        public void AppendTranslate(float dx, float dy)
        {
            Append(EWAffineTransform.GetTranslateInstance(dx, dy));
        }

        public void AppendScale(float sx, float sy)
        {
            Append(EWAffineTransform.GetScaleInstance(sx, sy));
        }

        public void AppendRotateInDegrees(float deg)
        {
            Append(EWAffineTransform.GetRotateInstance(Geometry.DegreesToRadians(deg)));
        }

        public void Append(EWAffineTransform aTransform)
        {
            if (aTransform == null)
            {
                return;
            }

            if (_index == -1)
            {
                _transforms.Add(aTransform);
            }
            else
            {
                _transforms.Insert(_index++, aTransform);
            }
        }

        public void Pop()
        {
            Pop(1);
        }

        public void PopFromTop()
        {
            PopFromTop(1);
        }

        public void Pop(int aCount)
        {
            while (aCount > 0)
            {
                _transforms.RemoveAt(_transforms.Count - 1);
                aCount--;
            }
        }

        public void PopFromTop(int aCount)
        {
            while (aCount > 0)
            {
                _transforms.RemoveAt(0);
                aCount--;
            }
        }

        public void StartGroup()
        {
            _index = 0;
        }

        public void EndGroup()
        {
            _index = -1;
        }

        public EWPoint Transform(EWImmutablePoint aPoint)
        {
            EWPoint vPoint = new EWPoint(aPoint);
            foreach (var t in _transforms)
            {
                vPoint = t.Transform(vPoint);
            }

            return vPoint;
        }

        public EWAffineTransformStack CreateInverse()
        {
            var vInverse = new EWAffineTransformStack();

            for (int i = _transforms.Count - 1; i >= 0; i--)
            {
                vInverse.Append(_transforms[i].CreateInverse());
            }

            return vInverse;
        }
    }
}