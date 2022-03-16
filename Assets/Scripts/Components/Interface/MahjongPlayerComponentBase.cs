using System;
using System.Collections;
using System.Collections.Generic;
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

            Board.Wall.RemoveAt(0);
            Player.Hand.Add(firstFromBank.Tile);

            yield return firstFromBank.SpawnAtParent(HandParent, duration);
            firstFromBank.transform.SetParent(HandParent, true);
        }

        public abstract IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile);

        public abstract IEnumerator MakeTurn();

        public IEnumerator Pon(MahjongTileComponent lastDiscard)
        {
            yield return lastDiscard.InterpolationAnimation(1,
                HandParent.localPosition + Vector3.forward * 2 + Vector3.left,
                Vector3.right * 90);
            TakeDiscard(lastDiscard);

            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == lastDiscard.Tile).Take(3).ToList();


            foreach (var tile in exposedTiles)
                StartCoroutine(tile.ExposeAndThrowTile(3));

            yield return new WaitForSeconds(3);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator Chi(MahjongTileComponent lastDiscard)
        {
            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == lastDiscard.Tile).Take(3).ToList();

            foreach (var tile in exposedTiles)
                StartCoroutine(tile.ExposeAndThrowTile(3));

            yield return new WaitForSeconds(3);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator OpenKan(MahjongTileComponent lastDiscard)
        {
            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == lastDiscard.Tile).ToList();

            if (exposedTiles.Count() != 4)
                throw new ArgumentException("Kan was called but the found count of matching tiles did not equal 4.");

            foreach (var tile in exposedTiles)
                StartCoroutine(tile.ExposeAndThrowTile(3));

            yield return new WaitForSeconds(3);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator HiddenKan(MahjongTileComponent compareTile)
        {
            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == compareTile.Tile).ToList();

            if (exposedTiles.Count() != 4)
                throw new ArgumentException("Kan was called but the found count of matching tiles did not equal 4.");

            foreach (var tile in exposedTiles)
                StartCoroutine(tile.ExposeAndThrowTile(3));

            yield return new WaitForSeconds(3);

            ExposeTiles(exposedTiles);
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

        private void TakeDiscard(MahjongTileComponent discard)
        {
            var lastPlayer = Board.Players[Board.CurrentRoundWind];
            discard.transform.SetParent(HandParent, true);
            lastPlayer.Hand.Remove(discard.Tile);
            Player.Hand.Add(discard.Tile);
        }

        private void ExposeTiles(List<MahjongTileComponent> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.transform.SetParent(ExposedTilesParent, true);
                Player.Hand.Remove(tile.Tile);
            }

            Player.ExposedHand.Add(tiles.Select(x => x.Tile).ToList());
        }
    }
}