using System;
using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Components.Interface;
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
            HandParent.Rotate(45, 0, 0);

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

        public override void Pon()
        {
            throw new NotImplementedException();
        }

        public override void Chi()
        {
            throw new NotImplementedException();
        }

        public override void OpenKan()
        {
            throw new NotImplementedException();
        }

        public override void HiddenKan()
        {
            throw new NotImplementedException();
        }

        public override void Ron()
        {
            throw new NotImplementedException();
        }

        public override void Tsumo()
        {
            throw new NotImplementedException();
        }

        public override void Riichi()
        {
            throw new NotImplementedException();
        }
    }
}