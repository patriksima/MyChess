using ChessBoard;
using MyChess;
using NUnit.Framework;
using UnityEngine;
using NSubstitute;

namespace Tests
{
    public class GameDataTest
    {
        [Test]
        public void ConvertVectorPieceToSanTest()
        {
            var piece = NSubstitute.Substitute.For<IPiece>();
            var vector = new Vector2Int(3, 3);
            var san = MovePair.VectorToSan(vector, piece);
            Assert.AreEqual("d4", san);
        }
    }
}