using System.Collections;
using System.Threading;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Animations
{
    public static class MaterialColorAnimations
    {
        public static IEnumerator BlinkColor(this Material material, float duration, CancellationToken cancellation)
        {
            var startTime = Time.time;
            while (!cancellation.IsCancellationRequested)
            {
                yield return null;
                var step = (Time.time - startTime) % duration;
                var alpha = (Mathf.Sin((step / duration) * Mathf.PI)) - .3f;
                material.color = new Color(material.color.r, material.color.g, material.color.b, alpha);
            }

            material.color = new Color(material.color.r, material.color.g, material.color.b, 0);
        }
    }
}
