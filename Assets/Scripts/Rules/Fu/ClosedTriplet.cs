using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class ClosedTriplet : IMahjongFuRule
    {
        public int Fu { get; set; } = 4;
        public string Name => "Closed Triplet";
        public string JapName => "Ankou";
        public string KanjiName => "暗刻";
        public string Description => "A closed Triplet";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return handSplit.GetTriplets().Sum(x => x.First().IsTerminal() ? Fu * 2 : Fu);
        }
    }
}