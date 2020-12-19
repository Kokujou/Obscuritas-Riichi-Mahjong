using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class AllTriplets : MahjongRule
    {
        public override int Han => 2;
        public override string Name => "All Triplets";
        public override string JapName => "Toi Toi";
        public override string KanjiName => "対々";
        public override string Description => "The hand consists only of triplets or quads";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            for (var i = 1; i <= 9; i++)
            {
                var numberCount = player.Hand.Count(x => x.Number == i);
                if (numberCount % 3 != 0 && numberCount % 4 != 0)
                    return false;
            }

            return 2;
        }
    }
}