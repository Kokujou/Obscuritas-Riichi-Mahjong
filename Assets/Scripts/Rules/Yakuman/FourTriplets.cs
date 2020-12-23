using System.Collections.Generic;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class FourTriplets : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override int Yakuman => 1;
        public override string Name => "Four Concealed Triplets";
        public override string JapName => "Suu Ankou";
        public override string KanjiName => "四暗刻";

        public override string Description =>
            "Any four closed triplets or quads where the winning tile completes a triplet.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (board.WinningMoveType == WinningMoveType.Ron)
                return false;

            var allClosedTriplets = handSplit.GetTripletsOrQuads();

            if (allClosedTriplets.Count >= 4)
                return true;

            return false;
        }
    }
}