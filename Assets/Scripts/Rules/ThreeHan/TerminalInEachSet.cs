using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.ThreeHan
{
    public class TerminalInEachSet : MahjongRule
    {
        public override int Han => 3;
        public override int OpenHandPunishment => 1;
        public override string Name => "Fully Outside Hand";
        public override string JapName => "Junchan";
        public override string KanjiName => "純チャン";
        public override string Description => "Every set of the hand contains at least one 1 or 9.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var sequences = player.Hand.GetSequences();

            if (sequences.Any(group => group.All(tile => tile.Number != 9 && tile.Number != 1)))
                return false;

            var nonTerminalSuits =
                new[] {MahjongTileType.Bamboo, MahjongTileType.Circle, MahjongTileType.Kanji};
            for (var i = 2; i < 9; i++)
                foreach (var suit in nonTerminalSuits)
                    if (player.Hand.Count(x => x.Number == i && x.Type == suit) >= 2)
                        return false;

            return true;
        }
    }
}