namespace MyChess
{
    using System.Collections.Generic;

    public class PGNListener : PGNBaseListener
    {
        private int _lastMoveNumber;
        private readonly List<string> _lastMoves = new List<string>();

        // Game moves

        // Game tags


        public Dictionary<int, List<string>> Moves { get; } = new Dictionary<int, List<string>>();

        public Dictionary<string, string> Tags { get; } = new Dictionary<string, string>();

        /**
     * Game Info Tag like [Event "Event Name"], see http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm#c2.3
     */
        public override void EnterTag_pair(PGNParser.Tag_pairContext context)
        {
            Tags.Add(context.tag_name().GetText(), context.tag_value().GetText());
        }

        public override void EnterMove_number_indication(PGNParser.Move_number_indicationContext context)
        {
            if (_lastMoveNumber != 0)
            {
                Moves.Add(_lastMoveNumber, new List<string>(_lastMoves));
                _lastMoves.Clear();
            }

            _lastMoveNumber++;
        }

        public override void EnterElement(PGNParser.ElementContext context)
        {
            if (context.san_move() != null)
            {
                _lastMoves.Add(context.san_move().GetText());
            }
        }
    }
}