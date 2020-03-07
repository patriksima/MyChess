using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Knight : Piece
    {
        public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
        {
            base.Setup(newTeamColor, newSpriteColor, newPieceManager);

            GetComponent<Image>().sprite = Resources.Load<Sprite>("Knight");
        }

        private void CreateCellPath(int flipper)
        {
            var currentX = currentCell.BoardPosition.x;
            var currentY = currentCell.BoardPosition.y;

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
            var cellState = currentCell.Board.ValidateCell(targetX, targetY, this);

            if (cellState != CellState.Friendly && cellState != CellState.OutOfBounds)
            {
                highlightedCells.Add(currentCell.Board.AllCells[targetX, targetY]);
            }
        }
    }
}