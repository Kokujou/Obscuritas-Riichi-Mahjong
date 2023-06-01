using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class CallAnimations
    {
        public const float SlideEndZ = -9;

        public static IEnumerator CollectDiscard(this MahjongTileComponent discard, Transform targetHand, float duration)
        {
            var targetPosition = targetHand.position.ReplaceZ(SlideEndZ) + Vector3.left;
            targetPosition = targetPosition.ReplaceY(-1);

            yield return discard.gameObject.InterpolationAnimation(duration * 0.45f,
                targetPosition + Vector3.up * 6,
                Vector3.right * 90,
                timingFunction: x => Mathf.Pow(x, 2)
                );
            yield return new WaitForSeconds(duration * 0.5f);
            yield return discard.gameObject.InterpolationAnimation(duration * 0.05f, targetPosition);
        }

        public static IEnumerator ExposeAndThrowTiles(this IEnumerable<MahjongTileComponent> tiles, Transform exposedParent, float duration)
        {
            tiles = tiles.OrderByDescending(x => x.transform.localPosition.x).ToList();

            var routines = tiles.Select(x => x.StartCoroutine(x.ExposeTiles(duration / 3))).ToList();
            foreach (var routine in routines) yield return routine;

            yield return new WaitForSeconds(duration / 3);

            yield return tiles.ThrowTiles(duration / 3, exposedParent);
        }

        private static IEnumerator ExposeTiles(this MahjongTileComponent tile, float duration)
        {
            var rotation = tile.transform.rotation.eulerAngles;
            yield return tile.StartParallelCoroutines(
                tile.gameObject.InterpolationAnimation(duration * 0.5f, targetRotation: new Vector3(90, rotation.y, rotation.z), timingFunction: x => 1f - Mathf.Pow(1f - x, 4f)),
                tile.gameObject.InterpolationAnimation(duration, tile.transform.position.ReplaceZ(SlideEndZ)));
        }

        private static IEnumerator ThrowTiles(this IEnumerable<MahjongTileComponent> tiles, float duration, Transform exposedParent)
        {
            var lastX = exposedParent.position.x + exposedParent.localScale.x;
            lastX -= (exposedParent.childCount + 1) * MahjongTileComponent.SizeX;

            var routines = new List<Coroutine>();
            foreach (var tile in tiles)
            {
                routines.Add(tile.StartCoroutine(tile.gameObject.InterpolationAnimation(duration,
                tile.transform.position.ReplaceX(lastX), timingFunction: x => Mathf.Sqrt(x))));
                lastX -= MahjongTileComponent.SizeX;
            }

            foreach (var routine in routines) yield return routine;
        }
    }
}