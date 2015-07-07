﻿using Microsoft.Xna.Framework;
using Penumbra.Mathematics;
using Penumbra.Mathematics.Triangulation;
//using Polygon = System.Collections.Generic.List<Microsoft.Xna.Framework.Vector2>;
using Indices = System.Collections.Generic.List<int>;

namespace Penumbra
{
    internal class HullPart
    {
        private readonly Hull _hull;

        private bool _transformedNormalsDirty = true;

        private ApexNormals[] _transformedNormals;

        private bool _transformedHullVerticesDirty = true;
        private Polygon _transformedHullVertices;

        private bool _radiusDirty = true;
        private bool _centroidDirty = true;

        private float _radius;
        private Vector2 _centroid;

        // clockwise winding order
        public HullPart(Hull hull, Polygon polygon)
        {
            _hull = hull;
            CalculatePointsAndIndices(polygon);

            Component.SetDirty += (s, e) => { _transformedNormalsDirty = true; };
            OriginalNormals = new ApexNormals[Points.Count];

            for (int i = 0; i < Points.Count; i++)
            {
                Vector2 currentPos = Points[i];
                Vector2 prevPos = Points.PreviousElement(i);
                Vector2 nextPos = Points.NextElement(i);

                Vector2 n1 = VectorUtil.Rotate90CW(currentPos - prevPos);
                Vector2 n2 = VectorUtil.Rotate90CW(nextPos - currentPos);

                //// Ref: http://stackoverflow.com/a/25646126/1466456
                //Vector2 currentToPrev = prevPos - currentPos;
                //Vector2 currentToNext = nextPos - currentPos;                
                //float angle = Calc.Atan2(currentToNext.Y, currentToNext.X) - Calc.Atan2(currentToPrev.Y, currentToPrev.X);
                //bool isConvex = angle < MathUtil.Pi;

                OriginalNormals[i] = new ApexNormals(n1, n2);
            }
        }

        public ApexNormals[] OriginalNormals { get; }

        public ApexNormals[] TransformedNormals
        {
            get
            {
                if (_transformedNormalsDirty)
                {
                    if (_transformedNormals == null)
                        _transformedNormals = new ApexNormals[OriginalNormals.Length];

                    Matrix normalMatrix = Matrix.Identity;

                    float cos = Calc.Cos(Component.Rotation);
                    float sin = Calc.Sin(Component.Rotation);

                    // normalMatrix = scaleInv * rotation;
                    normalMatrix.M11 = (1f / Component.Scale.X) * cos;
                    normalMatrix.M12 = (1f / Component.Scale.X) * sin;
                    normalMatrix.M21 = (1f / Component.Scale.Y) * -sin;
                    normalMatrix.M22 = (1f / Component.Scale.Y) * cos;

                    for (var i = 0; i < OriginalNormals.Length; i++)
                    {
                        _transformedNormals[i] = ApexNormals.Transform(OriginalNormals[i], ref normalMatrix);
                    }

                    _transformedNormalsDirty = false;
                }
                return _transformedNormals;
            }
        }

        public Hull Component => _hull;

        public Polygon Points { get; private set; }
        public Indices Indices { get; private set; }
        public bool Enabled { get; set; }

        public Polygon TransformedHullVertices
        {
            get
            {
                if (_transformedHullVerticesDirty)
                {
                    if (_transformedHullVertices == null)
                        _transformedHullVertices = new Polygon(WindingOrder.Clockwise, Points.Count);
                    else
                        _transformedHullVertices.Clear();

                    Matrix transform = _hull.WorldTransform;
                    for (int i = 0; i < Points.Count; i++)
                    {
                        Vector2 originalPos = Points[i];
                        Vector2 transformedPos;
                        Vector2.Transform(ref originalPos, ref transform, out transformedPos);
                        //_transformedHullVertices[i] = transformedPos;
                        _transformedHullVertices.Add(transformedPos);
                    }
                    _transformedHullVerticesDirty = false;
                }
                return _transformedHullVertices;
            }
        }

        public Vector2 Centroid
        {
            get
            {
                if (_centroidDirty)
                {
                    _centroid = TransformedHullVertices.GetCentroid();
                    _centroidDirty = false;
                }
                return _centroid;
            }
        }

        public float Radius
        {
            get
            {
                if (_radiusDirty)
                {
                    _radius = TransformedHullVertices.GetRadius();
                    _radiusDirty = false;
                }
                return _radius;
            }
        }

        public void SetRadiusDirty()
        {
            _radiusDirty = true;
        }

        public void SetDirty()
        {
            _transformedHullVerticesDirty = true;
            _centroidDirty = true;
        }

        private void CalculatePointsAndIndices(Polygon points)
        {
            Indices = new Indices();
            Points = points;
            Points.EnsureWindingOrder(WindingOrder.CounterClockwise);
            Points.GetIndices(WindingOrder.Clockwise, Indices);                 
        }        
    }
}