using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Assets.Scripts.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Core.Extensions;
using ObscuritasRiichiMahjong.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObscuritasRiichiMahjong.Services
{
    public class GameInputLoopService
    {
        private readonly MahjongBoard _board;

        private readonly Dictionary<CardinalPoint, MahjongPlayerComponentBase> _initializedPlayerComponents = new();

        private MahjongPlayerComponentBase CurrentPlayer =>
            _initializedPlayerComponents[_board.CurrentRoundWind];

        public GameInputLoopService(MahjongBoard board, IEnumerable<MahjongPlayerComponentBase> playerComponents)
        {
            _board = board;

            var playerCardinal = (CardinalPoint)Random.Range(0, 4);
            foreach (var player in playerComponents)
            {
                player.Initialize(playerCardinal, _board);
                _board.Players.Add(player.Player.CardinalPoint, player.Player);
                _initializedPlayerComponents.Add(player.Player.CardinalPoint, player);
                playerCardinal = playerCardinal.Next();
            }
        }

        public IEnumerator PlayerInputLoop()
        {
            MahjongMain.CanHover = true;
            _board.CurrentRoundWind = CardinalPoint.East;

            while (_board.CurrentRound <= _board.MaxRounds)
            {
                _board.CurrentRound++;
                _board.LastDiscardedTile = null;

                var lastDrawn = MahjongTileComponent.FromTile(_board.Wall.First());
                yield return CurrentPlayer.DrawTile(lastDrawn, .5f);
                yield return new WaitForSeconds(.1f);

                var cancellation = new CancellationTokenSource();
                CurrentPlayer.StartCoroutine(CurrentPlayer.ActiveTurnRenderer.material.BlinkColor(1, cancellation.Token));
                CurrentPlayer.LastDiscardedTile = null;
                yield return CurrentPlayer.MakeTurn(lastDrawn);
                yield return new WaitForSeconds(.1f);

                if (_board.LastDiscardedTile != lastDrawn.Tile)
                    CurrentPlayer.StartCoroutine(lastDrawn.InsertTile(CurrentPlayer.HandParent, 1f));

                if (!_board.LastDiscardedTile || !CurrentPlayer.LastDiscardedTile)
                    throw new NotImplementedException(
                        "The MakeTurn method must set the boards LastDiscardedTile property.");

                foreach (var player in _initializedPlayerComponents.Values.Where(player => player != CurrentPlayer))
                {
                    player.LastCallType = CallType.Skip;
                    yield return player.ReactOnDiscard(CurrentPlayer.LastDiscardedTile);
                    if (player.LastCallType == CallType.Skip) continue;

                    _board.CurrentRoundWind = _initializedPlayerComponents.First(x => x.Value == player).Key;
                    break;
                }

                cancellation.Cancel();
                _board.CurrentRoundWind = _board.CurrentRoundWind.Next();
            }
        }
    }
}