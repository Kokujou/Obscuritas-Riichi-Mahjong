using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Assets.Scripts.Core.Extensions;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObscuritasRiichiMahjong.Components.Interface
{
    public abstract class MahjongPlayerComponentBase : MonoBehaviour
    {
        private MahjongTileComponent _lastDiscardedTile;
        public MahjongTileComponent LastDiscardedTile
        {
            get => _lastDiscardedTile;
            set
            {
                Board.LastDiscardedTile = value.Tile;
                _lastDiscardedTile = value;
            }
        }

        public Transform HandParent;
        public Transform DiscardedTilesParent;
        public Transform ExposedTilesParent;
        public MeshRenderer ActiveTurnRenderer;

        public TextMesh SeatWindPanel;

        public MahjongBoard Board;
        public MahjongPlayer Player;

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
            LastDiscardedTile = tile;
            Player.Hand.Remove(tile.Tile);
            Player.DiscardedTiles.Add(tile.Tile);
        }

        public virtual IEnumerator DrawTile(float duration)
        {
            var firstFromBank = MahjongTileComponent.FromTile(Board.Wall.First());

            Board.Wall.RemoveAt(0);
            Player.Hand.Add(firstFromBank.Tile);

            yield return firstFromBank.SpawnAtParent(HandParent, duration);
            firstFromBank.transform.SetParent(HandParent, true);
            yield return null;
        }

        public abstract IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile);

        public abstract IEnumerator MakeTurn();

        public IEnumerator Pon(MahjongTileComponent lastDiscard)
        {
            yield return lastDiscard.CollectDiscard(HandParent, 1f);
            TakeDiscard(lastDiscard);

            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == lastDiscard.Tile).Take(3).ToList();

            if (exposedTiles.Count < 3) throw new Exception("Pon was called while not having 3 tiles of the same type.");

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 3);

            yield return new WaitForSeconds(1);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator Chi(MahjongTileComponent lastDiscard,
            Tuple<MahjongTileComponent, MahjongTileComponent, MahjongTileComponent> selectedTriplet = null)
        {
            IEnumerable<MahjongTileComponent> exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var chis = exposedTiles.GetChisWithTile(lastDiscard).ToList();

            if (chis.Count == 0) throw new Exception("You called chi while not having 3 consecutive tiles of the same type.");
            if (chis.Count > 1 && selectedTriplet is null) throw new Exception("You have more than one triplet of consecutive tiles, " +
                "so you need to specify an identifier");
            if (selectedTriplet is not null) exposedTiles = selectedTriplet.ToList<MahjongTileComponent>();
            else exposedTiles = chis[0];

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 3);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator OpenKan(MahjongTileComponent lastDiscard)
        {
            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == lastDiscard.Tile).ToList();

            if (exposedTiles.Count() != 4)
                throw new ArgumentException("Kan was called while not having 4 times the same tile.");

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 3);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator HiddenKan(MahjongTileComponent compareTile)
        {
            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == compareTile.Tile).ToList();

            if (exposedTiles.Count() != 4)
                throw new ArgumentException("Kan was called while not having 4 times the same tile.");

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 3);

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

        private void ExposeTiles(IEnumerable<MahjongTileComponent> tiles)
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