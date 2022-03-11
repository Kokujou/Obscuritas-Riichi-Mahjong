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

        public void Initialize(MahjongPlayerComponentBase player, MahjongTileComponent lastDiscard)
        {
            _mahjongPlayer = player;
            _lastDiscardedTile = lastDiscard;
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
            _lastDiscardedTile = _mahjongPlayer.HandParent.GetComponentsInChildren<MahjongTileComponent>()[0];

            yield return MoveType switch
            {
                CallType.Skip => null,
                CallType.Pon => _mahjongPlayer.Pon(_lastDiscardedTile,
                    _lastDiscardedTile.transform.rotation * ForcePositionOffset),
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

        private void OnDrawGizmos()
        {
            try
            {
                var discardedTileTransform =
                    _mahjongPlayer.HandParent.GetComponentsInChildren<MahjongTileComponent>()[0].transform;
                var direction = discardedTileTransform.rotation * Vector3.forward;
                Gizmos.DrawRay(discardedTileTransform.position + transform.rotation * (Vector3.up * .1f), direction);
            }
            catch{}
        }
    }
}