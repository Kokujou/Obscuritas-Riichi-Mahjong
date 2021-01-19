using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class SpawnAtParentExtension
    {
        public static IEnumerator SpawnAtParent(this MonoBehaviour component, Transform parent, float duration)
        {
            yield return new[] {component}.SpawnAtParent(parent, duration);
        }

        public static IEnumerator SpawnAtParent<T>(this IEnumerable<T> components, Transform parent,
            float duration) where T : MonoBehaviour
        {
            var index = 0;
            var componentList = components.ToList();
            var componentCount = componentList.Count;
            var globalOffset =
                1 + parent.Cast<Transform>().OrderBy(x => x.localPosition.x).LastOrDefault()?.localPosition.x ?? 0;
            foreach (var component in componentList)
            {
                component.transform.SetParent(parent, true);

                component.transform.localRotation = Quaternion.Euler(Vector3.zero);
                component.transform.localPosition = (index + globalOffset) * Vector3.right + Vector3.up;

                var subDuration = duration / componentCount;
                component.StartCoroutine(component.FadeIn(subDuration));
                component.StartCoroutine(component.InterpolationAnimation(subDuration,
                    component.transform.position - parent.rotation * Vector3.up, parent.rotation.eulerAngles));

                yield return new WaitForSeconds(subDuration);
                index++;
            }

            yield return null;
        }

        public static IEnumerator FadeIn(this MonoBehaviour target, float duration)
        {
            var renderers = target.GetComponentsInChildren<MeshRenderer>();
            var startTime = Time.time;
            while (Time.time <= startTime + duration)
            {
                var newAlpha = (Time.time - startTime) / duration;

                foreach (var renderer in renderers)
                {
                    var color = renderer.material.color;
                    renderer.material.color = new Color(color.r, color.g, color.b, newAlpha);
                }

                yield return null;
            }

            foreach (var renderer in renderers)
            {
                var color = renderer.material.color;
                renderer.material.color = new Color(color.r, color.g, color.b, 1);
            }
        }
    }
}