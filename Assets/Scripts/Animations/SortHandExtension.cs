using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class SortHandExtension
    {
        public static IEnumerator SortHand(this Transform parent, float duration)
        {
            var hand = parent.GetComponentsInChildren<MahjongTileComponent>()
                .OrderBy(x => x.Tile.GetTileOrder()).ToList();
            var perTileDuration = duration / hand.Count;

            for (var index = 0; index < hand.Count; index++)
            {
                var tile = hand[index];
                yield return tile.transform.MoveToParent(parent, perTileDuration / 3f,
                    ignoreChildren: true);

                yield return new WaitForSeconds(perTileDuration / 3f);

                var children = hand.OrderByDescending(x => x.transform.localPosition.x);
                yield return children.Move(parent.rotation * Vector3.left, perTileDuration / 3f);
                tile.transform.SetSiblingIndex(index);
            }
        }

        public static IEnumerator Move<T>(this IEnumerable<T> tiles, Vector3 movement, float duration)
            where T : MonoBehaviour
        {
            var children = tiles.ToList();

            for (var index = 0; index < children.Count; index++)
            {
                var child = children[index];
                var nextChild = index == children.Count - 1 ? child : children[index + 1];

                child.StartCoroutine(child.InterpolationAnimation(duration,
                    child.transform.position + movement));

                if (Vector3.Distance(child.transform.position, nextChild.transform.position) >
                    movement.magnitude * 1.5)
                    break;
            }

            yield return new WaitForSeconds(duration);
        }
    }
}