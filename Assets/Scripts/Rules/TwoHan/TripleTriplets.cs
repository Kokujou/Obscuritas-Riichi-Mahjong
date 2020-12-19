using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class TripleTriplets : MahjongRule
    {
        public override int Han => 2;
        public override int OpenHandPunishment => 1;
        public override string Name => "Three Triplets";
        public override string JapName => "Sanshoku";
        public override string KanjiName => "三色";

        public override string Description =>
            "Three Triplets of the same number in all three suits.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            for (var i = 1; i <= 9; i++)
                if (player.Hand.Count(x => x.Number == i) == 9)
                    return true;

            return false;
        }
    }
}