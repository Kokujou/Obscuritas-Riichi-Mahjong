using System;
using System.Collections;
using System.Collections.Generic;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongPlayerComponent : MahjongPlayerComponentBase
    {
        private static MahjongTileComponent _activeTile;
        private MahjongTileComponent _lastTile;

        private readonly List<MahjongTileComponent> HandTiles = new List<MahjongTileComponent>();

        private MahjongTileComponent _clickedTile;

        public override void ScanHand()
        {
            HandParent.Rotate(45, 0, 0);

            foreach (Transform child in HandParent)
            {
                var mahjongTile = child.GetComponent<MahjongTileComponent>();

                if (!mahjongTile) continue;
                HandTiles.Add(mahjongTile);
                Player.Hand.Add(mahjongTile.Tile);

                if (mahjongTile)
                    mahjongTile.Selectable = true;
            }
        }

        public override void DiscardTile(MahjongTileComponent tile)
        {
            _lastTile = tile;
        }

        public override IEnumerator MakeTurn()
        {
            while (!_lastTile)
            {
                yield return null;
                HandlePlayerInput();
            }

            var tile = _lastTile;
            _lastTile = null;
        }

        public void HandlePlayerInput()
        {
            if (HandleTileHover()) return;

            HandleTileClick();
        }

        private bool HandleTileHover()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 1000f, ~LayerMask.NameToLayer("MahjongTile")))
            {
                if (!_activeTile)
                    return true;

                _activeTile.Unhover();
                _clickedTile = null;
                _activeTile = null;
                return true;
            }

            var objectHit = hit.transform;
            var mahjongTileComponent = objectHit.GetComponent<MahjongTileComponent>();

            if (_activeTile == mahjongTileComponent || !HandTiles.Contains(mahjongTileComponent)) return true;

            _activeTile?.Unhover();
            _clickedTile = null;

            if (!mahjongTileComponent)
                return true;

            mahjongTileComponent.Hover();
            _activeTile = mahjongTileComponent;
            return false;
        }

        private void HandleTileClick()
        {
            if (Input.GetMouseButtonDown(0))
                _clickedTile = _activeTile;

            if (_clickedTile && Input.GetMouseButtonUp(0))
            {
                var playerHand = GetComponentInParent<MahjongPlayerComponent>();

                if (!playerHand)
                    return;

                DiscardTile(_clickedTile);
            }
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