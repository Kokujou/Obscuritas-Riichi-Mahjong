using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ObscuritasRiichiMahjong.Rules
{
    public static class RuleProvider
    {
        public static PointResult CalculatePoints(this List<List<MahjongTile>> handSplit,
            MahjongPlayer player,
            MahjongBoard board)
        {
            var rules = Assembly.GetAssembly(typeof(MahjongRule))
                .GetTypes()
                .Where(myType =>
                    myType.IsClass && !myType.IsAbstract &&
                    myType.IsSubclassOf(typeof(MahjongRule)))
                .Select(type => (MahjongRule)Activator.CreateInstance(type))
                .ToList();

            var fuRules = Assembly.GetAssembly(typeof(IMahjongFuRule))
                .GetTypes()
                .Where(myType =>
                    myType.IsClass && !myType.IsAbstract &&
                    myType.IsSubclassOf(typeof(IMahjongFuRule)))
                .Select(type => (IMahjongFuRule)Activator.CreateInstance(type))
                .ToList();

            var fulfilledYaku = rules.Where(x => x.Fulfilled(handSplit, board, player)).ToList();
            var fu = fuRules.Sum(x => x.GetFu(handSplit, board, player));
            var han = fulfilledYaku.Sum(x => x.GetHan(handSplit, board, player));
            var pointResult = new PointResult(fulfilledYaku.ToList(),
                fu, han, board.WinningMoveType == WinningMoveType.Tsumo);

            return pointResult;
        }
    }
}