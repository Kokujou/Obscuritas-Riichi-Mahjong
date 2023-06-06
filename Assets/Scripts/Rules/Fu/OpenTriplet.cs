using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class OpenTriplet : IMahjongFuRule
    {
        public int Fu { get; set; } = 2;
        public string Name => "Open Triplet";
        public string JapName => "Minkou";
        public string KanjiName => "明刻";
        public string Description => "An open triplet";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return player.ExposedHand.GetTriplets()
                .Sum(x => x.First().IsTerminal() ? 2 * Fu : Fu);
        }
    }
}