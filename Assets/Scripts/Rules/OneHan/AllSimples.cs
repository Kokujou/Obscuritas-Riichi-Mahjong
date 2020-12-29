using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class AllSimples : MahjongRule
    {
        public override string Name => "All Simples";
        public override string JapName => "Tan'yao";
        public override string KanjiName => "断么";
        public override string Description => "A hand composed only of numbered tiles from 2-8.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTiles = handSplit.EnrichSplittedHand(player).SelectMany(x => x);
            if (allTiles.Any(x => x.IsTerminal()))
                return false;

            return true;
        }
    }
}