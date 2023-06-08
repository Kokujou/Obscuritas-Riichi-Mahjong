using ObscuritasRiichiMahjong.Assets.Scripts.Core.Data;
using ObscuritasRiichiMahjong.Models;
using ObscuritasRiichiMahjong.Rules;
using ObscuritasRiichiMahjong.Rules.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace ObscuritasRiichiMahjong
{
    public class GameEndComponent : MonoBehaviour
    {
        public GameObject GameEndUi;

        private List<VisualElement> handTiles;
        private List<VisualElement> kanDoraTiles;
        private List<VisualElement> uraDoraTiles;
        private TextElement romajiPointText;
        private TextElement winningMoveType;
        private List<(TextElement, TextElement)> yakuPoints;
        private TextElement primaryPointDescription;
        private TextElement secondaryPointDescription;
        private TextElement specialYakuName;

        public void Initialize(MahjongBoard board)
        {

            var validHands = board.Winner.Hand.GetValidHands();
            var possiblePoints = validHands.Select(x => x.CalculatePoints(board.Winner, board));
            var maxPoints = possiblePoints.Max(x => x.TotalPoints);
            var result = possiblePoints.First(x => x.TotalPoints == maxPoints);

            board.Winner.Hand.Remove(board.WinningTile);
            board.Winner.Hand.Add(board.WinningTile);

            InitializeGameEndUi();
            AssignPointsToUi(result, board);
        }

        private void AssignPointsToUi(PointResult pointResult, MahjongBoard board)
        {
            for (var i = 0; i < 14; i++)
                handTiles[i].style.backgroundImage = (Texture2D)board.Winner.Hand[i].Material.mainTexture;

            var exposedKanDora = board.KanDora.Where(x => x.Exposed).ToList();
            for (var i = 0; i < exposedKanDora.Count; i++)
            {
                kanDoraTiles[i].style.backgroundImage = (Texture2D)board.KanDora[i].Material.mainTexture;
                uraDoraTiles[i].style.backgroundImage = (Texture2D)board.UraDora[i].Material.mainTexture;
            }

            romajiPointText.text = pointResult.TotalPoints.ToString();
            winningMoveType.text = board.WinningMoveType.ToString();
            specialYakuName.text = pointResult.PointsDescriptionText;

            var pointDescription = pointResult.GetPointsDescription();
            if (pointDescription != PointDescription.None)
            {
                primaryPointDescription.text = pointDescription.ToString();
                secondaryPointDescription.RemoveFromHierarchy();
            }
            else
            {
                primaryPointDescription.text = pointResult.Han + " Han";
                secondaryPointDescription.text = pointResult.Fu + " Fu";
            }

            for (var i = 0; i < yakuPoints.Count; i++)
            {
                var yakuElement = yakuPoints[i];
                if (i >= pointResult.CollectedYaku.Count)
                {
                    yakuElement.Item1.parent.RemoveFromHierarchy();
                    continue;
                }

                var yaku = pointResult.CollectedYaku[i];
                yakuElement.Item1.text = $"{yaku.Name} ({yaku.KanjiName})";
                yakuElement.Item2.text = $"{yaku.Han} Han";
            }
        }

        private void InitializeGameEndUi()
        {
            var gameEndUi = Instantiate(GameEndUi).GetComponent<UIDocument>();
            var root = gameEndUi.rootVisualElement;
            handTiles = root.Query(className: "tile").Build().ToList();
            kanDoraTiles = root.Query("kan-dora-view").Children<VisualElement>(className: "dora-tile").Build().ToList();
            uraDoraTiles = root.Query("ura-dora-view").Children<VisualElement>(className: "dora-tile").Build().ToList();
            romajiPointText = root.Q<TextElement>("romaji-points-view");
            winningMoveType = root.Q<TextElement>("winning-type-button");
            yakuPoints = root.Query(className: "yaku").Build()
                .Select(x => (x.Q<TextElement>(className: "yaku-name"), x.Q<TextElement>(className: "yaku-points")))
                .ToList();
            primaryPointDescription = root.Q<TextElement>("primary-points");
            secondaryPointDescription = root.Q<TextElement>("secondary-points");
            specialYakuName = root.Q<TextElement>("special-yakuman-name");
        }
    }
}
