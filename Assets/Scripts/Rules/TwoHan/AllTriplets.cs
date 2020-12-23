﻿using System.Collections.Generic;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules.Extensions;
using ObscuritasRiichiMahjong.Rules.Interfaces;

namespace ObscuritasRiichiMahjong.Rules.TwoHan
{
    public class AllTriplets : MahjongRule
    {
        public override int Han => 2;
        public override string Name => "All Triplets";
        public override string JapName => "Toi Toi";
        public override string KanjiName => "対々";
        public override string Description => "The hand consists only of triplets or quads";

        public override bool Fulfilled(List<List<MahjongTile>> handSplit, MahjongBoard board,
            MahjongPlayer player)
        {
            var allTripletsOrQuads = handSplit.EnrichSplittedHand(player).GetTripletsOrQuads();
            if (allTripletsOrQuads.Count == 4)
                return true;

            return false;
        }
    }
}