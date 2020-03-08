using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public interface ICell
    {
        #region Chess Stuff

        Board Board { get; set; }
        Vector2Int BoardPosition { get; set; }
        Piece CurrentPiece { get; set; }

        #endregion

        #region MonoBehaviour Stuff

        RectTransform Rect { get; set; }

        #endregion

        void Setup(Vector2Int newBoardPosition, Board newBoard);
        void RemovePiece();
    }

    public class Cell : MonoBehaviour, ICell
    {
        [SerializeField] private Image outlineImage;

        public Image OutlineImage => outlineImage;

        public Vector2Int BoardPosition { get; set; }
        public Board Board { get; set; }
        public RectTransform Rect { get; set; }
        public Piece CurrentPiece { get; set; }

        public void Setup(Vector2Int newBoardPosition, Board newBoard)
        {
            BoardPosition = newBoardPosition;
            Board = newBoard;
            Rect = GetComponent<RectTransform>();
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