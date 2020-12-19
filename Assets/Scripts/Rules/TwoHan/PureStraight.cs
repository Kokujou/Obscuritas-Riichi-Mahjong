using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class PureStraight : MahjongRule
    {
        public override int Han => 2;
        public override int OpenHandPunishment => 1;
        public override string Name => "Straight";
        public override string JapName => "Ittsuu";
        public override string KanjiName => "一通";
        public override string Description => "All numbers from 1-9 in the same suit.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var groupedHand = player.Hand.Where(x => !x.IsTerminal())
                .GroupBy(x => x.Type);

            var groupedTileNumbers = from grouping in groupedHand
                select grouping.OrderBy(x => x.Number).ToList()
                into groupContent
                where groupContent.Count >= 9
                select groupContent.Aggregate("", (current, tile) => current + tile.Number);
            if (groupedTileNumbers.Any(numbers => numbers.Contains("123456789")))
                return true;

            return false;
        }
    }
}