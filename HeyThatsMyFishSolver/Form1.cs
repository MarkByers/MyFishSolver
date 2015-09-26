using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HeyThatsMyFishSolver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Tests();
        }

        private void Tests()
        {
            Test1();
            Test2();
            Test3();
            Test4();
            Test5();
            Test6();
            Test7();
        }

        private void Test1()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 0 B 1 1 R 0
0 0 0 0 0 0 0 0");

            int score = board.Solve();
            if (score != 2) { throw new Exception("Test failed"); }
        }

        private void Test2()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 B 0 0 0 0 0
0 0 2 1 1 1 R 0
 0 0 0 0 0 0 0
");

            int score = board.Solve();
            if (score != -1) { throw new Exception("Test failed"); }
        }

        private void Test3()
        {
            Board board = ParseBoard(@"
0 0 0 0 0 0 0 0
 0 2 1 0 0 0 0 0
0 B 0 R 0 0 0 0
 0 2 1 0 0 0 0 0
0 0 0 0 0 0 3 0
 0 3 B 1 1 R 0 0
0 0 0 0 0 0 0 0
");

            int score = board.Solve();
            if (score != 1) { throw new Exception("Test failed"); }
        }

        private void Test4()
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
            if (score != 3) { throw new Exception("Test failed"); }
        }

        private void Test5()
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
            if (score != 2) { throw new Exception("Test failed"); }
        }

        private void Test6()
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
            if (score != 2) { throw new Exception("Test failed"); }
        }

        private void Test7()
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
            if (score != -2) { throw new Exception("Test failed"); }
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
