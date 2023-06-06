using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class SequenceMiddleWait : IMahjongFuRule
    {
        public int Fu { get; set; } = 2;
        public string Name => "Sequence Middle Wait";
        public string JapName => "Kanchan-Machi";
        public string KanjiName => "嵌張待ち";
        public string Description => "Waiting for the middle tile of a sequence";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var winningSequence = handSplit.EnrichSplittedHand(player).GetSequences()
                .FirstOrDefault(x => x[1] == board.WinningTile);

            if (winningSequence != default)
                return Fu;

            return 0;
        }
    }
}