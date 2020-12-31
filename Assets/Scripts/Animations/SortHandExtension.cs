using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Components;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class SortHandExtension
    {
        public static IEnumerator SortHand(this Transform parent)
        {
            var hand = parent.GetComponentsInChildren<MahjongTileComponent>()
                .OrderByDescending(x => x.Tile.GetTileOrder()).ToList();

            foreach (var tile in hand)
            {
                yield return new[] {tile.transform}.ToList()
                    .MoveToParent(parent, .1f, Vector3.left);

                yield return new WaitForSeconds(.1f);

                var children = hand.OrderBy(x => x.transform.position.x);
                yield return MoveChildren(children, Vector3.right);
            }
        }

        private static IEnumerator MoveChildren(IEnumerable<MahjongTileComponent> parent,
            Vector3 movement)
        {
            const float duration = .1f;
            var children = parent.ToList();

            for (var index = 0; index < children.Count; index++)
            {
                var child = children[index];
                var nextChild = index == children.Count - 1 ? child : children[index + 1];

                child.StartCoroutine(child.transform.InterpolationAnimation(duration,
                    child.transform.position + movement));

                if (Vector3.Distance(child.transform.position, nextChild.transform.position) >
                    movement.magnitude * 1.5)
                    break;
            }

            yield return new WaitForSeconds(duration);
        }
    }
}