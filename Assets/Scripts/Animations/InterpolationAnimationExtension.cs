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

            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                float t = timingFunction(elapsedTime / duration);

                if (targetPosition is not null) target.position = Vector3.Lerp(startPos, targetPosition.Value, t);
                if (targetRotation is not null) target.rotation = Quaternion.Slerp(Quaternion.Euler(startRot.eulerAngles), Quaternion.Euler(targetRotation.Value), t);
                if (targetScale is not null) target.localScale = Vector3.Lerp(startScale, targetScale.Value, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (targetPosition is not null) target.position = targetPosition.Value;
            if (targetRotation is not null) target.rotation = Quaternion.Euler(targetRotation.Value);
            if (targetScale is not null) target.localScale = targetScale.Value;
        }

    }
}