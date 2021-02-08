using System.Collections.Generic;
using ObscuritasRiichiMahjong.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class ReadyHand : MahjongRule
    {
        public override string Name => "Ready hand";
        public override string JapName => "Riichi";
        public override string KanjiName => "立直";

        public override string Description =>
            "Player declared riichi on a closed hand with only one tile missing for completion.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (player.Riichi == RiichiType.Riichi)
                return true;

            return false;
        }
    }
}