using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessBoard
{
    public abstract class Piece : EventTrigger
    {
        protected Cell currentCell;
        protected List<Cell> highlightedCells = new List<Cell>();
        protected bool isCastling;
        protected bool isFirstMove = true;

        protected Vector3Int movement = Vector3Int.one;
        protected Cell originalCell;
        protected PieceManager pieceManager;
        protected RectTransform rect;
        protected Cell targetCell;

        public Color TeamColor { get; set; } = Color.clear;

        public bool IsFirstMove
        {
            get => isFirstMove;
            set => isFirstMove = value;
        }

        public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
        {
            pieceManager = newPieceManager;
            TeamColor = newTeamColor;
            GetComponent<Image>().color = newSpriteColor;
            rect = GetComponent<RectTransform>();
        }

        public virtual void Place(Cell newCell)
        {
            currentCell = newCell;
            originalCell = newCell;
            currentCell.CurrentPiece = this;

            transform.position = newCell.transform.position;
            gameObject.SetActive(true);
        }

        private void CreateCellPath(int xDir, int yDir, int move)
        {
            var currX = currentCell.BoardPosition.x;
            var currY = currentCell.BoardPosition.y;

            for (var i = 1; i <= move; i++)
            {
                currX += xDir;
                currY += yDir;

                var cellState = currentCell.Board.ValidateCell(currX, currY, this);

                if (cellState == CellState.Enemy)
                {
                    highlightedCells.Add(currentCell.Board.AllCells[currX, currY]);
                    break;
                }

                if (cellState != CellState.Free)
                {
                    break;
                }

                highlightedCells.Add(currentCell.Board.AllCells[currX, currY]);
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
            Place(originalCell);
        }

        protected void ShowCells()
        {
            foreach (var cell in highlightedCells)
            {
                cell.OutlineImage.enabled = true;
            }
        }

        protected void ClearCells()
        {
            foreach (var cell in highlightedCells)
            {
                cell.OutlineImage.enabled = false;
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
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.RectTransform, Input.mousePosition))
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
                transform.position = currentCell.gameObject.transform.position;
                return;
            }

            Move();

            pieceManager.SwitchSides(TeamColor);
        }

        public virtual void Kill()
        {
            currentCell.CurrentPiece = null;
            gameObject.SetActive(false);
        }

        protected virtual void Move()
        {
            SanMove(currentCell, targetCell);

            targetCell.RemovePiece();
            currentCell.CurrentPiece = null;
            currentCell = targetCell;
            currentCell.CurrentPiece = this;
            transform.position = currentCell.transform.position;
            targetCell = null;
            isFirstMove = false;
        }

        protected void SanMove(Cell previous, Cell current)
        {
            Debug.Log("isCastling: " + isCastling + ", Piece prev: " + previous.CurrentPiece.GetType() +
                      ", Type caller: " + GetType());

            if (previous.CurrentPiece is King && current.BoardPosition.x - previous.BoardPosition.x == 2)
            {
                Debug.Log("Castle short 0-0");
                isCastling = true;
                Debug.Log(isCastling);
                return;
            }

            if (previous.CurrentPiece is King && current.BoardPosition.x - previous.BoardPosition.x == -2)
            {
                isCastling = true;
                Debug.Log("Castle long 0-0-0");
                return;
            }
/*
        if (isCastling && previous.CurrentPiece is Rook)
        {
            isCastling = false;
            return;
        }*/

            string[] ranks = {"A", "B", "C", "D", "E", "F", "G", "H"};
            var file = (current.BoardPosition.y + 1).ToString();
            var rank = ranks[current.BoardPosition.x];
            var piece = "";

            switch (previous.CurrentPiece.GetType().Name)
            {
                case "Pawn":
                    piece = "";
                    break;
                case "Rook":
                    piece = "R";
                    break;
                case "Knight":
                    piece = "N";
                    break;
                case "Bishop":
                    piece = "B";
                    break;
                case "Queen":
                    piece = "Q";
                    break;
                case "King":
                    piece = "K";
                    break;
                default:
                    piece = "";
                    break;
            }

            Debug.Log("Move: " + piece + rank + file);
        }
    }
}