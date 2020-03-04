using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;

    public Cell[,] AllCells { get; set; } = new Cell[8, 8];

    public void Create()
    {
        for (var y = 0; y < 8; y++)
        for (var x = 0; x < 8; x++)
        {
            var cell = Instantiate(cellPrefab, transform);
            var rect = cell.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x * 100 + 50, y * 100 + 50);

            AllCells[x, y] = cell.GetComponent<Cell>();
            AllCells[x, y].Setup(new Vector2Int(x, y), this);
        }

        for (var x = 0; x < 8; x += 2)
        for (var y = 0; y < 8; y++)
        {
            var offset = y % 2 == 0 ? 1 : 0;
            var finalX = x + offset;

            AllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
        }
    }

    public CellState ValidateCell(int targetX, int targetY, Piece checkingPiece)
    {
        if (targetX < 0 || targetX > 7)
            return CellState.OutOfBounds;
        if (targetY < 0 || targetY > 7)
            return CellState.OutOfBounds;

        var targetCell = AllCells[targetX, targetY];

        if (targetCell.CurrentPiece != null)
        {
            if (checkingPiece.TeamColor == targetCell.CurrentPiece.TeamColor) return CellState.Friendly;

            if (checkingPiece.TeamColor != targetCell.CurrentPiece.TeamColor) return CellState.Enemy;
        }

        return CellState.Free;
    }
}