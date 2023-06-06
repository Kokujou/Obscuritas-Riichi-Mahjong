using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class FourTripletsSingleWait : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override int Yakuman => 2;
        public override string Name => "Four Triplets (Single Wait)";
        public override string JapName => "Suu Ankou Tanki";
        public override string KanjiName => "四暗刻単騎";

        public override string Description =>
            "Any four closed triplets or quads with the winning tile completes the 2-tile set";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allClosedTriplets = handSplit.Concat(player.HiddenKan).GetTripletsOrQuads();

            if (allClosedTriplets.Count < 4)
                return false;

            var pair = handSplit.First(x => x.Count == 2);
            if (board.WinningTile == pair.First())
                return true;

            return false;
        }
    }
}