using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class HonorTiles : MahjongRule
    {
        public override string Name => "Honor tiles";
        public override string JapName => "Yakuhai";
        public override string KanjiName => "役牌";

        public override string Description =>
            "One or more sets (min. 3x) of a dragon tile, a seat wind or a round wind.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            return GetHan(board, player) > 0;
        }

        public override int GetHan(MahjongBoard board, MahjongPlayer player)
        {
            var dragonTiles =
                player.Hand.GroupBy(x => x.Type)
                    .Where(x =>
                        x.Count() == 3 && (x.Key == MahjongTileType.Dragon
                                           || x.Key == MahjongTileType.Wind));

            var han = dragonTiles.Where(x => x.Key == MahjongTileType.Dragon
                                             || x.First().Name == board.CardinalPoint.ToString()
                                             || x.First().Name == player.CardinalPoint.ToString())
                .Count(x => x.Count() > 3);
            return han;
        }
    }
}