using System.Collections;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class InterpolationAnimationExtension
    {
        public static IEnumerator InterpolationAnimation(this GameObject target, float duration,
            Vector3? targetPosition = null, Vector3? targetRotation = null,
            Vector3? targetScale = null)
        {
            yield return target.transform.InterpolationAnimation(duration, targetPosition,
                targetRotation, targetScale);
        }

        public static IEnumerator InterpolationAnimation<T>(this T target, float duration,
         Vector3? targetPosition = null, Vector3? targetRotation = null,
         Vector3? targetScale = null) where T : MonoBehaviour
        {
            yield return target.transform.InterpolationAnimation(duration, targetPosition,
                targetRotation, targetScale);
        }


        public static IEnumerator InterpolationAnimation(this Transform target, float duration,
            Vector3? targetPosition = null, Vector3? targetRotation = null,
            Vector3? targetScale = null)
        {
            var transform = target;
            var startTime = Time.time;

            var targetPos = target.position;
            var targetRot = target.rotation.eulerAngles;
            var targetSize = target.localScale;

            if (targetPosition.HasValue)
                targetPos = targetPosition.Value;
            if (targetRotation.HasValue)
                targetRot = targetRotation.Value;
            if (targetScale.HasValue)
                targetSize = targetScale.Value;

            var startPos = transform.position;
            var startRot = transform.rotation.eulerAngles;
            var startScale = transform.localScale;
            while (transform.position != targetPos ||
                   transform.localScale != targetSize ||
                   transform.rotation.eulerAngles != targetRot)
            {
                var elapsed = Time.time - startTime;
                if (elapsed >= duration)
                {
                    transform.position = targetPos;
                    transform.rotation = Quaternion.Euler(targetRot);
                    transform.localScale = targetSize;
                    break;
                }

                var posStep = (targetPos - startPos) / (duration / elapsed);
                if (transform.position != targetPos)
                {
                    if (Vector3.Distance(transform.position, targetPos) <
                        Vector3.Distance(startPos + posStep, targetPos))
                        transform.position = targetPos;
                    else
                        transform.position = startPos + posStep;
                }

                transform.rotation = Quaternion.Slerp(Quaternion.Euler(startRot),
                    Quaternion.Euler(targetRot), elapsed / duration > 1 ? 1 : elapsed / duration);

                var scaleStep = (targetSize - startScale) / (duration / elapsed);
                if (transform.localScale != targetSize)
                    if (Vector3.Distance(transform.localScale, targetSize) <
                        Vector3.Distance(startScale + scaleStep, targetSize))
                    {
                        transform.localScale = targetSize;
                    }
                    else
                    {
                        var nextSize = startScale + scaleStep;
                        transform.localScale = nextSize;
                    }

                yield return new WaitForSeconds(.0f);
            }
        }
    }
}