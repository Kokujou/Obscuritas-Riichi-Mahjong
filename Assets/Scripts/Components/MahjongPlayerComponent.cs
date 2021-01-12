using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongPlayerComponent : MahjongPlayerComponentBase
    {
        private MahjongTile _lastTile;

        public void DiscardTile(MahjongTileComponent tile)
        {
            _lastTile = tile.Tile;
        }

        public override void Initialize(ref List<CardinalPoint> availableWinds)
        {
            base.Initialize(ref availableWinds);
            transform.Rotate(45, 0, 0);

            foreach (Transform child in transform)
            {
                var mahjongTile = child.GetComponent<MahjongTileComponent>();

                if (!mahjongTile) continue;

                if (mahjongTile)
                    mahjongTile.Selectable = true;
            }

            StartCoroutine(transform.SortHand());
        }

        public override async Task<MahjongTile> MakeTurn()
        {
            while (!_lastTile)
                await Task.Yield();

            var tile = _lastTile;
            _lastTile = null;

            return tile;
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