using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class AllTerminalsAndHonors : MahjongRule
    {
        public override int Han => 2;
        public override string Name => "All Terminals";
        public override string JapName => "Honrou";
        public override string KanjiName => "混老";

        public override string Description =>
            "Each Set of tiles must contain at least one terminal.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            if (player.Hand.Exists(x => !x.IsTerminal()))
                return false;

            return true;
        }
    }
}