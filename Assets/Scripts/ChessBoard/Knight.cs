using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Knight : Piece
    {


        private void CreateCellPath(int flipper)
        {
            var currentX = CurrentCell.BoardPosition.x;
            var currentY = CurrentCell.BoardPosition.y;

            //left
            MatchesState(currentX - 2, currentY + flipper);
            //Upper left
            MatchesState(currentX - 1, currentY + 2 * flipper);
            //Upper right
            MatchesState(currentX + 1, currentY + 2 * flipper);
            //Right
            MatchesState(currentX + 2, currentY + flipper);
        }

        protected override void CheckPathing()
        {
            CreateCellPath(1);
            CreateCellPath(-1);
        }

        private void MatchesState(int targetX, int targetY)
        {
            var cellState = CurrentCell.Board.GetCellState(targetX, targetY, this);

            if (cellState != CellState.Friendly && cellState != CellState.OutOfBounds)
            {
                highlightedCells.Add(CurrentCell.Board.AllCells[targetX, targetY]);
            }
        }
    }
}