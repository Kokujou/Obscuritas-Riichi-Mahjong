using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class LittleThreeDragons : MahjongRule
    {
        public override int Han => 2;
        public override string Name => "Little Three Dragons";
        public override string JapName => "Shousangen";
        public override string KanjiName => "小三元";

        public override string Description =>
            "Two triplets or quads of dragons, plus a pair of the third.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            var dragons = player.Hand
                .Where(x => x.Type == MahjongTileType.Dragon).ToList();

            if (dragons.Count != 8)
                return false;

            var groupedDragons = dragons.GroupBy(x => x.name)
                .Select(x => x.Count()).ToList();

            if (groupedDragons.Count(x => x == 2) == 1
                && groupedDragons.Count(x => x == 3) >= 2)
                return true;

            return false;
        }
    }
}