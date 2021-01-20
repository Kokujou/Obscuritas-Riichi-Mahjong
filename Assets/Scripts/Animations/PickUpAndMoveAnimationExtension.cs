using System.Collections;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class PickUpAndMoveAnimationExtension
    {
        public static IEnumerator PickUpAndMove(this GameObject target, float duration,
            Vector3 targetPosition, Vector3? targetRotation = null, Vector3? targetScale = null)
        {
            var rigidBody = target.GetComponent<Rigidbody>();
            var collider = target.GetComponent<Collider>();

            rigidBody.isKinematic = true;

            var firstAnimationDuration = duration / 2f;
            yield return target
                .InterpolationAnimation(firstAnimationDuration,
                    target.transform.position + Vector3.up * 5);

            yield return target.InterpolationAnimation(duration / 2f, targetPosition,
                targetRotation, targetScale);
        }
    }
}