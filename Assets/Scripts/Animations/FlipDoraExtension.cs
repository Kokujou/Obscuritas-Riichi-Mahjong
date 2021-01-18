using System.Collections;
using ObscuritasRiichiMahjong.Components;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Animations
{
    public static class FlipDoraExtension
    {
        public static IEnumerator FlipDora(this MahjongTileComponent dora)
        {
            yield return dora.InterpolationAnimation(.5f, targetRotation: Vector3.zero);
        }
    }
}