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

        public abstract bool Fulfilled(MahjongBoard board, MahjongPlayer player);

        public virtual int GetHan(MahjongBoard board, MahjongPlayer player)
        {
            if (!AcceptOpenHand && player.HandOpen)
                return 0;

            if (Fulfilled(board, player))
                return Han - OpenHandPunishment;

            return 0;
        }
    }
}