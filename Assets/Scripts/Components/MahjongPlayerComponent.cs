using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongPlayerComponent : MahjongPlayerComponentBase
    {
        private static MahjongTileComponent _activeTile;
        private MahjongTileComponent _lastTile;

        public override void DiscardTile(MahjongTileComponent tile)
        {
            _lastTile = tile;
        }

        public override void Initialize(ref List<CardinalPoint> availableWinds, MahjongBoard board)
        {
            base.Initialize(ref availableWinds, board);
            HandParent.Rotate(45, 0, 0);

            foreach (Transform child in HandParent)
            {
                var mahjongTile = child.GetComponent<MahjongTileComponent>();

                if (!mahjongTile) continue;

                if (mahjongTile)
                    mahjongTile.Selectable = true;
            }
        }

        public override async Task<MahjongTileComponent> MakeTurn()
        {
            while (!_lastTile)
                await Task.Yield();

            var tile = _lastTile;
            _lastTile = null;

            return tile;
        }

        public void Update()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 1000f, ~LayerMask.NameToLayer("MahjongTile")))
            {
                if (!_activeTile)
                    return;

                _activeTile.HandleMouseOut();
                _activeTile = null;
                return;
            }

            var objectHit = hit.transform;
            var mahjongTileComponent = objectHit.GetComponent<MahjongTileComponent>();

            if (_activeTile == mahjongTileComponent) return;

            _activeTile?.HandleMouseOut();

            if (!mahjongTileComponent)
                return;

            mahjongTileComponent.HandleInput();
            _activeTile = mahjongTileComponent;
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