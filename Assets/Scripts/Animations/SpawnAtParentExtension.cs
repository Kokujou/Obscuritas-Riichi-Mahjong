using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Core.Extensions;
using ObscuritasRiichiMahjong.Models;
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
            yield return new[] { component }.SpawnAtParent(parent, duration);
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
                component.transform.localPosition = (index + globalOffset) * Vector3.right * 1.1f + Vector3.up;

                var subDuration = duration / componentCount;
                component.StartCoroutine(component.FadeIn(subDuration));
                component.StartCoroutine(component.InterpolationAnimation(subDuration,
                    component.transform.position - parent.rotation * Vector3.up, parent.rotation.eulerAngles));

                yield return new WaitForSeconds(subDuration);
                index++;
            }

            yield return null;
        }

        public static IEnumerable<MahjongTileComponent> RandomSubsetSpawns(this IEnumerable<MahjongTile> tileSet,
            int count, out List<MahjongTile> leftover)
        {
            return tileSet.TransformRandomSubset(count, mahjongTile =>
            {
                var mahjongTileComponent = MahjongTileComponent.FromTile(mahjongTile);
                return mahjongTileComponent;
            }, out leftover);
        }

        private static IEnumerator FadeIn(this Component target, float duration)
        {
            var materials = target.GetComponentsInChildren<MeshRenderer>()
                .Select(x => x.material).Cast<dynamic>();
            var textMeshs = target.GetComponentsInChildren<TextMesh>();
            var coloredObjects = materials
                .Union(textMeshs);

            yield return coloredObjects.FadeColors(duration);
        }

        private static IEnumerator FadeColors(this IEnumerable<dynamic> objects, float duration)
        {
            var coloredObjects = objects.ToList();
            var startTime = Time.time;
            while (Time.time <= startTime + duration)
            {
                var newAlpha = (Time.time - startTime) / duration;

                foreach (var coloredObject in coloredObjects)
                {
                    var color = coloredObject.color;
                    coloredObject.color = new Color(color.r, color.g, color.b, newAlpha);
                }

                yield return null;
            }

            foreach (var coloredObject in coloredObjects)
            {
                var color = coloredObject.color;
                coloredObject.color = new Color(color.r, color.g, color.b, 1);
            }
        }
    }
}