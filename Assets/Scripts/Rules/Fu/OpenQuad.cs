using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.Fu
{
    public class OpenQuad : IMahjongFuRule
    {
        public int Fu { get; set; } = 8;
        public string Name => "Closed Quad";
        public string JapName => "Minkan";
        public string KanjiName => "暗槓";
        public string Description => "A closed Quad";

        public int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            return player.HiddenKan.Sum(x => x.First().IsTerminal() ? Fu * 2 : Fu);
        }
    }
}