using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Image outlineImage;

        public Image OutlineImage => outlineImage;

        public Vector2Int BoardPosition { get; set; }
        public Board Board { get; set; }
        public RectTransform RectTransform { get; set; }
        public Piece CurrentPiece { get; set; }

        public void Setup(Vector2Int newBoardPosition, Board newBoard)
        {
            BoardPosition = newBoardPosition;
            Board = newBoard;
            RectTransform = GetComponent<RectTransform>();
        }

        public void RemovePiece()
        {
            if (CurrentPiece != null)
            {
                CurrentPiece.Kill();
            }
        }
    }
}