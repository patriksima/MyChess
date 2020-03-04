﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Piece : EventTrigger
{
    [SerializeField] private Sprite exampleSprite;
    private Color teamColor = Color.clear;

    public Color TeamColor
    {
        get => teamColor;
        set => teamColor = value;
    }

    protected Cell currentCell = null;
    protected Cell originalCell = null;
    protected Cell targetCell = null;
    protected RectTransform rect = null;
    protected PieceManager pieceManager;
    protected bool isFirstMove = true;

    public bool IsFirstMove
    {
        get => isFirstMove;
        set => isFirstMove = value;
    }

    protected Vector3Int movement = Vector3Int.one;
    protected List<Cell> highlightedCells = new List<Cell>();

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        pieceManager = newPieceManager;
        teamColor = newTeamColor;
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
        int currX = currentCell.BoardPosition.x;
        int currY = currentCell.BoardPosition.y;

        for (int i = 1; i <= move; i++)
        {
            currX += xDir;
            currY += yDir;

            CellState cellState = CellState.None;
            cellState = currentCell.Board.ValidateCell(currX, currY, this);

            if (cellState == CellState.Enemy)
            {
                highlightedCells.Add(currentCell.Board.AllCells[currX, currY]);
                break;
            }

            if (cellState != CellState.Free)
                break;

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
        foreach (Cell cell in highlightedCells)
        {
            cell.OutlineImage.enabled = true;
        }
    }

    protected void ClearCells()
    {
        foreach (Cell cell in highlightedCells)
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

        pieceManager.SwitchSides(teamColor);
    }

    public virtual void Kill()
    {
        currentCell.CurrentPiece = null;
        gameObject.SetActive(false);
    }

    protected virtual void Move()
    {
        targetCell.RemovePiece();
        currentCell.CurrentPiece = null;
        currentCell = targetCell;
        currentCell.CurrentPiece = this;
        transform.position = currentCell.transform.position;
        targetCell = null;
        isFirstMove = false;
    }
}