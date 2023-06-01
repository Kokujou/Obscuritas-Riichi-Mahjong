using System;
using System.Collections;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class InterpolationAnimationExtension
    {
        public static IEnumerator InterpolationAnimation(this GameObject target, float duration,
            Vector3? targetPosition = null, Vector3? targetRotation = null,
            Vector3? targetScale = null, Func<float, float> timingFunction = null)
        {
            yield return target.transform.InterpolationAnimation(duration, targetPosition,
                targetRotation, targetScale, timingFunction);
        }

        public static IEnumerator InterpolationAnimation<T>(this T target, float duration,
            Vector3? targetPosition = null, Vector3? targetRotation = null,
            Vector3? targetScale = null, Func<float, float> timingFunction = null) where T : MonoBehaviour
        {
            yield return target.transform.InterpolationAnimation(duration, targetPosition,
                targetRotation, targetScale, timingFunction);
        }

        public static IEnumerator InterpolationAnimation(this Transform target, float duration,
            Vector3? targetPosition = null, Vector3? targetRotation = null,
            Vector3? targetScale = null, Func<float, float> timingFunction = null)
        {
            timingFunction ??= x => x;

            var startTime = Time.time;
            var startPos = target.position;
            var startRot = target.rotation;
            var startScale = target.localScale;
            for (var elapsed = 0f; elapsed < duration; elapsed = Time.time - startTime)
            {
                var progress = Mathf.Clamp(timingFunction(elapsed / duration), 0, 1);

                if (targetPosition is not null) target.TransformPosition(startPos, targetPosition.Value, progress);
                if (targetRotation is not null)
                    target.rotation = Quaternion.Slerp(startRot, Quaternion.Euler(targetRotation.Value), progress);
                if (targetScale is not null) target.TransformScale(startScale, targetScale.Value, progress);

                yield return null;
            }

            if (targetPosition is not null)
                target.position = targetPosition.Value;
            if (targetRotation is not null) target.rotation = Quaternion.Euler(targetRotation.Value);
            if (targetScale is not null) target.localScale = targetScale.Value;
            yield return null;
        }

        public static void TransformPosition(this Transform transform, Vector3 startPos, Vector3 targetPos, float progress)
        {
            var posStep = (targetPos - startPos) * progress;
            if (transform.position == targetPos) return;

            if (Vector3.Distance(transform.position, targetPos) < Vector3.Distance(startPos + posStep, targetPos))
                transform.position = targetPos;
            else
                transform.position = startPos + posStep;
        }

        public static void TransformScale(this Transform transform, Vector3 startScale, Vector3 targetScale, float progress)
        {
            var scaleStep = (targetScale - startScale) * progress;
            if (transform.localScale == targetScale)
                return;

            if (Vector3.Distance(transform.localScale, targetScale) < Vector3.Distance(startScale + scaleStep, targetScale))
                transform.localScale = targetScale;
            else
                transform.localScale = startScale + scaleStep;
        }
    }
}