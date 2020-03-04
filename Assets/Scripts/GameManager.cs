using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board;
    [SerializeField] private PieceManager pieceManager;

    // Start is called before the first frame update
    private void Start()
    {
        board.Create();
        pieceManager.Setup(board);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKey("escape")) Quit();
    }

    public void Quit()
    {
        Application.Quit();
    }
}