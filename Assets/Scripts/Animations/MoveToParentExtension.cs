using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class MoveToParentExtension
    {
        public static IEnumerator MoveToParent(this MonoBehaviour child, Transform parent,
            float duration, Vector3 offset = default, float spacing = default,
            bool useScale = false, int tileCount = -1, bool ignoreChildren = false)
        {
            yield return new List<Transform> { child.transform }.MoveToParent(parent, duration, offset, spacing,
                useScale,
                false, tileCount);
        }

        public static IEnumerator MoveToParent(this Transform child, Transform parent,
            float duration, Vector3 offset = default, float spacing = default,
            bool useScale = false, int tileCount = -1, bool ignoreChildren = false)
        {
            yield return new List<Transform> { child }.MoveToParent(parent, duration, offset, spacing, useScale,
                false, tileCount);
        }

        public static IEnumerator MoveToParent(this List<Transform> children, Transform parent,
            float duration, Vector3 globalOffset = default, float spacing = default,
            bool useScale = false, bool randomOrder = false, int tileCount = -1, bool ignoreChildren = false)
        {
            var targetRotation = parent.rotation.eulerAngles;

            if (tileCount == -1)
                tileCount = children.Count;

            var existingTilesOffset = 0f;

            if (!ignoreChildren)
                existingTilesOffset = 1 + parent.Cast<Transform>().OrderBy(x => x.localPosition.x).LastOrDefault()?.localPosition.x ?? 0;

            for (var handIndex = 0; handIndex < tileCount; handIndex++)
            {
                var parentDirection = parent.rotation * Vector3.right;

                var index = randomOrder
                    ? Random.Range(0, children.Count)
                    : tileCount - (handIndex + 1);
                var tile = children[index];

                children.RemoveAt(index);

                var spacingVector = parentDirection * handIndex * spacing;
                var tileOffset = handIndex * parentDirection;
                var targetScale = tile.localScale;

                if (useScale)
                {
                    spacingVector.Scale(parent.localScale);
                    tileOffset.Scale(parent.localScale);
                    targetScale.Scale(parent.localScale);
                }

                var targetPosition = parent.position + globalOffset + tileOffset + spacingVector
                    + existingTilesOffset * parentDirection;

                yield return tile.gameObject.PickUpAndMove(duration / tileCount,
                    targetPosition, targetRotation, targetScale);
                tile.SetParent(parent, true);
            }
        }
    }
}