using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public enum CellState
    {
        Free,
        Friendly,
        Enemy,
        OutOfBounds
    }

    /// <summary>
    /// Create chess board from 64 white and black squares (cells)
    /// </summary>
    public class Board : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Color32 whiteCellColor;
        [SerializeField] private Color32 blackCellColor;

        public Cell[,] AllCells { get; } = new Cell[8, 8];

        /// <summary>
        /// Setup the board
        /// </summary>
        public void Setup()
        {
            // Instantiate cell prefab and put them onto board
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    var cell = Instantiate(cellPrefab, transform);
                    var rectTransform = cell.GetComponent<RectTransform>();
                    var rect = rectTransform.rect;
                    var w = rect.width;
                    var h = rect.height;
                    rectTransform.anchoredPosition = new Vector2(x * w + w * .5f, y * h + h * .5f);

                    AllCells[x, y] = cell.GetComponent<Cell>();
                    AllCells[x, y].Setup(new Vector2Int(x, y), this);
                }
            }

            // Setup cells color (alternating black/white cells)
            var alternate = true;
            for (var y = 0; y < 8; y++)
            {
                for (var x = 0; x < 8; x++)
                {
                    AllCells[x, y].GetComponent<Image>().color = (alternate) ? blackCellColor : whiteCellColor;
                    alternate = !alternate;
                }

                alternate = !alternate;
            }
        }

        /// <summary>
        /// Check an return the cell state against given Piece
        /// Free, Enemy, Friendly, Out of Bounds
        /// </summary>
        /// <param name="targetX"></param>
        /// <param name="targetY"></param>
        /// <param name="checkingPiece"></param>
        /// <returns>CellState</returns>
        public CellState GetCellState(int targetX, int targetY, Piece checkingPiece)
        {
            if (targetX < 0 || targetX > 7 || targetY < 0 || targetY > 7)
            {
                return CellState.OutOfBounds;
            }

            var targetCell = AllCells[targetX, targetY];

            if (targetCell.CurrentPiece == null)
            {
                return CellState.Free;
            }

            if (checkingPiece.TeamColor == targetCell.CurrentPiece.TeamColor)
            {
                return CellState.Friendly;
            }

            if (checkingPiece.TeamColor != targetCell.CurrentPiece.TeamColor)
            {
                return CellState.Enemy;
            }

            return CellState.Free;
        }
    }
}