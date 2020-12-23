using System.Collections.Generic;
using ObscuritasRiichiMahjong.Models;

namespace ObscuritasRiichiMahjong.Rules.Interfaces
{
    public abstract class MahjongRule
    {
        public virtual bool AcceptOpenHand => true;
        public virtual int Han => 1;
        public virtual int OpenHandPunishment => 0;
        public abstract string Name { get; }
        public abstract string JapName { get; }
        public abstract string KanjiName { get; }
        public abstract string Description { get; }

        public abstract bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player);

        public virtual int GetHan(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            if (!AcceptOpenHand && player.HandOpen)
                return 0;

            if (Fulfilled(handSplit, board, player))
                return Han - OpenHandPunishment;

            return 0;
        }

        public override string ToString()
        {
            return $"{JapName}({KanjiName})\t\t\t{Han} Han";
        }
    }
}