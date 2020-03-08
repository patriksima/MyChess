using System;
using MyChess;
using UnityEngine;

namespace ChessBoard
{
    public class AnnotationEngine
    {
        // get SAN annotation from move
        // - check castle
        // - check and mate
        // - check enpassant
        // - check capturing
        // - check multiple choice (eg Jbd7, R1d3) http://www.saremba.de/chessgml/standards/pgn/pgn-complete.htm#AEN242
        // - promotions fxg1=Q+, g1=N etc.
        private static AnnotationEngine _instance;

        public static AnnotationEngine Instance => _instance ?? (_instance = new AnnotationEngine());

        private ICell _currCell;
        private Vector2Int _prevPos;
        private CastleStatus _castleStatus;
        private Color _teamColor;
        private IPiece _targetPiece;


        // inputs
        // - previous piece state
        // - current piece state
        // - previous cell Piece
        // - piece team color (white, black)
        // - player castling status (if player have castled short or long or none)
        // - board - for checking ambiguity (multiple choice)
        public string ToSan(ICell currCell, Vector2Int prevPos, IPiece targetPiece, Color teamColor,
            CastleStatus castleStatus)
        {
            _currCell = currCell;
            _prevPos = prevPos;
            _castleStatus = castleStatus;
            _teamColor = teamColor;
            _targetPiece = targetPiece;


            if (IsCastleShort() && castleStatus == CastleStatus.NONE)
            {
                return "0-0";
            }

            if (IsCastleLong() && castleStatus == CastleStatus.NONE)
            {
                return "0-0-0";
            }

            string san = "";
            if (IsCapturing())
            {
                // eg. dxc4
                if (currCell.CurrentPiece is Pawn)
                {
                    var f = new[] {"a", "b", "c", "d", "e", "f", "g", "h"}[_prevPos.x];
                    var t = new[] {"a", "b", "c", "d", "e", "f", "g", "h"}[_currCell.BoardPosition.x];
                    var r = _currCell.BoardPosition.y + 1;
                    san = f + "x" + t + r;
                }
                else
                {
                    var f = GetPieceAsSymbol(_currCell.CurrentPiece);
                    var t = new[] {"a", "b", "c", "d", "e", "f", "g", "h"}[_currCell.BoardPosition.x];
                    var r = _currCell.BoardPosition.y + 1;
                    san = f + "x" + t + r;
                }
            }
            else
            {
                var f = GetPieceAsSymbol(_currCell.CurrentPiece);
                var t = new[] {"a", "b", "c", "d", "e", "f", "g", "h"}[_currCell.BoardPosition.x];
                var r = _currCell.BoardPosition.y + 1;
                san = f + t + r;
            }

            if (IsKingInCheck())
            {
                san += "+";
            }

            return san;
        }

        private bool IsKingInCheck()
        {
            switch (_currCell.CurrentPiece)
            {
                case Pawn pawn:
                    if (_teamColor == Color.white)
                    {
                        Cell tLeft = null;
                        var tLeftPos = new Vector2Int(_currCell.BoardPosition.x - 1, _currCell.BoardPosition.y + 1);

                        if (IsValidPosition(tLeftPos))
                        {
                            tLeft = _currCell.Board.AllCells[tLeftPos.x, tLeftPos.y];
                        }

                        Cell tRight = null;
                        var tRightPos = new Vector2Int(_currCell.BoardPosition.x + 1, _currCell.BoardPosition.y + 1);

                        if (IsValidPosition(tRightPos))
                        {
                            tRight = _currCell.Board.AllCells[tRightPos.x, tRightPos.y];
                        }

                        if ((tLeft != null && tLeft.CurrentPiece is King) ||
                            (tRight != null && tRight.CurrentPiece is King))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        Cell tLeft = null;
                        var tLeftPos = new Vector2Int(_currCell.BoardPosition.x - 1, _currCell.BoardPosition.y - 1);

                        if (IsValidPosition(tLeftPos))
                        {
                            tLeft = _currCell.Board.AllCells[tLeftPos.x, tLeftPos.y];
                        }

                        Cell tRight = null;
                        var tRightPos = new Vector2Int(_currCell.BoardPosition.x + 1, _currCell.BoardPosition.y - 1);

                        if (IsValidPosition(tRightPos))
                        {
                            tRight = _currCell.Board.AllCells[tRightPos.x, tRightPos.y];
                        }

                        if ((tLeft != null && tLeft.CurrentPiece is King) ||
                            (tRight != null && tRight.CurrentPiece is King))
                        {
                            return true;
                        }
                    }

                    break;
                case Rook rook:
                    // left
                    var tLeftPos2 = new Vector2Int(_currCell.BoardPosition.x - 1, _currCell.BoardPosition.y);
                    if (IsValidPosition(tLeftPos2))
                    {
                        for (var x = tLeftPos2.x; x >= 0; x--)
                        {
                            var cell = _currCell.Board.AllCells[x, _currCell.BoardPosition.y];
                            if (cell.CurrentPiece != null)
                            {
                                return cell.CurrentPiece is King;
                            }
                        }
                    }

                    break;
                case Knight knight:

                    break;
                case Bishop bishop:

                    break;
                case Queen queen:

                    break;
                case King king:

                    break;
                default:

                    break;
            }

            return false;
        }

        private bool IsCapturing()
        {
            return (!(_targetPiece is null));
        }

        private bool IsCastleShort()
        {
            //Debug.Log("Current piece: " + _currCell.CurrentPiece.GetType());
            //Debug.Log("Color: " + _teamColor+"; prev: " + _prevPos.x + ", " + _prevPos.y+"; curr: " + _currCell.BoardPosition.x + ", " + _currCell.BoardPosition.y);
            if (_currCell.CurrentPiece is King)
            {
                return (_teamColor == Color.white && _currCell.BoardPosition.y == 0 && _prevPos.y == 0 &&
                        _prevPos.x == 4 && _currCell.BoardPosition.x == 6)
                       || (_teamColor == Color.black && _currCell.BoardPosition.y == 7 &&
                           _prevPos.y == 7 && _prevPos.x == 4 &&
                           _currCell.BoardPosition.x == 6);
            }

            return false;
        }

        private bool IsCastleLong()
        {
            //Debug.Log("Current piece: " + _currCell.CurrentPiece.GetType());
            //Debug.Log("Color: " + _teamColor+"; prev: " + _prevPos.x + ", " + _prevPos.y+"; curr: " + _currCell.BoardPosition.x + ", " + _currCell.BoardPosition.y);
            if (_currCell.CurrentPiece is King)
            {
                return (_teamColor == Color.white && _currCell.BoardPosition.y == 0 && _prevPos.y == 0 &&
                        _prevPos.x == 4 && _currCell.BoardPosition.x == 2)
                       || (_teamColor == Color.black && _currCell.BoardPosition.y == 7 &&
                           _prevPos.y == 7 && _prevPos.x == 4 &&
                           _currCell.BoardPosition.x == 2);
            }

            return false;
        }

        #region Helpers

        private bool IsValidPosition(Vector2Int position)
        {
            return (position.x >= 0 && position.x < 8 && position.y >= 0 && position.y < 8);
        }

        private string GetPieceAsSymbol(IPiece piece)
        {
            string symbol;

            switch (piece)
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

        #endregion
    }
}