using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class King : Piece
{
    private Rook leftRook = null;
    private Rook rightRook = null;

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
        if (rook == null) return false;

        if (rook.CastleTriggercell != currentCell)
            return false;

        return true;
    }

    private Rook GetRook(int direction, int count)
    {
        // Has the king moved?
        if (!isFirstMove) return null;

        // Position
        int currentX = currentCell.BoardPosition.x;
        int currentY = currentCell.BoardPosition.y;

        // Go through the cells in between
        for (int i = 1; i < count; i++)
        {
            int offsetX = currentX + (i * direction);
            CellState cellState = currentCell.Board.ValidateCell(offsetX, currentY, this);

            if (cellState != CellState.Free)
            {
                return null;
            }
        }

        // Try and get rook
        Cell rookCell = currentCell.Board.AllCells[currentX + (count * direction), currentY];
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
            return null;

        if (rook.TeamColor != TeamColor || !rook.IsFirstMove)
            return null;

        // Add castle trigger to movement
        if (rook != null)
        {
            highlightedCells.Add(rook.CastleTriggercell);
        }

        return rook;
    }
}