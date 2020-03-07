using System.Collections;
using System.Linq;
using System.Text;
using ChessBoard;
using UnityEngine;

namespace MyChess
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public enum GameResult
    {
        None,
        Win,
        Draw,
        Loss
    }

    public class MovePair
    {
        private string _white;
        private Vector2Int _whiteVec;
        private string _black;
        private Vector2Int _blackVec;

        public string White => _white;

        public string Black => _black;

        private MovePair()
        {
            _white = "";
            _black = "";
        }

        public void AddHalfMove(string move)
        {
            if (String.IsNullOrEmpty(_white))
            {
                _white = move;
            }
            else if (string.IsNullOrEmpty(_black))
            {
                _black = move;
            }
        }

        public static Vector2Int SanToVector(string san)
        {
            Vector2Int vector = new Vector2Int();

            // Pawn
            if (san.Length == 2)
            {
                san = "P" + san;
            }

            vector.x = "abcdefgh".IndexOf(san.Substring(1,1), StringComparison.Ordinal);
            vector.y = int.Parse(san.Substring(2, 1));

            return vector;
        }

        public static string VectorToSan(Vector2Int vector, IPiece piece)
        {
            string san;

            var rank = (vector.y + 1).ToString();
            var file = new[] {"a", "b", "c", "d", "e", "f", "g", "h"}[vector.x];

            switch (piece)
            {
                case Pawn pawn:
                    san = "";
                    break;
                case Rook rook:
                    san = "R";
                    break;
                case Knight knight:
                    san = "N";
                    break;
                case Bishop bishop:
                    san = "B";
                    break;
                case Queen queen:
                    san = "Q";
                    break;
                case King king:
                    san = "K";
                    break;
                default:
                    san = "";
                    break;
            }

            return san + file + rank;
        }
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

        public MovePair GetMovePair(int moveNumber)
        {
            return Moves[moveNumber];
        }

        public void SetMovePair(int moveNumber, MovePair pair)
        {
            Moves[moveNumber] = pair;
        }

        public int GetLastMoveNumber()
        {
            return (Moves.Count == 0) ? 0 : Moves.Last().Key;
        }

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

        /**
     * Parse list of tags from PGN header
     */
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