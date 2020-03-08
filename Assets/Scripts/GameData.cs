using System.Collections;
using System.Linq;
using System.Text;
using ChessBoard;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace MyChess
{
    public enum GameResult
    {
        None,
        Win,
        Draw,
        Loss
    }

    public class Move
    {
        private Vector2Int _vector;
        private IPiece _piece;

        public Vector2Int Vector => _vector;

        public IPiece Piece => _piece;

        public Move(Vector2Int vector, IPiece piece)
        {
            _vector = vector;
            _piece = piece;
        }

        private string GetPieceAsSymbol()
        {
            string symbol;

            switch (_piece)
            {
                case Pawn pawn:
                    symbol = "";
                    break;
                case Rook rook:
                    symbol = "R";
                    break;
                case Knight knight:
                    symbol = "N";
                    break;
                case Bishop bishop:
                    symbol = "B";
                    break;
                case Queen queen:
                    symbol = "Q";
                    break;
                case King king:
                    symbol = "K";
                    break;
                default:
                    symbol = "";
                    break;
            }

            return symbol;
        }

        /// <summary>
        /// Convert internal representation to SAN annotation (eg. Nf3, Kf1, d4)
        /// </summary>
        /// <returns>string</returns>
        public string ToSan()
        {
            var rank = (_vector.y + 1).ToString();
            var file = new[] {"a", "b", "c", "d", "e", "f", "g", "h"}[_vector.x];
            var symbol = GetPieceAsSymbol();

            return symbol + file + rank;
        }
    }

    public class MovePair
    {
        private Move _white;
        private Move _black;

        public Move White => _white;

        public Move Black => _black;

        /// <summary>
        /// Add half move
        /// </summary>
        /// <param name="move"></param>
        /// <returns>bool</returns>
        public bool AddHalfMove(Move move)
        {
            var success = false;

            if (_white == null)
            {
                _white = move;
                success = true;
            }
            else if (_black == null)
            {
                _black = move;
                success = true;
            }

            return success;
        }
    }

    public enum CastleStatus
    {
        NONE,
        SHORT,
        LONG,
        BOTH
    }

    public class GameData
    {
        private DateTime _date;
        private string _event;
        private int _round;
        private string _site;

        public string White { get; private set; }

        public string Black { get; private set; }

        public GameResult Result { get; private set; }


        public Dictionary<int, MovePair> Moves { get; } = new Dictionary<int, MovePair>();

        #region CustomNonPGN

        private CastleStatus _whiteCastleStatus;

        public CastleStatus WhiteCastleStatus => _whiteCastleStatus;

        private CastleStatus _blackCastleStatus;
        public CastleStatus BlackCastleStatus => _blackCastleStatus;

        #endregion

        public MovePair GetMovePair(int moveNumber)
        {
            return Moves[moveNumber];
        }

        public MovePair GetPreviousMovePair()
        {
            var lastMoveNumber = GetLastMoveNumber();
            return lastMoveNumber > 1 ? GetMovePair(lastMoveNumber - 1) : null;
        }

        public void AddHalfMove(Move move)
        {
            var lastMoveNumber = GetLastMoveNumber();
            if (lastMoveNumber == 0)
            {
                var movePair = new MovePair();
                movePair.AddHalfMove(move);
                Moves.Add(1, movePair);
            }
            else
            {
                var movePair = Moves[lastMoveNumber];
                var success = movePair.AddHalfMove(move);
                if (!success)
                {
                    movePair = new MovePair();
                    movePair.AddHalfMove(move);
                    Moves.Add(lastMoveNumber + 1, movePair);
                }
            }

            Debug.Log(Moves.Last().Key + "." + Moves.Last().Value?.White?.ToSan() + " " +
                      Moves.Last().Value?.Black?.ToSan());
        }

        public int GetLastMoveNumber()
        {
            return (Moves.Count == 0) ? 0 : Moves.Last().Key;
        }

        /// <summary>
        /// Converts result enum to human readable text
        /// </summary>
        /// <returns>string</returns>
        public string GetResultAsText()
        {
            var text = "";
            switch (Result)
            {
                case GameResult.None:
                    text = "";
                    break;
                case GameResult.Win:
                    text = "1 - 0";
                    break;
                case GameResult.Loss:
                    text = "0 - 1";
                    break;
                case GameResult.Draw:
                    text = "1/2";
                    break;
            }

            return text;
        }

        /// <summary>
        /// Parse list of tags from PGN header
        /// </summary>
        public void SetTags(Dictionary<string, string> tags)
        {
            foreach (var tagPair in tags)
            {
                var key = tagPair.Key.ToLower().Trim();
                var val = tagPair.Value.Trim().Trim('"');

                switch (key)
                {
                    case "event":
                        _event = val;
                        break;
                    case "site":
                        _site = val;
                        break;
                    case "white":
                        White = val;
                        break;
                    case "black":
                        Black = val;
                        break;
                    case "date":
                        _date = DateTime.ParseExact(val, "yyyy.MM.dd",
                            CultureInfo.InvariantCulture);
                        break;
                    case "round":
                        if (!int.TryParse(val, out _round))
                        {
                            _round = -1;
                        }

                        break;
                    case "result":
                        switch (tagPair.Value.Trim('"'))
                        {
                            case "1-0":
                                Result = GameResult.Win;
                                break;
                            case "0-1":
                                Result = GameResult.Loss;
                                break;
                            case "1/2-1/2":
                                Result = GameResult.Draw;
                                break;
                            case "*":
                                Result = GameResult.None;
                                break;
                            default:
                                Result = GameResult.None;
                                break;
                        }

                        break;
                }
            }
        }
    }
}