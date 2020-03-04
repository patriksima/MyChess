using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Board board = null;
    [SerializeField] private PieceManager pieceManager = null;

    // Start is called before the first frame update
    void Start()
    {
        board.Create();
        pieceManager.Setup(board);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}