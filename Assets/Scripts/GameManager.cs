using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Board board;
    [SerializeField] private PieceManager pieceManager;

    private Dictionary<string, string> _tagSection = new Dictionary<string, string>();
    private Dictionary<int, string[]> _moves = new Dictionary<int, string[]>();

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

    public void LoadFile(string fileName)
    {
        Debug.Log("Loading " + fileName);

        try
        {
            var lexer = new PGNLexer(new AntlrInputStream(new StreamReader(fileName)));
            var stream = new CommonTokenStream(lexer);
            var parser = new PGNParser(stream);

            var ctx = parser.parse();

            if (ctx == null || ctx.pgn_database() == null)
            {
                Debug.Log("Wrong PGN format.");
                return;
            }

            Debug.Log("Count games in PGN file: " + ctx.pgn_database().pgn_game().Length);
/*
            foreach (var node in ctx.pgn_database().pgn_game()[0].tag_section().tag_pair())
            {
                Debug.Log(node.tag_name().GetText() + ":" + node.tag_value().GetText());
                _tagSection.Add(node.tag_name().GetText(), node.tag_value().GetText());
            }

            int n;
            string[] m = new string[2];
            foreach (var node in ctx.pgn_database().pgn_game()[0].movetext_section().element_sequence().element())
            {
                var a = node.move_number_indication();
                var b = node.san_move();
                var c = node.NUMERIC_ANNOTATION_GLYPH();

                Debug.Log("No." + a?.GetText() ?? "");
                Debug.Log("SAN:" + b?.GetText() ?? "");
                Debug.Log("Glyph:" + c?.GetText() ?? "");

                if (a != null)
                {
                    n = a.getAltNumber();
                    Debug.Log("No." + n);
                }

                if (a == null && b != null)
                {
                    m[0] = b.GetText();
                }
            }
*/

            var walker = ParseTreeWalker.Default;
            var listener = new PGNListener();
            walker.Walk(listener, ctx);

            foreach (var move in listener.Moves)
            {
                Debug.Log(move.Key.ToString() + ". " + string.Join(", ", move.Value));
            }
        }
        catch (IOException e)
        {
            Debug.Log(fileName + " cannot be read.");
            Debug.Log(e.Message);
        }
    }
}