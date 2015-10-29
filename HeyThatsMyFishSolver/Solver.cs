using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeyThatsMyFishSolver
{
    public class Solver
    {
        public int[][] Fish = { new int[10], new int[10], new int[10], new int[10], new int[10], new int[10], new int[10], new int[10], new int[10], new int[10] };
        public int Score { get; set; }
        public Position[] Blue = new Position[0];
        public Position[] Red = new Position[0];

        /// <summary>
        /// Negamax with alpha beta pruning.
        /// https://en.wikipedia.org/wiki/Negamax#Negamax_with_alpha_beta_pruning
        /// </summary>
        public int Solve(int alpha = -10000, int beta = 10000, bool opponentSkipped = false)
        {
            int bestScore = int.MinValue;
            Move bestMove;

            List<Move> moves = GetAvailableMoves().ToList();

            if (moves.Count == 0) {
                if (opponentSkipped) { return Score; }
                else
                {
                    SwapSides();
                    int result = -Solve(-beta, -alpha, true);
                    SwapSides();
                    return result;
                }
            }

            foreach (Move move in moves)
            {
                int newScore = EvaluateMove(move, alpha, beta);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestMove = move;

                    if (newScore > alpha)
                    {
                        alpha = newScore;
                        if (alpha >= beta) { break; }
                    }
                }
            }

            return bestScore;
        }

        private int EvaluateMove(Move move, int alpha = -10000, int beta = 10000)
        {
            int penguin = move.Penguin;
            Position source = move.Source;
            Position target = move.Target;

            // Play the move.
            int fish = Fish[target.Row][target.Column];
            Score += fish;
            Fish[target.Row][target.Column] = 0;
            Blue[penguin] = target;

            SwapSides();
            int score = -Solve(-beta, -alpha);
            SwapSides();

            // Undo the move.
            Blue[penguin] = source;
            Fish[target.Row][target.Column] = fish;
            Score -= fish;

            return score;
        }

        public void SwapSides()
        {
            var temp = Blue;
            Blue = Red;
            Red = temp;
            Score = -Score;
        }

        public IEnumerable<MoveScore> GetMoveScores()
        {
            return GetAvailableMoves()
                .Select(move => new MoveScore { Move = move, Score = EvaluateMove(move) });
        }

        public IEnumerable<Position> GetDeadPenguins()
        {
            IEnumerable<Move> moves = GetAvailableMoves();
            var livePenguins = moves.Select(m => m.Penguin).Distinct().ToDictionary(x => x);
            for (int penguin = 0; penguin < Blue.Length; ++penguin)
            {
                if (!livePenguins.ContainsKey(penguin))
                {
                    yield return Blue[penguin];
                }
            }
        }

        public IEnumerable<Move> GetAvailableMoves()
        {
            for (int penguin = 0; penguin < Blue.Length; ++penguin)
            {
                // Right
                int row = Blue[penguin].Row;
                int column = Blue[penguin].Column;
                while (Fish[row][column + 1] > 0)
                {
                    column++;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Left
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row][column - 1] > 0)
                {
                    column--;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Down+right
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row + 1][column + (row % 2)] > 0)
                {
                    column += (row % 2);
                    row += 1;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Down+left
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row + 1][column + (row % 2) - 1] > 0)
                {
                    column += (row % 2) - 1;
                    row += 1;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Up+right
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row - 1][column + (row % 2)] > 0)
                {
                    column += (row % 2);
                    row -= 1;
                    yield return new Move { Penguin = penguin, Source = Blue[penguin], Target = new Position(row, column) };
                }

                // Up+left
                row = Blue[penguin].Row;
                column = Blue[penguin].Column;
                while (Fish[row - 1][column + (row % 2) - 1] > 0)
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
                                Fish[row][column].ToString();
                    sb.Append(c + " ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    public class Position
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

    public class Move
    {
        public int Penguin { get; set; }
        public Position Source { get; set; }
        public Position Target { get; set; }

        public override string ToString()
        {
            return string.Format("{1}->{2}", Penguin, Source, Target);
        }
    }
}
