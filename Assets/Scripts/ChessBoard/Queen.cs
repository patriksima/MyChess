using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Queen : Piece
    {
        public override void Setup(Color teamColor, Color32 imageColor, PieceManager pieceManager)
        {
            base.Setup(teamColor, imageColor, pieceManager);

            movement = new Vector3Int(7, 7, 7);
        }
    }
}