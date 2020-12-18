using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class MoveToParentExtension
    {
        public static IEnumerator MoveToParent(this List<Transform> children, Transform parent,
            float duration, Vector3 offset = default, float spacing = default,
            bool useScale = false, bool randomOrder = false, int tileCount = -1)
        {
            var targetRotation = parent.rotation.eulerAngles;

            if (tileCount == -1)
                tileCount = children.Count;

            for (var handIndex = 0; handIndex < tileCount; handIndex++)
            {
                var parentDirection = parent.rotation * Vector3.right;

                var index = randomOrder
                    ? Random.Range(0, children.Count)
                    : tileCount - (handIndex + 1);
                var tile = children[index];

                children.RemoveAt(index);

                var spacingVector = parentDirection * handIndex * spacing;
                var tileWidth = handIndex * parentDirection;
                var targetScale = tile.localScale;

                if (useScale)
                {
                    spacingVector.Scale(parent.localScale);
                    tileWidth.Scale(parent.localScale);
                    targetScale.Scale(parent.localScale);
                }

                var targetPosition =
                    parent.position + offset + tileWidth + spacingVector;

                yield return tile.gameObject.PickUpAndMove(duration / tileCount,
                    targetPosition, targetRotation, targetScale);
                tile.SetParent(parent, true);
            }
        }
    }
}