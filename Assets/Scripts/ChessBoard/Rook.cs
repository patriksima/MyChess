using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Rook : Piece
    {
        private Cell castleCell;
        public Cell CastleTriggercell { get; set; }

        public override void Setup(Color teamColor, Color32 imageColor, PieceManager pieceManager)
        {
            base.Setup(teamColor, imageColor, pieceManager);

            movement = new Vector3Int(7, 7, 0);
            
        }

        public override void AttachToCell(Cell cell)
        {
            base.AttachToCell(cell);

            // trigger cell
            var triggerOffset = CurrentCell.BoardPosition.x < 4 ? 2 : -1;
            CastleTriggercell = SetCell(triggerOffset);

            // castle cell
            var castleOffset = CurrentCell.BoardPosition.x < 4 ? 3 : -2;
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
            var newPosition = CurrentCell.BoardPosition;
            newPosition.x += offset;

            // return
            return CurrentCell.Board.AllCells[newPosition.x, newPosition.y];
        }
    }
}