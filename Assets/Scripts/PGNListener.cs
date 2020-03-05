using System.Collections.Generic;
using UnityEngine;

public class PGNListener : PGNBaseListener
{
    private int _lastMoveNumber = 0;
    private List<string> _lastMoves = new List<string>();
    private Dictionary<int, List<string>> _moves = new Dictionary<int, List<string>>();

    public Dictionary<int, List<string>> Moves => _moves;

    public override void EnterTag_section(PGNParser.Tag_sectionContext context)
    {
        foreach (var tagPairContext in context.tag_pair())
        {
            Debug.Log("Enter Tag section: " + tagPairContext.GetText());
        }
    }

/*
    public override void ExitTag_section(PGNParser.Tag_sectionContext context)
    {
        Debug.Log("End Tag: " + context.tag_pair());
    }
*/
    public override void EnterTag_pair(PGNParser.Tag_pairContext context)
    {
        Debug.Log("Open Tag_pair: " + context.tag_name().GetText() + ", " + context.tag_value().GetText());
    }

/*
    public override void ExitTag_pair(PGNParser.Tag_pairContext context)
    {
        Debug.Log("End Tag_pair: " + context.tag_name().GetText() + ", " + context.tag_value().GetText());
    }*/

    public override void EnterMove_number_indication(PGNParser.Move_number_indicationContext context)
    {
        if (_lastMoveNumber != 0)
        {
            _moves.Add(_lastMoveNumber, new List<string>(_lastMoves));
            _lastMoves.Clear();
        }

        _lastMoveNumber++;
    }

    public override void EnterElement_sequence(PGNParser.Element_sequenceContext context)
    {
        Debug.Log("Element sequence: " + context.GetText());
    }

    public override void EnterElement(PGNParser.ElementContext context)
    {
        if (context.san_move() != null)
        {
            _lastMoves.Add(context.san_move().GetText());
        }
    }
}