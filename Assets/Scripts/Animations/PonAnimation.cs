using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class PonAnimation
    {
        public static IEnumerator DoPonAnimation(this MahjongTileComponent discardedTile,
            MahjongPlayerComponentBase player, float duration)
        {
            var tiles = player.HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == discardedTile.Tile);

            foreach (var tile in tiles)
                yield return tile.gameObject.InterpolationAnimation(duration / 3, targetRotation: Vector3.zero);
        }
    }
}