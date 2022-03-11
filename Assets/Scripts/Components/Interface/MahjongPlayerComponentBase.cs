using System;
using System.Collections;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components.Interface
{
    public abstract class MahjongPlayerComponentBase : MonoBehaviour
    {
        public MahjongTileComponent LastDiscardedTile;

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
            Board.LastDiscardedTile = tile.Tile;
            LastDiscardedTile = tile;
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

        public abstract IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile);

        public abstract IEnumerator MakeTurn();

        public IEnumerator Pon(MahjongTileComponent lastDiscard, Vector3 forceOffset)
        {
            yield return lastDiscard.DoPonAnimation(this, 1f, forceOffset);
        }

        public IEnumerator Chi()
        {
            throw new NotImplementedException();
        }

        public IEnumerator OpenKan()
        {
            throw new NotImplementedException();
        }

        public IEnumerator HiddenKan()
        {
            throw new NotImplementedException();
        }

        public IEnumerator Ron()
        {
            throw new NotImplementedException();
        }

        public IEnumerator Tsumo()
        {
            throw new NotImplementedException();
        }

        public IEnumerator Riichi()
        {
            throw new NotImplementedException();
        }
    }
}