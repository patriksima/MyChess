using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    /// <summary>
    /// One square on the chessboard holding piece or be empty
    /// </summary>
    public class Cell : MonoBehaviour
    {
        #region MonoBehaviour Stuff

        [SerializeField] private Image outlineImage;
        public Image OutlineImage => outlineImage;

        /// <summary>
        /// Cached RectTransform
        /// </summary>
        public RectTransform CellRectTransform { get; private set; }

        #endregion

        #region Cell Stuff

        public Vector2Int BoardPosition { get; private set; }
        public Board Board { get; private set; }
        public Piece CurrentPiece { get; private set; }

        #endregion

        /// <summary>
        /// Setup position on the board
        /// </summary>
        /// <param name="boardPosition"></param>
        /// <param name="board"></param>
        public void Setup(Vector2Int boardPosition, Board board)
        {
            Board = board;
            BoardPosition = boardPosition;
            CellRectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        /// Set a piece
        /// </summary>
        /// <param name="piece"></param>
        public void SetPiece(Piece piece)
        {
            CurrentPiece = piece;
        }

        /// <summary>
        /// Remove a piece
        /// </summary>
        public void RemovePiece()
        {
            CurrentPiece = null;
        }

        /// <summary>
        /// Kill piece
        /// </summary>
        public void KillPiece()
        {
            if (CurrentPiece != null)
            {
                CurrentPiece.Kill();
            }
        }
    }
}