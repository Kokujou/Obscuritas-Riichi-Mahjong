using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.ThreeHan
{
    public class TwicePureDoubleSequence : MahjongRule
    {
        public override int Han => 3;
        public override bool AcceptOpenHand => false;
        public override string Name => "Twice Pure Double Sequence";
        public override string JapName => "Ryanpeikou";
        public override string KanjiName => "二盃口";
        public override string Description => "Two independent pairs of identical sequences.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var numberSequences = player.Hand.GetSequences()
                .Select(x => x.Aggregate($"{x.First().Type.ToString()[0]}",
                    (text, tile) => text + tile.Number)).ToList();

            if (numberSequences.Count(
                sequence => numberSequences.Count(x => x == sequence) == 2) == 2)
                return true;

            return false;
        }
    }
}