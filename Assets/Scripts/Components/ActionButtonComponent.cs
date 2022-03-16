using System;
using System.Collections;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.Components
{
    [RequireComponent(typeof(RawImage))]
    public class ActionButtonComponent : MonoBehaviour
    {
        public CallType MoveType;

        public MahjongPlayerComponentBase _mahjongPlayer;
        private MahjongTileComponent _lastDiscardedTile;

        public Vector3 ForcePositionOffset = Vector3.up * 1;

        public bool Submitted { get; private set; }

        public ActionButtonComponent Initialize(MahjongPlayerComponentBase player, MahjongTileComponent lastDiscard)
        {
            _mahjongPlayer = player;
            _lastDiscardedTile = lastDiscard;
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
            StartCoroutine(TriggerActionOnPlayer());
        }

        public IEnumerator TriggerActionOnPlayer()
        {
            yield return MoveType switch
            {
                CallType.Skip => null,
                CallType.Pon => _mahjongPlayer.Pon(_lastDiscardedTile),
                CallType.Chi => _mahjongPlayer.Chi(_lastDiscardedTile),
                CallType.OpenKan => _mahjongPlayer.OpenKan(_lastDiscardedTile),
                CallType.HiddenKan => _mahjongPlayer.HiddenKan(_lastDiscardedTile),
                CallType.Riichi => _mahjongPlayer.Riichi(),
                CallType.Tsumo => _mahjongPlayer.Tsumo(),
                CallType.Ron => _mahjongPlayer.Ron(),
                _ => throw new ArgumentOutOfRangeException()
            };


            Submitted = true;
        }
    }
}