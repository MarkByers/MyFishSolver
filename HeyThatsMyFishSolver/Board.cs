using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeyThatsMyFishSolver
{
    class Board
    {
        public int[,] Fish = new int[8, 8];
        public int Score { get; set; }
        public List<Position> Blue = new List<Position>();
        public List<Position> Red = new List<Position>();

        public int Solve(bool opponentSkipped = false)
        {
            int bestScore = int.MinValue;
            int bestPenguin = 0;
            Position bestTarget;

            List<Move> moves = GetAvailableMoves().ToList();

            if (moves.Count == 0) {
                if (opponentSkipped) { return Score; }
                else
                {
                    SwapSides();
                    int result = -Solve(true);
                    SwapSides();
                    return result;
                }
            }

            foreach (Move move in moves)
            {
                int penguin = move.Penguin;
                Position source = move.Source;
                Position target = move.Target;

                // Play the move.
                int fish = Fish[target.Row, target.Column];
                Score += fish;
                Fish[target.Row, target.Column] = 0;
                Blue[penguin] = target;

                SwapSides();

                int newScore = -Solve();
                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestPenguin = penguin;
                    bestTarget = target;
                }

                SwapSides();

                Blue[penguin] = source;
                Fish[target.Row, target.Column] = fish;
                Score -= fish;
            }

            return bestScore;
        }

        private void SwapSides()
        {
            List<Position> temp = Blue;
            Blue = Red;
            Red = temp;
            Score = -Score;
        }

        private IEnumerable<Move> GetAvailableMoves()
        {
            for (int penguin = 0; penguin < Blue.Count; ++penguin)
            {
                // Right
                int row = Blue[penguin].Row;
                int column = Blue[penguin].Column;
                while (Fish[row, column + 1] > 0)
                {
                    column++;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Left
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row, column - 1] > 0)
                {
                    column--;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Down+right
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row + 1, column + (row % 2)] > 0)
                {
                    column += (row % 2);
                    row += 1;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Down+left
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row + 1, column + (row % 2) - 1] > 0)
                {
                    column += (row % 2) - 1;
                    row += 1;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Up+right
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row - 1, column + (row % 2)] > 0)
                {
                    column += (row % 2);
                    row -= 1;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Up+left
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row - 1, column + (row % 2) - 1] > 0)
                {
                    column += (row % 2) - 1;
                    row -= 1;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < 8; ++row)
            {
                if (row % 2 == 1)
                {
                    sb.Append(" ");
                }
                for (int column = 0; column < 8; ++column)
                {
                    string c =  Blue.Any(p => p.Column == column && p.Row == row) ? "B" :
                                Red.Any(p => p.Column == column && p.Row == row) ? "R":
                                Fish[row, column].ToString();
                    sb.Append(c + " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    class Position
    {
        public int Row { get; private set; }
        public int Column { get; private set; }

        public Position(int row, int column)
        {
            this.Row = row;
            this.Column = column;
        }

        public override string ToString()
        {
            return string.Format("({0},{1})", Row, Column);
        }
    }

    class Move
    {
        public int Penguin { get; set; }
        public Position Source { get; set; }
        public Position Target { get; set; }

        public override string ToString()
        {
            return string.Format("P{0} {1}->{2})", Penguin, Source, Target);
        }
    }
}
