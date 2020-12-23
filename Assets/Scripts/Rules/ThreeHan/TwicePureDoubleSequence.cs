using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.ThreeHan
{
    public class TwicePureDoubleSequence : MahjongRule
    {
        public override int Han { get; set; } = 3;
        public override bool AcceptOpenHand => false;
        public override string Name => "Twice Pure Double Sequence";
        public override string JapName => "Ryanpeikou";
        public override string KanjiName => "二盃口";
        public override string Description => "Two independent pairs of identical sequences.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var sequenceTypes = handSplit.GetSequences().GroupBy(x => x.First().Type);
            var firstDoubleSequence = "";
            foreach (var sequenceType in sequenceTypes)
            {
                var numberSequences =
                    sequenceType.Select(x => string.Join("", x.Select(y => y.Number)))
                        .GroupBy(x => x);
                var doubleSequence = numberSequences.FirstOrDefault(x => x.Count() == 2)?.Key;
                if (firstDoubleSequence == "" && doubleSequence != null)
                    firstDoubleSequence = doubleSequence;
                else if (firstDoubleSequence != "" && doubleSequence == firstDoubleSequence)
                    return true;
            }

            return false;
        }
    }
}