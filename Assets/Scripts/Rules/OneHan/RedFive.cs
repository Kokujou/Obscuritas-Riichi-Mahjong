using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class RedFive : MahjongRule
    {
        public override string Name => "Red Five";
        public override string JapName { get; }
        public override string KanjiName { get; }
        public override string Description => "The winning hand contains one or more red fives.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            return GetHan(board, player) > 0;
        }

        public override int GetHan(MahjongBoard board, MahjongPlayer player)
        {
            return player.Hand.Count(x => x.Red);
        }
    }
}