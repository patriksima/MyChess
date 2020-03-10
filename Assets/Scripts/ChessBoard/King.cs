using MyChess;
using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class King : Piece
    {
        private Rook leftRook;
        private Rook rightRook;

        public override void Setup(Color teamColor, Color32 imageColor, PieceManager pieceManager)
        {
            base.Setup(teamColor, imageColor, pieceManager);

            movement = new Vector3Int(1, 1, 1);
        }

        public override void Kill()
        {
            base.Kill();

            GameManager.Instance.IsKingAlive = false;
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

            if (rook.CastleTriggercell != CurrentCell)
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
            var currentX = CurrentCell.BoardPosition.x;
            var currentY = CurrentCell.BoardPosition.y;

            // Go through the cells in between
            for (var i = 1; i < count; i++)
            {
                var offsetX = currentX + i * direction;
                var cellState = CurrentCell.Board.GetCellState(offsetX, currentY, this);

                if (cellState != CellState.Free)
                {
                    return null;
                }
            }

            // Try and get rook
            var rookCell = CurrentCell.Board.AllCells[currentX + count * direction, currentY];
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

            if (rook.TeamColor != TeamColor || !rook.isFirstMove)
            {
                return null;
            }

            // Add castle trigger to movement
            highlightedCells.Add(rook.CastleTriggercell);

            return rook;
        }
    }
}