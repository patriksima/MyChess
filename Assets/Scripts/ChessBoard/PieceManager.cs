using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChessBoard
{
    public class PieceManager : MonoBehaviour
    {
        private readonly Dictionary<string, Type> pieceLibrary = new Dictionary<string, Type>
        {
            {"P", typeof(Pawn)},
            {"R", typeof(Rook)},
            {"N", typeof(Knight)},
            {"B", typeof(Bishop)},
            {"K", typeof(King)},
            {"Q", typeof(Queen)}
        };

        private readonly string[] pieceOrder = new string[16]
        {
            "P", "P", "P", "P", "P", "P", "P", "P",
            "R", "N", "B", "Q", "K", "B", "N", "R"
        };

        [SerializeField] private Color32 blackColor = new Color32(210, 95, 64, 255);

        private List<Piece> blackPieces;

        [SerializeField] private GameObject piecePrefab;
        [SerializeField] private Color32 whiteColor = new Color32(80, 124, 159, 255);

        private List<Piece> whitePieces;

        public bool IsKingAlive { get; set; } = true;

        public void Setup(Board board)
        {
            whitePieces = CreatePiece(Color.white, whiteColor, board);
            blackPieces = CreatePiece(Color.black, blackColor, board);
            PlacePieces(1, 0, whitePieces, board);
            PlacePieces(6, 7, blackPieces, board);
        }

        private List<Piece> CreatePiece(Color teamColor, Color32 spriteColor, Board board)
        {
            var newPieces = new List<Piece>();
            for (var i = 0; i < pieceOrder.Length; i++)
            {
                var newPieceObject = Instantiate(piecePrefab, transform);

                newPieceObject.transform.localScale = Vector3.one;
                newPieceObject.transform.localRotation = Quaternion.identity;

                var key = pieceOrder[i];
                var pieceType = pieceLibrary[key];

                var newPiece = (Piece) newPieceObject.AddComponent(pieceType);
                newPieces.Add(newPiece);

                newPiece.Setup(teamColor, spriteColor, this);
            }

            return newPieces;
        }

        private void PlacePieces(int pawnRow, int royaltyRow, List<Piece> pieces, Board board)
        {
            for (var i = 0; i < 8; i++)
            {
                pieces[i].Place(board.AllCells[i, pawnRow]);
                pieces[i + 8].Place(board.AllCells[i, royaltyRow]);
            }
        }

        private void SetInteractive(List<Piece> allPieces, bool value)
        {
            foreach (var piece in allPieces)
            {
                piece.enabled = true;
            }
        }

        public void SwitchSides(Color color)
        {
            if (!IsKingAlive)
            {
                ResetPieces();
                IsKingAlive = true;
                color = Color.black;
            }

            var isBlackTurn = color == Color.white ? true : false;

            SetInteractive(whitePieces, !isBlackTurn);

            SetInteractive(blackPieces, isBlackTurn);
        }

        public void ResetPieces()
        {
            foreach (var whitePiece in whitePieces)
            {
                whitePiece.Reset();
            }

            foreach (var blackPiece in blackPieces)
            {
                blackPiece.Reset();
            }
        }
    }
}