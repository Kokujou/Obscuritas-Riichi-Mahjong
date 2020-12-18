using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class MoveToParentExtension
    {
        public static IEnumerator MoveToParent(this List<Transform> children, Transform parent,
            Vector3 offset = default, float spacing = default, bool useScale = false,
            bool randomOrder = false)
        {
            var targetRotation = parent.rotation.eulerAngles;

            for (var handIndex = 0; handIndex < children.Count; handIndex++)
            {
                var parentDirection = parent.rotation * Vector3.right;

                var index = randomOrder ? Random.Range(0, parent.childCount) : handIndex;
                var tile = children[index];

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

                yield return tile.gameObject.PickUpAndMove(.1f, targetPosition, targetRotation,
                    targetScale);
                tile.SetParent(parent, true);
            }
        }
    }
}