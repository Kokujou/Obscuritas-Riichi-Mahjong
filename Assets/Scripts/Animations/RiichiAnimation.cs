using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using System.Collections;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Animations
{
    public static class RiichiAnimation
    {
        public static IEnumerator DoRiichiAnimation(this MahjongPlayerComponentBase player,
            MahjongTileComponent riichiTile, float duration)
        {
            yield return DiscardRiichiTile(riichiTile, player.ExposedTilesParent, duration);
            player.StartCoroutine(SpawnRiichiStick());
        }

        private static IEnumerator SpawnRiichiStick()
        {
            yield return null;
        }

        private static IEnumerator DiscardRiichiTile(MahjongTileComponent riichiTile, Transform exposedTilesParent, float duration)
        {
            yield return riichiTile.MoveToParent(exposedTilesParent, duration * 0.9f);
            yield return riichiTile.InterpolationAnimation(duration * 0.1f, targetRotation: new());
        }
    }
}
