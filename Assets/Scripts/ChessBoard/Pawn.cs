using MyChess;
using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Pawn : Piece
    {
        public override void Setup(Color teamColor, Color32 imageColor, PieceManager pieceManager)
        {
            base.Setup(teamColor, imageColor, pieceManager);

            movement = TeamColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);

        }

        private bool MatchesState(int targetX, int targetY, CellState targetState)
        {
            var cellState = CurrentCell.Board.GetCellState(targetX, targetY, this);

            if (cellState == targetState)
            {
                highlightedCells.Add(CurrentCell.Board.AllCells[targetX, targetY]);
                return true;
            }

            return false;
        }

        protected override void CheckPathing()
        {
            var currentX = CurrentCell.BoardPosition.x;
            var currentY = CurrentCell.BoardPosition.y;

            MatchesState(currentX - movement.z, currentY + movement.z, CellState.Enemy);

            if (MatchesState(currentX, currentY + movement.y, CellState.Free))
            {
                if (isFirstMove)
                {
                    MatchesState(currentX, currentY + movement.y * 2, CellState.Free);
                }
            }

            MatchesState(currentX + movement.z, currentY + movement.z, CellState.Enemy);
        }
    }
}