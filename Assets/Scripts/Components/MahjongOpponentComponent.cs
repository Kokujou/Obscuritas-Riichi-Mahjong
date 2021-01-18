using System;
using System.Collections;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongOpponentComponent : MahjongPlayerComponentBase
    {
        public override IEnumerator MakeTurn()
        {
            DrawTile();
            yield return new WaitForSeconds(1);

            var hand = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var selectedTile = hand[Random.Range(0, hand.Length)];

            DiscardTile(selectedTile);
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