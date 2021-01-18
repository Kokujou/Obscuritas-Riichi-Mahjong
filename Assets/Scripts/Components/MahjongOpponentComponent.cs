using System;
using System.Threading.Tasks;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongOpponentComponent : MahjongPlayerComponentBase
    {
        public override async Task<MahjongTileComponent> MakeTurn()
        {
            Debug.Log("opponent turn");
            await Task.Yield();

            DrawTile();

            var hand = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var selectedTile = hand[Random.Range(0, hand.Length)];

            DiscardTile(selectedTile);

            return selectedTile;
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