﻿using System;
using Microsoft.Xna.Framework;
using Penumbra;

namespace Sandbox.Scenarios
{
    class C_Antumbra : Scenario
    {
        private const float MinLightRadius = 10;
        private const float MaxLightRadius = 160;
        private const float RadiusSpeed = 2f;

        private Light _light;
        private bool _isRadiusIncreasing;
        private float _progress;

        public override string Name { get; } = "Antumbra";

        public override void Activate(PenumbraComponent penumbra)
        {
            _isRadiusIncreasing = true;
            _progress = 0;

            _light = new Light
            {
                Position = new Vector2(-100, 50),
                //Position = new Vector2(-100, 0),
                Color = Color.White,
                Range = 500,
                Radius = MinLightRadius,
                ShadowType = ShadowType.Illuminated
            };
            penumbra.Lights.Add(_light);

            var hullVertices = new[]
            {
                new Vector2(-0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, -0.5f), new Vector2(-0.5f, -0.5f)
            };
            penumbra.Hulls.Add(new Hull(hullVertices)
            {
                Position = new Vector2(100, 0),
                Scale = new Vector2(50f)
            });
            //penumbra.Hulls.Add(new Hull(hullVertices)
            //{
            //    Position = new Vector2(250, 0),
            //    Scale = new Vector2(50f)
            //});
        }

        public override void Update(float deltaSeconds)
        {
            _progress = Math.Min(_progress + deltaSeconds / RadiusSpeed, 1f);

            _light.Radius = _isRadiusIncreasing 
                ? MathHelper.Lerp(MinLightRadius, MaxLightRadius, _progress) 
                : MathHelper.Lerp(MaxLightRadius, MinLightRadius, _progress);

            if (_progress >= 1f)
            {
                _progress = 0;
                _isRadiusIncreasing = !_isRadiusIncreasing;
            }
        }
    }
}