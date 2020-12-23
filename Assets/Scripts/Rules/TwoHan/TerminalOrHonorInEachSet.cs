using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class TerminalOrHonorInEachSet : MahjongRule
    {
        public override int OpenHandPunishment => 1;
        public override int Han { get; set; } = 2;
        public override string Name => "Terminal or Honor in each set";
        public override string JapName => "Chanta";
        public override string KanjiName => "チャンタ";

        public override string Description =>
            "Each Set contains at least one terminal or one honor.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (handSplit.All(group => group.Any(x => x.IsTerminal())))
                return true;

            return false;
        }
    }
}