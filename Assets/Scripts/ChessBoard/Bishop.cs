using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Bishop : Piece
    {
        public override void Setup(Color teamColor, Color32 imageColor, PieceManager pieceManager)
        {
            base.Setup(teamColor, imageColor, pieceManager);

            movement = new Vector3Int(0, 0, 7);
        }
    }
}