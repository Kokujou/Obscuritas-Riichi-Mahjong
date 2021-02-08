using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Animations;
using ObscuritasRiichiMahjong.Components;
using ObscuritasRiichiMahjong.Components.Interface;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Core.Extensions;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ObscuritasRiichiMahjong.Services
{
    public class GameInputLoopService
    {
        private readonly MahjongBoard _board;

        private readonly Dictionary<CardinalPoint, MahjongPlayerComponentBase> _initializedPlayerComponents =
            new Dictionary<CardinalPoint, MahjongPlayerComponentBase>();

        private MahjongPlayerComponentBase CurrentPlayer =>
            _initializedPlayerComponents[_board.CurrentRoundWind];

        public GameInputLoopService(MahjongBoard board, IEnumerable<MahjongPlayerComponentBase> playerComponents)
        {
            _board = board;

            var playerCardinal = (CardinalPoint) Random.Range(0, 4);
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
            _board.CurrentRoundWind = CardinalPoint.East;

            while (_board.CurrentRound <= _board.MaxRounds)
            {
                _board.CurrentRound++;
                var currentPlayer = CurrentPlayer;
                _board.LastDiscardedTile = null;

                yield return currentPlayer.DrawTile(.5f);
                yield return new WaitForSeconds(.1f);

                yield return currentPlayer.MakeTurn();
                yield return new WaitForSeconds(.1f);

                var drawnTile = currentPlayer.HandParent.GetComponentsInChildren<MahjongTileComponent>().Last();
                drawnTile.StartCoroutine(drawnTile.InsertTile(currentPlayer.HandParent, 1f));

                if (!_board.LastDiscardedTile)
                    throw new NotImplementedException(
                        "The MakeTurn method must set the boards LastDiscardedTile property.");

                foreach (var player in _initializedPlayerComponents.Values.Where(player => player != currentPlayer))
                    yield return player.ReactOnDiscard(_board.LastDiscardedTile);

                _board.CurrentRoundWind = _board.CurrentRoundWind.Next();
            }
        }
    }
}