using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class AllSimples : MahjongRule
    {
        public override string Name => "All Simples";
        public override string JapName => "Tan'Yao";
        public override string KanjiName => "断么";
        public override string Description => "A hand composed only of numbered tiles from 2-8.";

        public override bool Fulfilled(MahjongBoard board, MahjongPlayer player)
        {
            if (player.Hand.Any(x => x.IsTerminal()))
                return false;

            return true;
        }
    }
}