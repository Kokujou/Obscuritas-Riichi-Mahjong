using System;
using System.Linq;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using UnityEngine;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class WinningTypeSetterComponent : MonoBehaviour
    {
        public SelectableButton ChankanButton;
        public SelectableButton RinshanKaihouButton;
        public SelectableButton HaiteiHouteiButton;

        public PointCalculator PointCalculator;

        public void Start()
        {
            ChankanButton.IsSelected = IsChankan;
            RinshanKaihouButton.IsSelected = IsRinshanKaihou;
            HaiteiHouteiButton.IsSelected = IsHaiteiOrHoutei;
        }

        private bool IsChankan()
        {
            return PointCalculator.Board.LastMoveType == MoveType.OpenKan &&
                   PointCalculator.Board.WinningMoveType == WinningMoveType.Ron;
        }

        private bool IsRinshanKaihou()
        {
            return (PointCalculator.Player.LastMoveType == MoveType.HiddenKan ||
                    PointCalculator.Player.LastMoveType == MoveType.OpenKan)
                   && PointCalculator.Board.WinningMoveType == WinningMoveType.Tsumo;
        }

        private bool IsHaiteiOrHoutei()
        {
            return PointCalculator.Board.Wall.Count == 0 &&
                   PointCalculator.Board.CurrentRound > 1 &&
                   PointCalculator.Player.LastMoveType != MoveType.HiddenKan &&
                   PointCalculator.Player.LastMoveType != MoveType.OpenKan;
        }

        public void SetChankan()
        {
            if (IsChankan())
            {
                PointCalculator.Board.LastMoveType = MoveType.Normal;
            }
            else
            {
                PointCalculator.Board.LastMoveType = MoveType.OpenKan;
                PointCalculator.Board.WinningMoveType = WinningMoveType.Ron;
            }
        }

        public void SetRinshanKaihou()
        {
            if (PointCalculator.Player.ExposedHand.All(x => x.Count != 4) &&
                PointCalculator.Player.HiddenKan.Count == 0)
                throw new InvalidOperationException(
                    "To call rinshan kaihou, the player needs at least one quad in his/her hand.");

            if (IsRinshanKaihou())
            {
                PointCalculator.Player.LastMoveType = MoveType.Normal;
            }
            else
            {
                PointCalculator.Board.Wall.Add(new MahjongTile());
                PointCalculator.Board.WinningMoveType = WinningMoveType.Tsumo;
                PointCalculator.Player.LastMoveType = PointCalculator.Player.HiddenKan.Count > 0
                    ? MoveType.HiddenKan
                    : MoveType.OpenKan;
            }
        }

        public void SetHaiteiHoutei()
        {
            if (IsHaiteiOrHoutei())
            {
                PointCalculator.Board.Wall.Add(new MahjongTile());
            }
            else
            {
                if (PointCalculator.Player.LastMoveType == MoveType.HiddenKan ||
                    PointCalculator.Player.LastMoveType == MoveType.OpenKan)
                    PointCalculator.Player.LastMoveType = MoveType.Normal;
                PointCalculator.Board.CurrentRound = 2;
                PointCalculator.Board.Wall.Clear();
            }
        }
    }
}