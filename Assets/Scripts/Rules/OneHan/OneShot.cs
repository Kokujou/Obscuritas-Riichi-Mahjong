using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class OneShot : MahjongRule
    {
        public override string Name => "One Shot";
        public override string JapName => "OneShot";
        public override string KanjiName => "一発";

        public override string Description =>
            "Complete the Hand the first round after declaring ReadyHand.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            if (player.Ippatsu)
                return true;

            return false;
        }
    }
}