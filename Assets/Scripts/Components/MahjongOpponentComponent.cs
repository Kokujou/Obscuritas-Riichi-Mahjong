using System.Collections;
using ObscuritasRiichiMahjong.Components.Interface;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components
{
    public class MahjongOpponentComponent : MahjongPlayerComponentBase
    {
        public override IEnumerator MakeTurn()
        {
            var hand = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var selectedTile = hand[Random.Range(0, hand.Length)];

            yield return DiscardTile(selectedTile);
        }

        public override IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile)
        {
            return null;
        }
    }
}