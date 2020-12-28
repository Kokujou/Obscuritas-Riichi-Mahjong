using System.Collections.Generic;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class BasePoints : IMahjongFuRule
    {
        public int Fu { get; set; } = 20;
        public string Name => "Winning Hand";
        public string JapName => "Fuutei";
        public string KanjiName => "副底";
        public string Description => "Base Points for a winning hand.";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return Fu;
        }
    }
}