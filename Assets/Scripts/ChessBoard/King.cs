using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class King : Piece
    {
        private Rook leftRook;
        private Rook rightRook;

        public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
        {
            base.Setup(newTeamColor, newSpriteColor, newPieceManager);

            movement = new Vector3Int(1, 1, 1);
            GetComponent<Image>().sprite = Resources.Load<Sprite>("King");
        }

        public override void Kill()
        {
            base.Kill();

            pieceManager.IsKingAlive = false;
        }

        protected override void CheckPathing()
        {
            base.CheckPathing();

            // right
            rightRook = GetRook(1, 3);

            // left
            leftRook = GetRook(-1, 4);
        }

        protected override void Move()
        {
            base.Move();

            if (CanCastle(leftRook))
            {
                leftRook.Castle();
            }

            if (CanCastle(rightRook))
            {
                rightRook.Castle();
            }
        }

        private bool CanCastle(Rook rook)
        {
            if (rook == null)
            {
                return false;
            }

            if (rook.CastleTriggercell != currentCell)
            {
                return false;
            }

            return true;
        }

        private Rook GetRook(int direction, int count)
        {
            // Has the king moved?
            if (!isFirstMove)
            {
                return null;
            }

            // Position
            var currentX = currentCell.BoardPosition.x;
            var currentY = currentCell.BoardPosition.y;

            // Go through the cells in between
            for (var i = 1; i < count; i++)
            {
                var offsetX = currentX + i * direction;
                var cellState = currentCell.Board.ValidateCell(offsetX, currentY, this);

                if (cellState != CellState.Free)
                {
                    return null;
                }
            }

            // Try and get rook
            var rookCell = currentCell.Board.AllCells[currentX + count * direction, currentY];
            Rook rook = null;

            // Cast
            if (rookCell.CurrentPiece != null)
            {
                if (rookCell.CurrentPiece is Rook)
                {
                    rook = (Rook) rookCell.CurrentPiece;
                }
            }

            if (rook == null)
            {
                return null;
            }

            if (rook.TeamColor != TeamColor || !rook.IsFirstMove)
            {
                return null;
            }

            // Add castle trigger to movement
            highlightedCells.Add(rook.CastleTriggercell);

            return rook;
        }
    }
}