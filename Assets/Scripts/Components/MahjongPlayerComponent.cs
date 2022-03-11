using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Global;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongPlayerComponent : MahjongPlayerComponentBase
    {
        private static MahjongTileComponent _activeTile;
        private MahjongTileComponent _lastTile;

        private MahjongTileComponent _clickedTile;

        public override void ScanHand()
        {
            foreach (Transform child in HandParent)
            {
                var mahjongTile = child.GetComponent<MahjongTileComponent>();

                if (!mahjongTile) continue;
                Player.Hand.Add(mahjongTile.Tile);
            }
        }

        public override IEnumerator DiscardTile(MahjongTileComponent tile)
        {
            _clickedTile = null;

            _activeTile?.Unhover();
            _activeTile = null;

            yield return base.DiscardTile(tile);
            _lastTile = tile;
        }

        public override IEnumerator MakeTurn()
        {
            while (!_lastTile)
            {
                yield return null;
                HandlePlayerInput();
            }

            LastDiscardedTile = _lastTile;
            Board.LastDiscardedTile = _lastTile.Tile;
            _lastTile = null;
        }

        public void HandlePlayerInput()
        {
            HandleTileHover();

            HandleTileClick();
        }

        private void HandleTileHover()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            var activeSameTiles = Enumerable.Empty<MahjongTileComponent>();
            if (_activeTile)
                activeSameTiles = MahjongMain.GetSameTiles(_activeTile);

            if (!Physics.Raycast(ray, out var hit, 1000f, ~LayerMask.NameToLayer("MahjongTile")))
            {
                if (!_activeTile)
                    return;

                foreach (var tile in activeSameTiles) tile.Unhover();
                _clickedTile = null;
                _activeTile = null;
                return;
            }

            var objectHit = hit.transform;
            var mahjongTileComponent = objectHit.GetComponent<MahjongTileComponent>();
            var currentSameTiles = MahjongMain.GetSameTiles(mahjongTileComponent);

            if (_activeTile == mahjongTileComponent ||
                mahjongTileComponent.transform.parent != HandParent) return;

            foreach (var tile in activeSameTiles) tile.Unhover();
            _clickedTile = null;

            if (!mahjongTileComponent)
                return;

            foreach (var tile in currentSameTiles) tile.Hover();
            _activeTile = mahjongTileComponent;
        }

        private void HandleTileClick()
        {
            if (Input.GetMouseButtonDown(0))
                _clickedTile = _activeTile;

            if (!_clickedTile || !Input.GetMouseButtonUp(0)) return;

            var playerHand = GetComponentInParent<MahjongPlayerComponent>();

            if (!playerHand)
                return;

            StartCoroutine(DiscardTile(_clickedTile));
        }

        public override IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile)
        {
            yield return null;
            var possibleCalls = new List<CallType> { CallType.Skip };

            if (Player.CanPon(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.Pon);
            if (Player.CanChi(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.Chi);
            if (Player.CanKan(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.OpenKan);
            if (Player.CanRon(lastDiscardedTile.Tile))
                possibleCalls.Add(CallType.Ron);

            if (possibleCalls.Count <= 1)
                yield break;

            var actionButtons = new List<ActionButtonComponent>();
            foreach (var possibleCall in possibleCalls)
            {
                var actionButton = Instantiate(
                    PrefabCollection.Instance.ActionButtonDictionary[possibleCall],
                    SceneObjectCollection.Instance.ActionButtonPanel).GetComponent<ActionButtonComponent>();

                actionButton.Initialize(this, lastDiscardedTile);
                actionButtons.Add(actionButton);
            }

            while (true)
            {
                if (actionButtons.Any(x => x.Submitted))
                {
                    foreach (var actionButton in actionButtons) Destroy(actionButton.gameObject);
                    yield break;
                }

                yield return null;
            }
        }
    }
}