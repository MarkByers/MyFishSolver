﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HeyThatsMyFishSolver;

namespace MyFishSolverTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test1()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 0 B 1 1 R 0
0 0 0 0 0 0 0 0");

            int score = board.Solve();
            Assert.AreEqual(2, score);
        }

        [TestMethod]
        public void Test2()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 B 0 0 0 0 0
0 0 2 1 1 1 R 0
 0 0 0 0 0 0 0
");

            int score = board.Solve();
            Assert.AreEqual(-1, score);
        }

        [TestMethod]
        public void Test3()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 3 1 0 0 0 0 0
0 B 0 R 0 0 0 0
 0 3 1 0 0 0 0 0
0 0 0 0 0 0 3 0
 0 3 B 1 1 R 0 0
0 0 0 0 0 0 0 0
");

            int score = board.Solve();
            Assert.AreEqual(1, score);
        }

        [TestMethod]
        public void Test4()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 1 0 0 0 0 0 0
0 2 B 0 0 0 0 0
 0 1 R 0 0 0 0 0
0 0 0 0 0 0 0 0
 0 0 B 1 R 0 0 0
0 0 0 0 0 0 0 0
");

            int score = board.Solve();
            Assert.AreEqual(3, score);
        }

        [TestMethod]
        public void Test5()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 1 R 0 0 0 0 0
0 2 B 1 0 0 0 0
 0 1 2 0 0 0 0 0
0 0 0 0 0 0 0 0
 0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0
");

            int score = board.Solve();
            Assert.AreEqual(2, score);
        }

        [TestMethod]
        public void Test6()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 1 R 0 0 0 0 0
0 2 1 1 B 0 0 0
 0 1 2 0 0 0 0 0
0 0 0 0 0 0 0 0
 0 0 B 1 R 0 0 0
0 0 0 0 0 0 0 0
");

            int score = board.Solve();
            Assert.AreEqual(2, score);
        }

        [TestMethod]
        public void Test7()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 1 1 1 0 0 0 0
0 1 1 R 1 0 0 0
 0 1 1 B 1 0 0 0
0 1 1 1 0 0 0 0
 0 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0
");

            int score = board.Solve();
            Assert.AreEqual(-2, score);
        }

        [TestMethod]
        public void Test8()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 1 1 1 1 0 0 0
0 1 1 R 1 1 0 0
 0 1 1 B 1 1 0 0
0 1 1 1 1 1 0 0
 0 0 0 0 0 0 0
0 0 0 0 0 0 0 0
");

            int score = board.Solve();
            Assert.AreEqual(1, score);
        }

        private Board ParseBoard(string p)
        {
            p = p.Trim().Replace(" ", "");
            Board board = new Board();
            int row = 0;
            foreach (string rowString in p.Split('\n'))
            {
                int column = 0;
                foreach (char c in rowString)
                {
                    if (c >= '1' && c <= '3')
                    {
                        board.Fish[row, column] = c - '0';
                    }
                    else if (c == 'B')
                    {
                        board.Blue.Add(new Position(row, column));
                    }
                    else if (c == 'R')
                    {
                        board.Red.Add(new Position(row, column));
                    }
                    column++;
                }
                row++;
            }
            return board;
        }
    }

}
