using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class NoPointsHand : MahjongRule
    {
        public override string Name => "No-points hand";
        public override string JapName => "Pinfu";
        public override string KanjiName => "平和";

        public override string Description =>
            "A hand consisting of only sequences, with the last tile completing one of those sequences.";

        public override bool AcceptOpenHand => false;

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var sequences = handSplit.GetSequences();
            if (sequences.Count == 4 && TileCompletesSequence(board.WinningTile, sequences))
                return true;

            return false;
        }

        public static bool TileCompletesSequence(MahjongTile tile,
            List<List<MahjongTile>> sequences)
        {
            foreach (var sequence in sequences)
                if (tile == sequence.Last() || tile == sequence.First())
                    return true;

            return false;
        }
    }
}