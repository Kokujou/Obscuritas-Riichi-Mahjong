using ObscuritasRiichiMahjong.Core.Extensions;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class MoveToParentExtension
    {
        public static IEnumerator MoveToParent(this Transform tile, Transform parent,
            float duration)
        {
            var lastChild = parent.Cast<Transform>().OrderBy(x => x.localPosition.x).LastOrDefault();
            var newPosition = parent.position;
            if (lastChild)
            {
                var lastObjectBounds = lastChild.gameObject.CalculateGlobalBounds();
                var newObjectBounds = tile.gameObject.CalculateGlobalBounds();
                newPosition = lastChild.position + parent.right * (lastObjectBounds.size.x + newObjectBounds.size.x) / 2f;
            }

            var parentDirection = parent.rotation * Vector3.right;
            var targetRotation = parent.rotation.eulerAngles;

            yield return tile.gameObject.PickUpAndMove(duration, newPosition, targetRotation);
            tile.SetParent(parent, true);
        }
    }
}