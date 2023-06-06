using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class ThirteenOrphans : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override int Yakuman => 1;
        public override string Name => "Thirteen Orphans";
        public override string JapName => "Kokushi Musou";
        public override string KanjiName => "国士無双";

        public override string Description =>
            "All non-triplet terminals plus a matching pair of terminals with the winning tile being drawn from the orphans.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return Fulfilled(player.Hand);
        }

        public static bool Fulfilled(List<MahjongTile> hand)
        {
            if (!hand.Any(X => X.Number == 1 && X.Type == MahjongTileType.Circle)) return false;
            if (!hand.Any(X => X.Number == 9 && X.Type == MahjongTileType.Circle)) return false;
            if (!hand.Any(X => X.Number == 1 && X.Type == MahjongTileType.Bamboo)) return false;
            if (!hand.Any(X => X.Number == 9 && X.Type == MahjongTileType.Bamboo)) return false;
            if (!hand.Any(X => X.Number == 1 && X.Type == MahjongTileType.Kanji)) return false;
            if (!hand.Any(X => X.Number == 9 && X.Type == MahjongTileType.Kanji)) return false;
            if (!hand.Any(X => X.Name == "West")) return false;
            if (!hand.Any(X => X.Name == "South")) return false;
            if (!hand.Any(X => X.Name == "North")) return false;
            if (!hand.Any(X => X.Name == "East")) return false;
            if (!hand.Any(X => X.Name == "White Dragon")) return false;
            if (!hand.Any(X => X.Name == "Red Dragon")) return false;
            if (!hand.Any(X => X.Name == "Green Dragon")) return false;
            if (!hand.GroupBy(x => x.Name).Any(x => x.Count() == 2)) return false;

            return true;
        }
    }
}