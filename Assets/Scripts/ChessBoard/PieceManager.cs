using System.Collections.Generic;
using MyChess;
using UnityEngine;

namespace ChessBoard
{
    public class PieceManager : MonoBehaviour
    {
        #region Prefabs

        [Header("Pieces")] [SerializeField] private Piece pawnPrefab;
        [SerializeField] private Piece rookPrefab;
        [SerializeField] private Piece knightPrefab;
        [SerializeField] private Piece bishopPrefab;
        [SerializeField] private Piece queenPrefab;
        [SerializeField] private Piece kingPrefab;

        [Header("Appearance")] [SerializeField]
        private Color32 whiteColor = new Color32(80, 124, 159, 255);

        [SerializeField] private Color32 blackColor = new Color32(210, 95, 64, 255);

        #endregion

        #region Piece Setup

        private readonly string[] _pieceOrder =
        {
            "P", "P", "P", "P", "P", "P", "P", "P",
            "R", "N", "B", "Q", "K", "B", "N", "R"
        };

        private Board _board;
        private readonly List<Piece> _whitePieces = new List<Piece>();
        private readonly List<Piece> _blackPieces = new List<Piece>();

        #endregion

        /// <summary>
        /// Instantiate and place all pieces to board
        /// </summary>
        /// <param name="board"></param>
        public void Setup(Board board)
        {
            _board = board;

            // instantiate pieces from prefabs
            foreach (var symbol in _pieceOrder)
            {
                switch (symbol)
                {
                    case "P":
                        _whitePieces.Add(CreatePiece(Color.white, pawnPrefab));
                        _blackPieces.Add(CreatePiece(Color.black, pawnPrefab));
                        break;
                    case "R":
                        _whitePieces.Add(CreatePiece(Color.white, rookPrefab));
                        _blackPieces.Add(CreatePiece(Color.black, rookPrefab));
                        break;
                    case "N":
                        _whitePieces.Add(CreatePiece(Color.white, knightPrefab));
                        _blackPieces.Add(CreatePiece(Color.black, knightPrefab));
                        break;
                    case "B":
                        _whitePieces.Add(CreatePiece(Color.white, bishopPrefab));
                        _blackPieces.Add(CreatePiece(Color.black, bishopPrefab));
                        break;
                    case "Q":
                        _whitePieces.Add(CreatePiece(Color.white, queenPrefab));
                        _blackPieces.Add(CreatePiece(Color.black, queenPrefab));
                        break;
                    case "K":
                        _whitePieces.Add(CreatePiece(Color.white, kingPrefab));
                        _blackPieces.Add(CreatePiece(Color.black, kingPrefab));
                        break;
                }
            }

            // place pieces
            for (var i = 0; i < 8; i++)
            {
                _whitePieces[i].Place(_board.AllCells[i, 1]);
                _whitePieces[i + 8].Place(_board.AllCells[i, 0]);
                _blackPieces[i].Place(_board.AllCells[i, 6]);
                _blackPieces[i + 8].Place(_board.AllCells[i, 7]);
            }
        }

        /// <summary>
        /// Instantiate and setup piece
        /// </summary>
        /// <param name="side">White or Black</param>
        /// <param name="prefab">Prefab of piece</param>
        /// <returns>Piece</returns>
        private Piece CreatePiece(Color side, Piece prefab)
        {
            var piece = Instantiate(prefab, _board.transform);
            piece.Setup(side, side == Color.white ? whiteColor : blackColor, this);
            return piece;
        }

        /// <summary>
        /// Enable/disable interactivity of players pieces
        /// </summary>
        /// <param name="teamColor"></param>
        public void SwitchSides(Color teamColor)
        {
            // TODO: Refactor
            if (!GameManager.Instance.IsKingAlive)
            {
                ResetPieces();
                GameManager.Instance.IsKingAlive = true;
                teamColor = Color.black;
            }

            foreach (var piece in _whitePieces)
            {
                piece.enabled = teamColor != Color.white;
            }

            foreach (var piece in _blackPieces)
            {
                piece.enabled = teamColor != Color.black;
            }
        }

        /// <summary>
        /// Reset board and position of all pieces
        /// </summary>
        private void ResetPieces()
        {
            foreach (var whitePiece in _whitePieces)
            {
                whitePiece.Reset();
            }

            foreach (var blackPiece in _blackPieces)
            {
                blackPiece.Reset();
            }
        }
    }
}