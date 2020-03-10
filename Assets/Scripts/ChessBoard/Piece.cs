using System;
using System.Collections.Generic;
using MyChess;
using NSubstitute;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessBoard
{
    public abstract class Piece : EventTrigger
    {
        #region Events

        public static event Action<Piece> OnPieceMoved;

        #endregion

        #region Chess Data

        
        public bool isFirstMove = true;
        public Color TeamColor { get; private set; }
        public Move LastMove { get; private set; }
        
        #endregion

        private Cell _currentCell;

        public Cell CurrentCell => _currentCell;

        protected List<Cell> highlightedCells = new List<Cell>();

        protected Vector3Int movement = Vector3Int.one;
        private Cell _originalCell;
        private PieceManager _pieceManager;
        protected Cell targetCell;


        public virtual void Setup(Color teamColor, Color32 imageColor, PieceManager pieceManager)
        {
            _pieceManager = pieceManager;
            TeamColor = teamColor;
            GetComponent<Image>().color = imageColor;
        }

        /// <summary>
        /// Attach a piece to a cell
        /// </summary>
        /// <param name="cell"></param>
        public virtual void AttachToCell(Cell cell)
        {
            _currentCell = cell;
            _originalCell = cell;
            _currentCell.SetPiece(this);
            transform.position = cell.transform.position;
            gameObject.SetActive(true);
            LastMove = new Move(_currentCell.BoardPosition,_currentCell.BoardPosition, this);
        }

        private void CreateCellPath(int xDir, int yDir, int move)
        {
            var currX = _currentCell.BoardPosition.x;
            var currY = _currentCell.BoardPosition.y;

            for (var i = 1; i <= move; i++)
            {
                currX += xDir;
                currY += yDir;

                var cellState = _currentCell.Board.GetCellState(currX, currY, this);

                if (cellState == CellState.Enemy)
                {
                    highlightedCells.Add(_currentCell.Board.AllCells[currX, currY]);
                    break;
                }

                if (cellState != CellState.Free)
                {
                    break;
                }

                highlightedCells.Add(_currentCell.Board.AllCells[currX, currY]);
            }
        }

        protected virtual void CheckPathing()
        {
            CreateCellPath(1, 0, movement.x);
            CreateCellPath(-1, 0, movement.x);

            CreateCellPath(0, 1, movement.y);
            CreateCellPath(0, -1, movement.y);

            CreateCellPath(1, 1, movement.z);
            CreateCellPath(-1, 1, movement.z);

            CreateCellPath(-1, -1, movement.z);
            CreateCellPath(1, -1, movement.z);
        }

        public void Reset()
        {
            Kill();
            AttachToCell(_originalCell);
        }

        protected void ShowCells()
        {
            foreach (var cell in highlightedCells)
            {
                cell.outlineAbility.On();
            }
        }

        protected void ClearCells()
        {
            foreach (var cell in highlightedCells)
            {
                cell.outlineAbility.Off();
            }

            highlightedCells.Clear();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            CheckPathing();

            ShowCells();
        }

        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            transform.position += (Vector3) eventData.delta;

            foreach (var cell in highlightedCells)
            {
                if (cell.IsPointInside(Input.mousePosition))
                {
                    targetCell = cell;
                    break;
                }

                targetCell = null;
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            ClearCells();

            if (!targetCell)
            {
                transform.position = _currentCell.gameObject.transform.position;
                return;
            }

            Move();

            _pieceManager.ChangeSide(TeamColor);
        }

        public virtual void Kill()
        {
            gameObject.SetActive(false);
        }

        protected virtual void Move()
        {
            var prevCell = _currentCell;
            var targetPiece = targetCell.CurrentPiece;

            targetCell.KillPiece();
            targetCell.RemovePiece();
            _currentCell.RemovePiece();

            _currentCell = targetCell;
            _currentCell.SetPiece(this);
            gameObject.SetActive(true);

            transform.position = _currentCell.transform.position;

            targetCell = null;
            isFirstMove = false;

            LastMove = new Move(prevCell.BoardPosition, _currentCell.BoardPosition, this);


            // Send message about movement
            OnPieceMoved?.Invoke(this);

            // GameManager.Instance.GameData.AddHalfMove(new Move(currentCell.BoardPosition, currentCell.CurrentPiece));

            Debug.Log(
                AnnotationEngine.Instance.ToSan(_currentCell, prevCell.BoardPosition, targetPiece, TeamColor,
                    CastleStatus.NONE));
        }
    }
}