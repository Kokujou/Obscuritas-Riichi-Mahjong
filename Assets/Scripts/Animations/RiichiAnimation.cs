using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Extensions;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Animations
{
    public static class RiichiAnimation
    {
        public static IEnumerator DoRiichiAnimation(this MahjongPlayerComponentBase player,
            MahjongTileComponent riichiTile, float duration)
        {
            yield return DiscardRiichiTile(riichiTile, player.DiscardedTilesParent, duration);
            player.StartCoroutine(SpawnRiichiStick());
        }

        private static IEnumerator SpawnRiichiStick()
        {
            yield return null;
        }

        private static IEnumerator DiscardRiichiTile(MahjongTileComponent riichiTile, Transform discardedTilesParent, float duration)
        {
            var lastExposedTile = discardedTilesParent.Cast<Transform>().OrderBy(x => x.localPosition.x).LastOrDefault();
            var lastTileX = lastExposedTile?.localPosition.x ?? 0f;
            var targetPosition = discardedTilesParent.position + Vector3.right * lastTileX;
            var targetRotation = discardedTilesParent.rotation.eulerAngles.ReplaceZ(90);

            yield return riichiTile.gameObject.PickUpAndMove(duration, targetPosition, targetRotation);
            riichiTile.transform.SetParent(discardedTilesParent, true);
        }
    }
}
