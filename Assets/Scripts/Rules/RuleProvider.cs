using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules
{
    public static class RuleProvider
    {
        public static PointResult CalculatePoints(List<List<MahjongTile>> handSplit,
            MahjongPlayer player,
            MahjongBoard board)
        {
            var rules = Assembly.GetAssembly(typeof(MahjongRule))
                .GetTypes()
                .Where(myType =>
                    myType.IsClass && !myType.IsAbstract &&
                    myType.IsSubclassOf(typeof(MahjongRule)))
                .Select(type => (MahjongRule) Activator.CreateInstance(type))
                .ToList();

            var pointResult = new PointResult();
            foreach (var yaku in rules)
            {
                var han = yaku.GetHan(handSplit, board, player);
                yaku.Han = han;

                if (han <= 0) continue;

                pointResult.CollectedYaku.Add(yaku);

                if (yaku.Yakuman > 0)
                    pointResult.Yakuman += yaku.Yakuman;
                else
                    pointResult.Han += han;
            }

            if (board.WinningMoveType == WinningMoveType.Tsumo)
                pointResult.FromAll = true;

            return pointResult;
        }
    }
}