﻿using UnityEngine;
using UnityEngine.UI;

namespace ChessBoard
{
    public class Bishop : Piece
    {
        public override void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
        {
            base.Setup(newTeamColor, newSpriteColor, newPieceManager);

            movement = new Vector3Int(0, 0, 7);
            GetComponent<Image>().sprite = Resources.Load<Sprite>("Bishop");
        }

        protected override void Move()
        {
            base.Move();
        }
    }
}