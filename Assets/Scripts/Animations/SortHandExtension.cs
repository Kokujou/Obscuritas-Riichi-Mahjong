using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Components;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class SortHandExtension
    {
        public static IEnumerator InsertTile(this MahjongTileComponent tile, Transform parent, float duration)
        {
            var tilesByPosition = parent.GetComponentsInChildren<MahjongTileComponent>()
                .OrderBy(x => x.Tile.GetTileOrder()).ToList();
            var coroutines = new List<IEnumerator>();

            for (var index = 0; index < tilesByPosition.Count; index++)
            {
                var child = tilesByPosition[index];

                if (child == tile) continue;

                var targetPosition = parent.position + parent.rotation * Vector3.right * index * 1.1f;
                coroutines.Add(child.InterpolationAnimation(duration / 2f, targetPosition));
            }

            yield return tile.StartParallelCoroutines(coroutines.ToArray());

            var insertedTileIndex = tilesByPosition.IndexOf(tile);
            var insertedTargetPosition = parent.position + parent.rotation * Vector3.right * insertedTileIndex * 1.1f;
            yield return tile.gameObject.PickUpAndMove(duration / 2f, insertedTargetPosition);
        }
    }
}