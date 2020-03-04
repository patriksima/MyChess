using UnityEngine;
using UnityEngine.UI;

public class Pawn : Piece
{
    public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        base.Setup(newTeamColor, newSpriteColor, newPieceManager);

        isFirstMove = true;

        movement = TeamColor == Color.white ? new Vector3Int(0, 1, 1) : new Vector3Int(0, -1, -1);

        GetComponent<Image>().sprite = Resources.Load<Sprite>("Pawn");
    }

    protected override void Move()
    {
        base.Move();

        isFirstMove = false;
    }

    private bool MatchesState(int targetX, int targetY, CellState targetState)
    {
        var cellState = currentCell.Board.ValidateCell(targetX, targetY, this);

        if (cellState == targetState)
        {
            highlightedCells.Add(currentCell.Board.AllCells[targetX, targetY]);
            return true;
        }

        return false;
    }

    protected override void CheckPathing()
    {
        var currentX = currentCell.BoardPosition.x;
        var currentY = currentCell.BoardPosition.y;

        MatchesState(currentX - movement.z, currentY + movement.z, CellState.Enemy);

        if (MatchesState(currentX, currentY + movement.y, CellState.Free))
            if (isFirstMove)
                MatchesState(currentX, currentY + movement.y * 2, CellState.Free);

        MatchesState(currentX + movement.z, currentY + movement.z, CellState.Enemy);
    }
}