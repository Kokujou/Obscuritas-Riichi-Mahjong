using System.Collections.Generic;
using ObscuritasRiichiMahjong.Models;

namespace ObscuritasRiichiMahjong.Rules.Interfaces
{
    public interface IMahjongFuRule
    {
        int Fu { get; set; }
        string Name { get; }
        string JapName { get; }
        string KanjiName { get; }
        string Description { get; }

        int GetFu(List<List<MahjongTile>> handSplit, MahjongBoard board, MahjongPlayer player);
    }
}