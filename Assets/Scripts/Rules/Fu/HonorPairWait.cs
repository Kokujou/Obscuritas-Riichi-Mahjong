using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class HonorPairWait : IMahjongFuRule
    {
        public int Fu { get; set; } = 2;
        public string Name => "Honor Pair Wait";
        public string JapName => "Tanki-Machi";
        public string KanjiName => "単騎待ち";
        public string Description => "Waiting for a valid honor pair";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var pair = handSplit.First(x => x.Count == 2);
            var firstTile = pair.First();
            if (board.WinningTile != firstTile)
                return 0;

            if (firstTile.Type == MahjongTileType.Dragon)
                return Fu;

            if (firstTile.Type != MahjongTileType.Wind)
                return 0;

            var fu = 0;
            if (firstTile.Name == board.CardinalPoint.ToString())
                fu += Fu;
            if (firstTile.Name == player.CardinalPoint.ToString())
                fu += Fu;

            return fu;
        }
    }
}