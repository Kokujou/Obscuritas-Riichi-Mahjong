using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace ObscuritasRiichiMahjong.Rules.OneHan
{
    public class PureDoubleSequences : MahjongRule
    {
        public override bool AcceptOpenHand => false;
        public override string Name => "Pure Double Sequences";
        public override string JapName => "Iipeikou";
        public override string KanjiName => "一盃口";
        public override string Description => "Two times the same sequence in the same suit.";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var sequences = handSplit.GetSequences();
            var numberSequences = sequences.Select(GetTilesAsNumber).ToList();
            return numberSequences.Any(sequence => numberSequences.Count(x => x == sequence) >= 2);
        }

        public static string GetTilesAsNumber(List<MahjongTile> tiles)
        {
            var result = $"{tiles.First().Type.ToString().First()}";

            foreach (var tile in tiles) result += tile.Number;

            return result;
        }
    }
}