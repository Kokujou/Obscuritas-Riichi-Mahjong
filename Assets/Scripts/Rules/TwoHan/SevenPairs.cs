using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class SevenPairs : MahjongRule
    {
        public override int Han => 2;
        public override bool AcceptOpenHand => false;
        public override string Name => "Seven Pairs";
        public override string JapName => "Chiitoitsu";
        public override string KanjiName => "七対子";
        public override string Description => "A hand consisting of only pairs (2x) of any tile.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var groupedHand = player.Hand.GroupBy(x => x.name);

            if (groupedHand.Any(x => x.Count() != 2))
                return false;

            return true;
        }
    }
}