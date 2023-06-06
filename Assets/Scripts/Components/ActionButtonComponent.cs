using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.Components
{
    [RequireComponent(typeof(RawImage))]
    public class ActionButtonComponent : MonoBehaviour
    {
        private static List<ActionButtonComponent> Instances = new();

        public CallType MoveType;

        public MahjongPlayerComponent _mahjongPlayer;

        public Vector3 ForcePositionOffset = Vector3.up * 1;

        public bool Submitted { get; private set; }

        public ActionButtonComponent Initialize(MahjongPlayerComponentBase player)
        {
            Instances.Add(this);
            _mahjongPlayer = (MahjongPlayerComponent)player;
            return this;
        }

        public void PointerExit()
        {
            transform.localScale = Vector3.one;
        }

        public void PointerEnter()
        {
            transform.localScale = Vector3.one * 1.2f;
        }

        public void Click()
        {
            _mahjongPlayer.StartCoroutine(TriggerActionOnPlayer());
            foreach (var instance in Instances) Destroy(instance.gameObject);
            Instances.Clear();
        }

        public IEnumerator TriggerActionOnPlayer()
        {

            yield return MoveType switch
            {
                CallType.Skip => null,
                CallType.Pon => _mahjongPlayer.Pon(),
                CallType.Chi => _mahjongPlayer.Chi(),
                CallType.OpenKan => _mahjongPlayer.OpenKan(),
                CallType.HiddenKan => _mahjongPlayer.HiddenKan(),
                CallType.Riichi => _mahjongPlayer.Riichi(),
                CallType.Tsumo => _mahjongPlayer.Tsumo(),
                CallType.Ron => _mahjongPlayer.Ron(),
                _ => throw new ArgumentOutOfRangeException()
            };

            Submitted = true;
        }
    }
}