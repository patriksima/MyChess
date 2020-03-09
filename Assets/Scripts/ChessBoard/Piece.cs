using System.Collections.Generic;
using MyChess;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ChessBoard
{
    public interface IPiece
    {
        // dummy interface
    }

    public abstract class Piece : EventTrigger, IPiece
    {
        protected Cell currentCell;
        protected List<Cell> highlightedCells = new List<Cell>();
        protected bool isFirstMove = true;

        protected Vector3Int movement = Vector3Int.one;
        protected Cell originalCell;
        protected PieceManager pieceManager;
        protected RectTransform rect;
        protected Cell targetCell;

        public Color TeamColor { get; set; }

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

        /// <summary>
        /// Place a piece into a cell
        /// </summary>
        /// <param name="newCell"></param>
        public virtual void Place(Cell newCell)
        {
            currentCell = newCell;
            originalCell = newCell;
            currentCell.SetPiece(this);
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

                var cellState = currentCell.Board.GetCellState(currX, currY, this);

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
                if (RectTransformUtility.RectangleContainsScreenPoint(cell.CellRectTransform, Input.mousePosition))
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
            //currentCell.RemovePiece();
            gameObject.SetActive(false);
        }

        protected virtual void Move()
        {
            var prevCell = currentCell;
            var targetPiece = targetCell.CurrentPiece;

            targetCell.KillPiece();
            targetCell.RemovePiece();
            currentCell.RemovePiece();

            currentCell = targetCell;
            currentCell.SetPiece(this);
            gameObject.SetActive(true);

            transform.position = currentCell.transform.position;

            targetCell = null;
            isFirstMove = false;


            // GameManager.Instance.GameData.AddHalfMove(new Move(currentCell.BoardPosition, currentCell.CurrentPiece));

            Debug.Log(
                AnnotationEngine.Instance.ToSan(currentCell, prevCell.BoardPosition, targetPiece, TeamColor,
                    CastleStatus.NONE));
        }
    }
}