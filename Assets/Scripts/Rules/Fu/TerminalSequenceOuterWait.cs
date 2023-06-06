using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class TerminalSequenceOuterWait : IMahjongFuRule
    {
        public static bool IsInnerTileOfTerminalSequence(MahjongTile tile,
            List<MahjongTile> sequence)
        {
            return sequence.Exists(x =>
                x.Number == 9 && tile == sequence[0] || (x.Number == 1) & (tile == sequence[2]));
        }

        public int Fu { get; set; } = 2;
        public string Name => "Open Triplet";
        public string JapName => "OpenTriplet";
        public string KanjiName => "明刻";

        public string Description =>
            "Waiting for the inner tile of a terminal sequence (3 for 1-2, 7 for 8-9)";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var winningSequence = handSplit.EnrichSplittedHand(player).GetSequences()
                .FirstOrDefault(x => IsInnerTileOfTerminalSequence(board.WinningTile, x));

            if (winningSequence != default)
                return Fu;

            return 0;
        }
    }
}