﻿using System;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules;
using ObscuritasRiichiMahjong.Rules.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class HandSplitResultComponent : MonoBehaviour
    {
        public Text FuNameListing;
        public Text FuPointsListing;
        public Transform HandSplitsParent;
        public Text ResultName;
        public Text ResultPoints;
        public Text YakuNameListing;
        public Text YakuPointsListing;

        public void Load(List<List<MahjongTile>> handSplit, MahjongPlayer player,
            MahjongBoard board)
        {
            var enrichedHand = handSplit.EnrichSplittedHand(player);
            if (enrichedHand.Count != 5)
                throw new NotImplementedException("The hand has an invalid number of groups != 5");

            var transforms = HandSplitsParent.Cast<Transform>().ToList();
            for (var index = 0; index < transforms.Count; index++)
            {
                var currentGroup = transforms[index];
                foreach (var tile in enrichedHand[index])
                {
                    var tileObject = new GameObject(tile.Name);
                    tileObject.transform.SetParent(currentGroup);
                    var image = tileObject.AddComponent<RawImage>();
                    image.texture = tile.Material.mainTexture;
                }
            }

            var pointResult = RuleProvider.CalculatePoints(handSplit, player, board);
            ResultName.text = pointResult.PointsDescription;
            ResultPoints.text = $"{pointResult.TotalPoints} pts.\n" +
                                $"{(pointResult.FromAll ? "from all" : "")}";

            var yakuman = pointResult.CollectedYaku.Where(x => x.Yakuman > 0).ToList();
            if (yakuman.Count > 0)
            {
                YakuNameListing.text = string.Join("\n", yakuman);
                YakuPointsListing.text = string.Join("\n",
                    yakuman.Select(x => $"{(x.Yakuman == 2 ? "Double" : "")} Yakuman"));
            }
            else
            {
                YakuNameListing.text = string.Join("\n", pointResult.CollectedYaku);
                YakuPointsListing.text = string.Join("\n",
                    pointResult.CollectedYaku.Select(x => $"{x.Han} Han"));
            }
        }
    }
}