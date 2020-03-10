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
        public ICellAbility outlineAbility;

        /// <summary>
        /// Cached RectTransform
        /// </summary>
        private RectTransform _rectTransform;

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
            _rectTransform = GetComponent<RectTransform>();
            outlineAbility = new CellOutlineAbility(outlineImage);
        }

        /// <summary>
        /// Check if (mouse) point is inside our cell (rect)
        /// </summary>
        /// <param name="point"></param>
        /// <returns>bool</returns>
        public bool IsPointInside(Vector3 point)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, point);
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