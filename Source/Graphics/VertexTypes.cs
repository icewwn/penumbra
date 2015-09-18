﻿using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Penumbra.Graphics
{
    internal static class VertexPosition2
    {
        public static readonly int Size = 8;

        public static readonly VertexDeclaration Layout = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0));

        // Missing implementation. Use Vector2 instead!
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct VertexPosition2Texture
    {
        public static readonly int Size = 16;

        public static readonly VertexDeclaration Layout = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0));

        public Vector2 Position;
        public Vector2 TexCoord;

        public VertexPosition2Texture(Vector2 position, Vector2 texCoord)
        {
            Position = position;
            TexCoord = texCoord;            
        }             

        public override string ToString()
        {
            return $"{nameof(Position)}:{Position} {nameof(TexCoord)}:{TexCoord}";
        }
    }
    
    [StructLayout(LayoutKind.Sequential)]
    internal struct VertexShadow
    {
        public const int Size = 28;

        public static readonly VertexDeclaration Layout = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(8, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
            new VertexElement(16, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(24, VertexElementFormat.Single, VertexElementUsage.Normal, 0));
        
        public Vector2 SegmentA;
        public Vector2 SegmentB;
        public Vector2 OccluderCoord;
        public float Radius;     

        public VertexShadow(Vector2 segA, Vector2 segB, Vector2 occluderCoord, float radius)
        {
            SegmentA = segA;
            SegmentB = segB;
            OccluderCoord = occluderCoord;            
            Radius = radius;
        }

        public override string ToString()
        {
            return $"{nameof(SegmentA)}:{SegmentA} {nameof(SegmentB)}:{SegmentB} {nameof(OccluderCoord)}:{OccluderCoord} {nameof(Radius)}:{Radius}";
        }
    }
}
