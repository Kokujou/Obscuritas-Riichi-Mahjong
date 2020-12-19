using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class TerminalOrHonorInEachSet : MahjongRule
    {
        public override int OpenHandPunishment => 1;
        public override int Han => 2;
        public override string Name => "Terminal or Honor in each set";
        public override string JapName => "Chanta";
        public override string KanjiName => "チャンタ";

        public override string Description =>
            "Each Set contains at least one terminal or one honor.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var sequences = player.Hand.GetSequences();

            if (sequences.Any(sequence => sequence.All(x => !x.IsTerminal())))
                return false;

            var groupedHand = player.Hand.GroupBy(x => x.Type).ToList();
            for (var i = 2; i < 9; i++)
                if (groupedHand.Any(group => group.Count(x => x.Number == i) >= 2))
                    return false;

            return true;
        }
    }
}