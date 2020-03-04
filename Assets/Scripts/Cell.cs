using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image outlineImage = null;

    public Image OutlineImage
    {
        get => outlineImage;
        set => outlineImage = value;
    }

    public Vector2Int BoardPosition { get; set; }
    public Board Board { get; set; }
    public RectTransform RectTransform { get; set; }
    public Piece CurrentPiece { get; set; }

    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        this.BoardPosition = newBoardPosition;
        this.Board = newBoard;
        this.RectTransform = GetComponent<RectTransform>();
/*
        var d = new GameObject("Cell" + newBoardPosition.ToString());
        d.transform.SetParent(transform);
        d.transform.position = transform.position;
        Text t = d.AddComponent<Text>();
        t.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        t.fontSize = 20;
        t.color = Color.red;
        t.text = newBoardPosition.ToString();*/
    }

    public void RemovePiece()
    {
        if (CurrentPiece != null) CurrentPiece.Kill();
    }
}