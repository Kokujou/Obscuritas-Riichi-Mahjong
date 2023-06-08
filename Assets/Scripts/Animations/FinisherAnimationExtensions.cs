using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Animations
{
    public static class FinisherAnimationExtensions
    {
        public static IEnumerator ThrowLastTile(this MahjongTileComponent lastTile, Transform targetHand, float duration)
        {
            lastTile.transform.SetParent(null, true);
            var lastExposedTile = targetHand.Cast<Transform>().OrderBy(x => x.localPosition.x).LastOrDefault();
            var lastTileX = lastExposedTile ? lastExposedTile.localPosition.x + 1f : 0f;
            var targetPosition = targetHand.position + Vector3.right * (lastTileX + 0.3f) + Vector3.right;
            var targetRotation = Vector3.right * 90;

            yield return lastTile.gameObject
                .InterpolationAnimation(duration * 0.6f, targetPosition + Vector3.up * 10, targetRotation);
            yield return new WaitForSeconds(duration * 0.3f);
            yield return lastTile.gameObject
                .InterpolationAnimation(duration * 0.1f, targetPosition);
        }

        public static IEnumerator ExposeHand(this Transform hand, float duration)
        {
            yield return hand.InterpolationAnimation(duration, targetRotation: Vector3.right * 90);
        }
    }
}
