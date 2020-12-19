using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class MixedTripleSequence : MahjongRule
    {
        public override int OpenHandPunishment => 1;
        public override int Han => 2;
        public override string Name => "Mixed Triple Sequence";
        public override string JapName => "Sanshoku Doujun";
        public override string KanjiName => "– 三色同順";
        public override string Description => "Three sequences of all different suits";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var sequences = player.Hand.GetSequences();
            var sequenceNumbers = sequences.Select(x =>
                x.Aggregate($"{x.First().Type.ToString()[0]}",
                    (numbers, tile) => numbers + tile.Number)).ToList();

            if (sequenceNumbers.Any(sequence => sequenceNumbers.Count(x =>
                x[0] != sequence[0] && x.Substring(1) == sequence.Substring(1)) == 2))
                return true;

            return false;
        }
    }
}