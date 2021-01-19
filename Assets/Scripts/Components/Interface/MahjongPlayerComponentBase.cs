using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components.Interface
{
    public abstract class MahjongPlayerComponentBase : MonoBehaviour
    {
        public Transform HandParent;
        public Transform DiscardedTilesParent;
        public Transform ExposedTilesParent;

        public TextMesh SeatWindPanel;

        public MahjongBoard Board { get; set; }
        public MahjongPlayer Player { get; set; }

        public virtual void Initialize(CardinalPoint wind, MahjongBoard board)
        {
            Board = board;

            var cardinalPoint = wind;
            SeatWindPanel.text = $"{cardinalPoint.ToString().First()}";
            Player = new MahjongPlayer(cardinalPoint);
        }

        public virtual void ScanHand()
        {
            var hand = transform.GetComponentsInChildren<MahjongTileComponent>()
                .Select(x => x.GetComponent<MahjongTileComponent>().Tile);
            Player.Hand.AddRange(hand);
        }

        public virtual IEnumerator DiscardTile(MahjongTileComponent tile)
        {
            yield return tile.MoveToParent(DiscardedTilesParent, 1f);
            tile.transform.SetParent(DiscardedTilesParent, true);
        }

        public virtual IEnumerator DrawTile(float duration)
        {
            var firstFromBank = MahjongTileComponent.FromTile(Board.Wall.First());
            firstFromBank.transform.GetComponent<Rigidbody>().isKinematic = true;

            Player.Wall.Remove(firstFromBank.Tile);
            Player.Hand.Add(firstFromBank.Tile);

            yield return firstFromBank.SpawnAtParent(HandParent, duration);
            firstFromBank.transform.SetParent(HandParent, true);
        }

        public abstract IEnumerator MakeTurn();

        public abstract void Pon();
        public abstract void Chi();
        public abstract void OpenKan();
        public abstract void HiddenKan();
        public abstract void Ron();
        public abstract void Tsumo();
        public abstract void Riichi();
    }
}