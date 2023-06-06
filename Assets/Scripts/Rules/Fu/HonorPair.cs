using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class HonorPair : IMahjongFuRule
    {
        public int Fu { get; set; } = 2;
        public string Name => "Honor Pair";
        public string JapName => "Toitsu";
        public string KanjiName => "対子";

        public string Description =>
            "Points awarded, if the pair is a honor pair of valid winds or dragons.";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var pair = handSplit.First(x => x.Count == 2);
            var firstTile = pair.First();
            if (firstTile.Type == MahjongTileType.Dragon)
                return Fu;

            if (firstTile.Type != MahjongTileType.Wind)
                return 0;

            var fu = 0;
            if (firstTile.Name == board.CardinalPoint.ToString())
                fu += Fu;
            if (firstTile.Name == player.CardinalPoint.ToString())
                fu += Fu;

            return fu;
        }
    }
}