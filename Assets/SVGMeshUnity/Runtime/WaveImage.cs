﻿using SVGMeshUnity.Internals;
using SVGMeshUnity.Internals.Cdt2d;
using UnityEngine;
using UnityEngine.UI;

namespace SVGMeshUnity
{
    public class WaveImage : Graphic
    {
        // https://github.com/mattdesl/adaptive-bezier-curve

        [SerializeField, Range(0f, 1f)] private float _waveHorRadius, _waveVertRadius, _sideWidth, _waveCenterY;
        private SVGData _data;

        private static readonly WorkBufferPool _workBufferPool = new WorkBufferPool();
        private readonly MeshData _meshData = new MeshData();
        private BezierToVertex _bezierToVertex;
        private Triangulation _triangulation;

        public float WaveCenterY
        {
            get { return _waveCenterY; }
            set
            {
                _waveCenterY = value;
                SetAllDirty();
            }
        }

        public float WaveHorRadius
        {
            get { return _waveHorRadius; }
            set
            {
                _waveHorRadius = value;
                SetAllDirty();
            }
        }

        public float WaveVertRadius
        {
            get { return _waveVertRadius; }
            set
            {
                _waveVertRadius = value;
                SetAllDirty();
            }
        }

        public float SideWidth
        {
            get { return _sideWidth; }
            set
            {
                _sideWidth = value;
                SetAllDirty();
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if (_bezierToVertex == null)
                _bezierToVertex = new BezierToVertex
                {
                    WorkBufferPool = _workBufferPool,
                    Scale = 1
                };

            if (_triangulation == null)
                _triangulation = new Triangulation
                {
                    Delaunay = false,
                    Interior = true,
                    Exterior = false,
                    Infinity = false,
                    WorkBufferPool = _workBufferPool
                };


            var selfRectTransform = (RectTransform) transform;
            var selfRect = selfRectTransform.rect;
            var offset = new Vector2(-selfRectTransform.pivot.x * selfRect.width,
                (1 - selfRectTransform.pivot.y) * selfRect.height);
            var svgData = BuildWave(selfRect,
                _waveCenterY * selfRect.height,
                _waveHorRadius * selfRect.width,
                _waveVertRadius * selfRect.height,
                _sideWidth * selfRect.width);
            _meshData.Clear();
            _bezierToVertex.GetContours(svgData, _meshData);
            _triangulation.BuildTriangles(_meshData);

            _meshData.MakeUnityFriendly();
            _meshData.Upload(vh, offset, color);
        }

        private static SVGData BuildWave(Rect bounds, float waveCenterY, float waveHorRadius, float waveVertRadius,
            float sideWidth)
        {
            var rect = bounds;
            var path = new SVGData();
            var maskWidth = rect.width - sideWidth;
            var curveStartY = waveCenterY + waveVertRadius;

            path.Move(new Vector2(maskWidth - sideWidth, 0f - waveVertRadius * 2));
            path.Line(new Vector2(0f - waveHorRadius - sideWidth, 0f - waveVertRadius * 2));
            path.Line(new Vector2(0f - waveHorRadius - sideWidth, rect.height + waveVertRadius * 2));
            path.Line(new Vector2(maskWidth, rect.height + waveVertRadius * 2));

            path.Line(new Vector2(maskWidth, curveStartY));

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.1561501458f, curveStartY - waveVertRadius * 0.3322374268f),
                new Vector2(maskWidth, curveStartY - waveVertRadius * 0.1346194756f),
                new Vector2(maskWidth - waveHorRadius * 0.05341339583f, curveStartY - waveVertRadius * 0.2412779634f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.5012484792f, curveStartY - waveVertRadius * 0.5350576951f),
                new Vector2(maskWidth - waveHorRadius * 0.2361659167f, curveStartY - waveVertRadius * 0.4030805244f),
                new Vector2(maskWidth - waveHorRadius * 0.3305285625f, curveStartY - waveVertRadius * 0.4561193293f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.574934875f, curveStartY - waveVertRadius * 0.5689655122f),
                new Vector2(maskWidth - waveHorRadius * 0.515878125f, curveStartY - waveVertRadius * 0.5418222317f),
                new Vector2(maskWidth - waveHorRadius * 0.5664134792f, curveStartY - waveVertRadius * 0.5650349878f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.8774032292f, curveStartY - waveVertRadius * 0.7399037439f),
                new Vector2(maskWidth - waveHorRadius * 0.7283715208f, curveStartY - waveVertRadius * 0.6397387195f),
                new Vector2(maskWidth - waveHorRadius * 0.8086618958f, curveStartY - waveVertRadius * 0.6833456585f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius, curveStartY - waveVertRadius),
                new Vector2(maskWidth - waveHorRadius * 0.9653464583f, curveStartY - waveVertRadius * 0.8122605122f),
                new Vector2(maskWidth - waveHorRadius, curveStartY - waveVertRadius * 0.8936183659f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.8608411667f, curveStartY - waveVertRadius * 1.270484439f),
                new Vector2(maskWidth - waveHorRadius, curveStartY - waveVertRadius * 1.100142878f),
                new Vector2(maskWidth - waveHorRadius * 0.9595746667f, curveStartY - waveVertRadius * 1.1887991951f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.5291125625f, curveStartY - waveVertRadius * 1.4665102805f),
                new Vector2(maskWidth - waveHorRadius * 0.7852123333f, curveStartY - waveVertRadius * 1.3330544756f),
                new Vector2(maskWidth - waveHorRadius * 0.703382125f, curveStartY - waveVertRadius * 1.3795848049f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.5015305417f, curveStartY - waveVertRadius * 1.4802616098f),
                new Vector2(maskWidth - waveHorRadius * 0.5241858333f, curveStartY - waveVertRadius * 1.4689677195f),
                new Vector2(maskWidth - waveHorRadius * 0.505739125f, curveStartY - waveVertRadius * 1.4781625854f)
            );

            path.CurveOther(
                new Vector2(maskWidth - waveHorRadius * 0.1541165417f, curveStartY - waveVertRadius * 1.687403f),
                new Vector2(maskWidth - waveHorRadius * 0.3187486042f, curveStartY - waveVertRadius * 1.5714239024f),
                new Vector2(maskWidth - waveHorRadius * 0.2332057083f, curveStartY - waveVertRadius * 1.6204116463f)
            );

            path.CurveOther(
                new Vector2(maskWidth, curveStartY - waveVertRadius * 2f),
                new Vector2(maskWidth - waveHorRadius * 0.0509933125f, curveStartY - waveVertRadius * 1.774752061f),
                new Vector2(maskWidth, curveStartY - waveVertRadius * 1.8709256829f)
            );

            path.Line(new Vector2(maskWidth, 0f - waveVertRadius * 2));
            return path;
        }
    }
}