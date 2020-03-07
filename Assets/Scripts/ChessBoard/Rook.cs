using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Rook : Piece
    {
        private Cell castleCell;
        public Cell CastleTriggercell { get; set; }

        public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
        {
            base.Setup(newTeamColor, newSpriteColor, newPieceManager);

            movement = new Vector3Int(7, 7, 0);
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Rook");
        }

        public override void Place(Cell newCell)
        {
            base.Place(newCell);

            // trigger cell
            var triggerOffset = currentCell.BoardPosition.x < 4 ? 2 : -1;
            CastleTriggercell = SetCell(triggerOffset);

            // castle cell
            var castleOffset = currentCell.BoardPosition.x < 4 ? 3 : -2;
            castleCell = SetCell(castleOffset);
        }

        public void Castle()
        {
            // Set target
            targetCell = castleCell;

            // Move
            Move();
        }

        private Cell SetCell(int offset)
        {
            // ne position
            var newPosition = currentCell.BoardPosition;
            newPosition.x += offset;

            // return
            return currentCell.Board.AllCells[newPosition.x, newPosition.y];
        }
    }
}