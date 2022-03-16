using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
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

                var targetPosition = parent.position + parent.rotation * Vector3.right * index * 1.1f;
                child.StartCoroutine(child.InterpolationAnimation(duration / 2f, targetPosition));
            }

            yield return new WaitForSeconds(duration / 2f);

            var insertedTileIndex = tilesByPosition.IndexOf(tile);
            var insertedTargetPosition = parent.position + parent.rotation * Vector3.right * insertedTileIndex * 1.1f;
            yield return tile.gameObject.PickUpAndMove(duration / 2f, insertedTargetPosition);
        }
    }
}