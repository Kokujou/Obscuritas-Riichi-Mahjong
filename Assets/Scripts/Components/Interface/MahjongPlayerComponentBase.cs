using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Assets.Scripts.Animations;
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
                Board.LastDiscardedTile = value?.Tile;
                MahjongMain.LastDiscard = value;
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

        public CallType LastCallType = CallType.Skip;

        public bool RiichiMode = false;

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
            yield return tile.transform.MoveToParent(DiscardedTilesParent, 1f);
            tile.transform.SetParent(DiscardedTilesParent, true);
            LastDiscardedTile = tile;
            Player.Hand.Remove(tile.Tile);
            Player.DiscardedTiles.Add(tile.Tile);
        }

        public virtual IEnumerator DrawTile(MahjongTileComponent tile, float duration)
        {
            Board.Wall.RemoveAt(0);
            Player.Hand.Add(tile.Tile);

            yield return tile.SpawnAtParent(HandParent, duration);
            tile.transform.SetParent(HandParent, true);
            yield return null;
        }

        public abstract IEnumerator ReactOnDiscard(MahjongTileComponent lastDiscardedTile);

        public abstract IEnumerator MakeTurn(MahjongTileComponent lastDrawn);

        public IEnumerator Tsumo()
        {
            Board.WinningMoveType = WinningMoveType.Tsumo;
            yield return EndGameWithTile(HandParent.GetComponentsInChildren<MahjongTileComponent>().Last());
        }

        public IEnumerator Ron()
        {
            TakeDiscard(MahjongMain.LastDiscard);
            Board.WinningMoveType = WinningMoveType.Ron;
            yield return EndGameWithTile(MahjongMain.LastDiscard);
        }

        private IEnumerator EndGameWithTile(MahjongTileComponent tile)
        {
            MahjongMain.CanHover = false;
            yield return tile.ThrowLastTile(HandParent, 1f);
            yield return HandParent.ExposeHand(.5f);
            Board.WinningTile = tile.Tile;
            Board.Winner = Player;

            FindAnyObjectByType<GameEndComponent>().Initialize(Board);
        }

        protected IEnumerator Riichi(MahjongTileComponent nonTenpaiDiscard)
        {
            yield return this.DoRiichiAnimation(nonTenpaiDiscard, 1);
            nonTenpaiDiscard.transform.SetParent(DiscardedTilesParent, true);
            LastDiscardedTile = nonTenpaiDiscard;
            Player.Hand.Remove(nonTenpaiDiscard.Tile);
            Player.DiscardedTiles.Add(nonTenpaiDiscard.Tile);
            RiichiMode = true;
        }

        public IEnumerator Pon()
        {
            yield return MahjongMain.LastDiscard.CollectDiscard(HandParent, 1f);
            TakeDiscard(MahjongMain.LastDiscard);

            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == Board.LastDiscardedTile).Take(3).ToList();

            if (exposedTiles.Count < 3) throw new Exception("Pon was called while not having 3 tiles of the same type.");

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 1.5f);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator Chi(ValueTuple<MahjongTileComponent, MahjongTileComponent> selectedTriplet = default)
        {
            yield return MahjongMain.LastDiscard.CollectDiscard(HandParent, 1f);
            TakeDiscard(MahjongMain.LastDiscard);

            IEnumerable<MahjongTileComponent> handTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>();
            var chis = handTiles.GetChisWithTile(MahjongMain.LastDiscard).ToList();

            if (chis.Count == 0) throw new Exception("You called chi while not having 3 consecutive tiles of the same type.");
            if (chis.Count > 1 && selectedTriplet is (null, null))
                throw new Exception("You have more than one triplet of consecutive tiles, so you need to specify an identifier");

            var exposedTiles = Enumerable.Empty<MahjongTileComponent>();
            chis[0].Remove(Board.LastDiscardedTile);
            if (selectedTriplet is not (null, null)) exposedTiles = selectedTriplet.ToList<MahjongTileComponent>();
            else exposedTiles = chis[0].Select(tile => handTiles.FirstOrDefault(component => tile == component.Tile));
            exposedTiles = exposedTiles.Append(MahjongMain.LastDiscard);

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 1.5f);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator OpenKan()
        {
            yield return MahjongMain.LastDiscard.CollectDiscard(HandParent, 1f);
            TakeDiscard(MahjongMain.LastDiscard);

            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == Board.LastDiscardedTile).ToList();

            if (exposedTiles.Count() != 4)
                throw new ArgumentException("Kan was called while not having 4 times the same tile.");

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 1.5f);

            ExposeTiles(exposedTiles);
        }

        public IEnumerator HiddenKan()
        {
            var exposedTiles = HandParent.GetComponentsInChildren<MahjongTileComponent>()
                .Where(x => x.Tile == Board.LastDiscardedTile).ToList();

            if (exposedTiles.Count() != 4)
                throw new ArgumentException("Kan was called while not having 4 times the same tile.");

            yield return exposedTiles.ExposeAndThrowTiles(ExposedTilesParent, 1.5f);

            ExposeTiles(exposedTiles);
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