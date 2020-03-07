namespace MyChess
{
    using TMPro;
    using UnityEngine;

    public class InfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI gameScore;
        [SerializeField] private TextMeshProUGUI mainContent;
        [SerializeField] private TextMeshProUGUI playerBlack;
        [SerializeField] private TextMeshProUGUI playerWhite;

        public void ShowGameData()
        {
            var gameData = GameManager.Instance.GameData;

            playerWhite.text = gameData.White;
            playerBlack.text = gameData.Black;
            gameScore.text = gameData.GetResultAsText();

            // build game string
            var movesText = "";
            foreach (var move in gameData.Moves)
            {
                var m1 = move.Value.White;
                var m2 = move.Value.Black;

                // if pawn move, add pawn symbol
                if (m1.Length == 2)
                {
                    m1 = "P" + m1;
                }

                if (m2.Length == 2)
                {
                    m2 = "P" + m2;
                }

                movesText += move.Key + ". ";
                movesText += "<link=W" + move.Key + m1 + ">" + move.Value.White + "</link> ";
                movesText += "<link=B" + move.Key + m2 + ">" + move.Value.Black + "</link> ";
            }

            mainContent.text = movesText;
        }
    }
}