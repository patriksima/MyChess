﻿using ChessBoard;
using MyChess;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;

namespace Tests
{
    public class AnnotationTest
    {
        /// <summary>
        /// Try add one half move to GameData 1. d4
        /// </summary>
        [Test]
        public void WhitePawnOneOneStep()
        {
            ICell cell = Substitute.For<ICell>();
            cell.BoardPosition = new Vector2Int(1,4);
            cell.CurrentPiece = Substitute.For<Pawn>();
            //cell.Board.AllCells;
            
            
            string san = AnnotationEngine.Instance.ToSan(cell, new Vector2Int(), null, Color.white, CastleStatus.NONE);
            
            Assert.AreEqual("d4", san);
        }
    }
}
