using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions
{
    public static class AnimationExtensions
    {
        public static IEnumerator StartParallelCoroutines(this MonoBehaviour target, params IEnumerator[] coroutines)
        {
            var startedCoroutines = new List<Coroutine>();
            foreach (var coroutine in coroutines) startedCoroutines.Add(target.StartCoroutine(coroutine));
            foreach (var coroutine in startedCoroutines) yield return coroutine;

        }
    }
}
