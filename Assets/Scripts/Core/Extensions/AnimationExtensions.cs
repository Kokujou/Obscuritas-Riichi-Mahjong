using System.Collections;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions
{
    public static class AnimationExtensions
    {
        public static IEnumerator StartParallelCoroutines(this MonoBehaviour target, params IEnumerator[] coroutines)
        {
            var startedCoroutines = coroutines.Select(x => target.StartCoroutine(x)).ToList();
            foreach (var coroutine in startedCoroutines) yield return coroutine;

        }
    }
}
