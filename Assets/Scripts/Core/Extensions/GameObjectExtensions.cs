using UnityEngine;

namespace ObscuritasRiichiMahjong.Core.Extensions
{
    public static class GameObjectExtensions
    {
        public static Bounds CalculateGlobalBounds(this GameObject gameObject)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) return new Bounds();

            var bounds = renderers[0].bounds;
            foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);

            return bounds;
        }
    }
}
