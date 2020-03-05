using System;
using System.IO;
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
        if (Input.GetKey("escape"))
        {
            Quit();
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public static void LoadFile(string fileName)
    {
        Debug.Log("Loading " + fileName);
        
        try
        {
            string pgn = "";
            string line;

            StreamReader theReader = new StreamReader(fileName);

            using (theReader)
            {
                do
                {
                    line = theReader.ReadLine();
                    if (line != null)
                    {
                        pgn += line;
                    }
                } while (line != null);

                theReader.Close();
            }

            Debug.Log(pgn);
        }
        catch (IOException e)
        {
            Debug.Log(fileName + " cannot be read.");
            Debug.Log(e.Message);
        }
    }
}