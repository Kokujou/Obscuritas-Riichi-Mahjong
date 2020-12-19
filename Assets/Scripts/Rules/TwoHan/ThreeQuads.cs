using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class ThreeQuads : MahjongRule
    {
        public override int Han => 2;
        public override string Name => "Three Quads";
        public override string JapName => "San Kantsu";
        public override string KanjiName => "三槓子";
        public override string Description => "Three quads of any color.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            for (var i = 1; i <= 9; i++)
                if (player.Hand.Count(x => x.Number == i) >= 12)
                    return true;

            return false;
        }
    }
}