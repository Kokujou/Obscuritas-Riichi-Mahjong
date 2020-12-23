using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
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

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return GetHan(handSplit, board, player) > 0;
        }

        public override int GetHan(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTriplets = handSplit.EnrichSplittedHand(player).GetTriplets();
            var dragonTilesCount =
                allTriplets.Count(x => IsValidHonorTile(x.First(), player, board));

            return dragonTilesCount * Han;
        }

        public static bool IsValidHonorTile(MahjongTile tile, MahjongPlayer player,
            MahjongBoard board)
        {
            if (tile.Type == MahjongTileType.Dragon)
                return true;

            if (tile.Type != MahjongTileType.Wind)
                return false;

            if (tile.Name == board.CardinalPoint.ToString() ||
                tile.name == player.CardinalPoint.ToString())
                return true;

            return false;
        }
    }
}