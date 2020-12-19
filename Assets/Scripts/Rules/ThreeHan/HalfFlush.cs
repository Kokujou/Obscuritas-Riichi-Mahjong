using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.ThreeHan
{
    public class HalfFlush : MahjongRule
    {
        public override int Han => 3;
        public override int OpenHandPunishment => 1;
        public override string Name => "Half Flush";
        public override string JapName => "Hon'itsu";
        public override string KanjiName => "混一 ";
        public override string Description => "";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            if (player.Hand
                .Where(x => x.Type != MahjongTileType.Wind && x.Type != MahjongTileType.Dragon)
                .GroupBy(x => x.Type)
                .Count() <= 1)
                return true;

            return false;
        }
    }
}