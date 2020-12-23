using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.Yakuman
{
    public class NineGates : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override int Yakuman => 1;
        public override string Name => "Nine Gates";
        public override string JapName => "Chuuren poutou";
        public override string KanjiName => "九蓮宝燈";

        public override string Description =>
            "A hand composed of 1-1-1-2-3-4-5-6-7-8-9-9-9 of one suit, plus any other tile of the same suit with the winning tile being part of this pattern.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var tilesByType = player.Hand.GroupBy(x => x.Type).ToList();

            if (tilesByType.Count > 1)
                return false;

            var tilesNumbers = tilesByType.Single().OrderBy(x => x.Number).Select(x => x.Number)
                .ToList();
            var tileNumbersString = string.Join("", tilesNumbers.Distinct());

            if (tileNumbersString != "123456789" || tilesNumbers.Count(x => x == 1) < 3 ||
                tilesNumbers.Count(x => x == 9) < 3)
                return false;

            tileNumbersString.Remove(board.WinningTile.Number);
            tileNumbersString = string.Join("", tilesNumbers);
            if (tileNumbersString == "1112345678999")
                return false;

            return true;
        }
    }
}