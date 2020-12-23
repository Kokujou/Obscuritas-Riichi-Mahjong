using System;
using System.Collections.Generic;
using System.Linq;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules;
using UnityEngine;
using UnityEngine.UI;

namespace ObscuritasRiichiMahjong.PointCalculation.Components
{
    public class HandSplitResultComponent : MonoBehaviour
    {
        public Text FuListing;
        public Transform HandSplitsParent;
        public Text ResultName;
        public Text ResultPoints;
        public Text YakuListing;

        public void Load(List<List<MahjongTile>> handSplit, MahjongPlayer player,
            MahjongBoard board)
        {
            if (handSplit.Count != 5)
                throw new NotImplementedException("The hand has an invalid number of groups != 5");

            var transforms = HandSplitsParent.Cast<Transform>().ToList();
            for (var index = 0; index < transforms.Count; index++)
            {
                var currentGroup = transforms[index];
                foreach (var tile in handSplit[index])
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
            YakuListing.text = string.Join("\n", pointResult.CollectedYaku);
        }
    }
}