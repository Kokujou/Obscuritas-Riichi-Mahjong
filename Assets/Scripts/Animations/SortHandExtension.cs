using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Core.Extensions;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class SortHandExtension
    {
        public static IEnumerator InsertTile(this MahjongTileComponent tile, Transform parent, float duration)
        {
            var tilesByPosition = parent.GetComponentsInChildren<MahjongTileComponent>()
                .OrderBy(x => x.Tile.GetTileOrder()).ToList();

            for (var index = 0; index < tilesByPosition.Count; index++)
            {
                var child = tilesByPosition[index];

                if (child == tile) continue;
                if (child.transform.localPosition.x.AboutEquals(index))
                    continue;

                var targetPosition = parent.position + parent.rotation * Vector3.right * index;
                child.StartCoroutine(child.InterpolationAnimation(duration / 2f, targetPosition));
            }

            yield return new WaitForSeconds(duration / 2f);

            var insertedTileIndex = tilesByPosition.IndexOf(tile);
            if (tile.transform.localPosition.x.AboutEquals(insertedTileIndex)) yield break;
            var insertedTargetPosition = parent.position + parent.rotation * Vector3.right * insertedTileIndex;
            yield return tile.gameObject.PickUpAndMove(duration / 2f, insertedTargetPosition);
        }
    }
}