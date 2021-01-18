using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class AllTerminalsAndHonors : MahjongRule
    {
        public override int Han { get; set; } = 2;
        public override string Name => "All Terminals";
        public override string JapName => "Honrou";
        public override string KanjiName => "混老";

        public override string Description =>
            "Each Set of tiles must contain at least one terminal.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTiles = handSplit.EnrichSplittedHand(player).SelectMany(x => x);
            if (allTiles.Any(x => !x.IsTerminal()))
                return false;

            return true;
        }
    }
}