using System.Collections.Generic;
using UnityEngine;

namespace ChessBoard
{
    public class RulesEngine
    {
        // check if move is valid
        // check is mate
        // check is check
        // get possible moves

        // need Board, Cellstates
        // last opponent move
        // last player move

        // getpawnpossiblemoves
        // - basic line - one or two
        // - if enemy ahead cant move
        // - if enemy diagonal can capture
        // - if on 4th/5th rank and last enemy move was besides can capture en-passant
        // - if 1st or 8th rank can promote to rook, knight, bishop, queen
        public List<Cell> GetPawnPossibleMoves(Cell currentCell, Cell lastOpponentMoveCell)
        {
            var possibleCells = new List<Cell>();

            var board = currentCell.Board;
            var direction = currentCell.CurrentPiece.TeamColor == Color.white
                ? new Vector3Int(0, 1, 1)
                : new Vector3Int(0, -1, 1);

            // check ahead
            var ahead = board.GetCellState(currentCell.BoardPosition.x, currentCell.BoardPosition.y + direction.y,
                currentCell.CurrentPiece);
            var upperLeft = board.GetCellState(currentCell.BoardPosition.x - 1,
                currentCell.BoardPosition.y + direction.y, currentCell.CurrentPiece);
            var upperRight = board.GetCellState(currentCell.BoardPosition.x + 1,
                currentCell.BoardPosition.y + direction.y, currentCell.CurrentPiece);


            if (ahead == CellState.Free)
            {
                possibleCells.Add(
                    board.AllCells[currentCell.BoardPosition.x, currentCell.BoardPosition.y + direction.y]);
            }

            if (lastOpponentMoveCell.CurrentPiece is Pawn)
            {
                var lastOpponentMoveCurrentPosition = lastOpponentMoveCell.CurrentPiece.LastMove.CurrentPosition;
                var lastOpponentMovePreviousPosition = lastOpponentMoveCell.CurrentPiece.LastMove.PreviousPosition;
                var lastPlayerMoveCurrentPosition = currentCell.BoardPosition;

                if (lastOpponentMoveCurrentPosition.y == lastPlayerMoveCurrentPosition.y
                    && lastOpponentMovePreviousPosition.y == lastOpponentMoveCurrentPosition.y - 2 * direction.y
                    && (lastOpponentMoveCurrentPosition.x == lastPlayerMoveCurrentPosition.x - 1 ||
                        lastOpponentMoveCurrentPosition.x == lastPlayerMoveCurrentPosition.x + 1))
                {
                    possibleCells.Add(
                        board.AllCells[currentCell.BoardPosition.x - 1, currentCell.BoardPosition.y]);
                    possibleCells.Add(
                        board.AllCells[currentCell.BoardPosition.x + 1, currentCell.BoardPosition.y]);
                }
            }

            return possibleCells;
        }
    }
}