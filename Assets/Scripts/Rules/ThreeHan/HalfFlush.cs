using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.ThreeHan
{
    public class HalfFlush : MahjongRule
    {
        public override int Han { get; set; } = 3;
        public override int OpenHandPunishment => 1;
        public override string Name => "Half Flush";
        public override string JapName => "Hon'itsu";
        public override string KanjiName => "混一 ";

        public override string Description =>
            "A hand consisting of only honor tiles and one more suit.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTiles = handSplit.EnrichSplittedHand(player).SelectMany(x => x).ToList();

            if (!allTiles.Any(x =>
                x.Type == MahjongTileType.Wind || x.Type == MahjongTileType.Dragon))
                return false;

            if (allTiles
                .Where(x => x.Type != MahjongTileType.Wind && x.Type != MahjongTileType.Dragon)
                .GroupBy(x => x.Type)
                .Count() <= 1)
                return true;

            return false;
        }
    }
}