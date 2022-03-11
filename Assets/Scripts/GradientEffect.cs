using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong
{
    [AddComponentMenu("UI/Effects/Gradient")]
    public class GradientEffect : BaseMeshEffect
    {
        public enum Type
        {
            Horizontal,
            Vertical
        }

        public enum Blend
        {
            Override,
            Add,
            Multiply
        }

        [SerializeField] private Type _gradientType;

        [SerializeField] private Blend _blendMode = Blend.Multiply;

        [SerializeField] [Range(-1, 1)] private float _offset;

        [SerializeField] private Gradient _effectGradient = new()
            { colorKeys = new[] { new GradientColorKey(Color.black, 0), new GradientColorKey(Color.white, 1) } };

        public override void ModifyMesh(VertexHelper helper)
        {
            if (!IsActive() || helper.currentVertCount == 0)
                return;

            var vertexList = new List<UIVertex>();

            helper.GetUIVertexStream(vertexList);

            var nCount = vertexList.Count;
            switch (GradientType)
            {
                case Type.Horizontal:
                {
                    var left = vertexList[0].position.x;
                    var right = vertexList[0].position.x;
                    var x = 0f;

                    for (var i = nCount - 1; i >= 1; --i)
                    {
                        x = vertexList[i].position.x;

                        if (x > right) right = x;
                        else if (x < left) left = x;
                    }

                    var width = 1f / (right - left);
                    var vertex = new UIVertex();

                    for (var i = 0; i < helper.currentVertCount; i++)
                    {
                        helper.PopulateUIVertex(ref vertex, i);

                        vertex.color = BlendColor(vertex.color,
                            EffectGradient.Evaluate((vertex.position.x - left) * width - Offset));

                        helper.SetUIVertex(vertex, i);
                    }
                }
                    break;

                case Type.Vertical:
                {
                    var bottom = vertexList[0].position.y;
                    var top = vertexList[0].position.y;
                    var y = 0f;

                    for (var i = nCount - 1; i >= 1; --i)
                    {
                        y = vertexList[i].position.y;

                        if (y > top) top = y;
                        else if (y < bottom) bottom = y;
                    }

                    var height = 1f / (top - bottom);
                    var vertex = new UIVertex();

                    for (var i = 0; i < helper.currentVertCount; i++)
                    {
                        helper.PopulateUIVertex(ref vertex, i);

                        vertex.color = BlendColor(vertex.color,
                            EffectGradient.Evaluate(vertex.position.y / top - Offset));

                        helper.SetUIVertex(vertex, i);
                    }
                }
                    break;
            }
        }

        private Color BlendColor(Color colorA, Color colorB)
        {
            switch (BlendMode)
            {
                default: return colorB;
                case Blend.Add: return colorA + colorB;
                case Blend.Multiply: return colorA * colorB;
            }
        }

        #region Properties

        public Blend BlendMode
        {
            get => _blendMode;
            set => _blendMode = value;
        }

        public Gradient EffectGradient
        {
            get => _effectGradient;
            set => _effectGradient = value;
        }

        public Type GradientType
        {
            get => _gradientType;
            set => _gradientType = value;
        }

        public float Offset
        {
            get => _offset;
            set => _offset = value;
        }

        #endregion
    }
}