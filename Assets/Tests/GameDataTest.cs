using ChessBoard;
using MyChess;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;

namespace Tests
{
    public class GameDataTest
    {
        /// <summary>
        /// Try add one half move to GameData 1. d4
        /// </summary>
        [Test]
        public void AddOneHalfMoveTest()
        {
            var gameData = new GameData();
            var move = new Move(new Vector2Int(3, 3), NSubstitute.Substitute.For<Pawn>());

            gameData.AddHalfMove(move);
            
            Assert.AreEqual(1, gameData.GetLastMoveNumber());
            var movePair = gameData.GetMovePair(1);
            
            Assert.AreEqual("d4", movePair.White.ToSan());
            Assert.IsNull(movePair.Black);
        }
        
        // Try add two half moves to GameData 1. d4, d5
        [Test]
        public void AddTwoHalfMoveTest()
        {
            var gameData = new GameData();
            var moveWhite = new Move(new Vector2Int(3, 3), NSubstitute.Substitute.For<Pawn>());
            var moveBlack = new Move(new Vector2Int(3, 4), NSubstitute.Substitute.For<Pawn>());

            gameData.AddHalfMove(moveWhite);
            gameData.AddHalfMove(moveBlack);

            var lastMoveNumber = gameData.GetLastMoveNumber();
            
            Assert.AreEqual(1, lastMoveNumber);
            var movePair = gameData.GetMovePair(lastMoveNumber);
            
            Assert.AreEqual("d4", movePair.White.ToSan());
            Assert.AreEqual("d5", movePair.Black.ToSan());
        }

        /// <summary>
        /// Try add three moves to GameData 1. d4 d5 2. e4
        /// </summary>
        [Test]
        public void AddThreeHalfMoveTest()
        {
            var gameData = new GameData();
            var moveOne = new Move(new Vector2Int(3, 3), NSubstitute.Substitute.For<Pawn>());
            var moveTwo = new Move(new Vector2Int(3, 4), NSubstitute.Substitute.For<Pawn>());
            var moveThree = new Move(new Vector2Int(4, 3), NSubstitute.Substitute.For<Pawn>());

            gameData.AddHalfMove(moveOne);
            gameData.AddHalfMove(moveTwo);
            gameData.AddHalfMove(moveThree);

            var lastMoveNumber = gameData.GetLastMoveNumber();
            
            Assert.AreEqual(2, lastMoveNumber);

            var movePair1 = gameData.GetMovePair(1);
            Assert.AreEqual("d4", movePair1.White.ToSan());
            Assert.AreEqual("d5", movePair1.Black.ToSan());
            
            var movePair2 = gameData.GetMovePair(2);
            Assert.AreEqual("e4", movePair2.White.ToSan());
            Assert.Null(movePair2.Black);

        }        
    }
}