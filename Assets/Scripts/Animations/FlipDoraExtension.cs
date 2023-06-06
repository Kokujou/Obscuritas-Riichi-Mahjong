using ObscuritasRiichiMahjong.Components;
using System.Collections;
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