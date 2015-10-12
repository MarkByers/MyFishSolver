using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HeyThatsMyFishSolver;
using HeyThatsMyFishWpf.Model;

namespace HeyThatsMyFishWpf.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public RelayCommand SolveCommand { get; set; }

        public MainViewModel()
        {
            SolveCommand = new RelayCommand(Solve);
            CreateBoard();
        }

        private void CreateBoard()
        {
            List<Tile> tiles = ParseBoard(@"
 3 1 0 0 0
1 0 1 0 0 0
 3 1 0 0 0
0 0 0 0 0 3
 3 1 1 1 1
");
            Board = new Board
            {
                Tiles = tiles,
                BluePenguins = new List<Position> { new Position(2, 1), new Position(5, 2) },
                RedPenguins = new List<Position> { new Position(2, 3), new Position(5, 5) }
            };
        }

        private List<Tile> ParseBoard(string p)
        {
            p = p.Trim().Replace(" ", "");
            List<Tile> tiles = new List<Tile>();
            int row = 1;
            foreach (string rowString in p.Split('\n'))
            {
                int column = 1;
                foreach (char c in rowString)
                {
                    if (c >= '1' && c <= '3')
                    {
                        tiles.Add(new Tile { Row = row, Column = column, Fish = c - '0' });
                    }
                    else if (c == 'B')
                    {
                        //board.Blue.Add(new Position(row, column));
                    }
                    else if (c == 'R')
                    {
                        //board.Red.Add(new Position(row, column));
                    }
                    column++;
                }
                row++;
            }
            return tiles;
        }

        private void Solve()
        {
            Solver solver = new Solver();
            foreach (Tile tile in Board.Tiles)
            {
                solver.Fish[tile.Row, tile.Column] = tile.Fish;
            }
            solver.Blue = Board.BluePenguins;
            solver.Red = Board.RedPenguins;

            foreach (Position penguin in solver.Blue.Concat(solver.Red))
            {
                solver.Fish[penguin.Row, penguin.Column] = 0;
            }

            MoveScores = solver.GetMoveScores();
        }

        public Board Board { get; set; }

        public void UnhighlightMove(Move move)
        {
            Tile source = GetTile(move.Source);
            source.IsHighlighted = false;
            Tile target = GetTile(move.Target);
            target.IsHighlighted = false;
        }

        public void HighlightMove(Move move)
        {
            Tile source = GetTile(move.Source);
            source.IsHighlighted = true;
            Tile target = GetTile(move.Target);
            target.IsHighlighted = true;
        }

        private Tile GetTile(Position position)
        {
            return Board.Tiles.Single(t => t.Row == position.Row && t.Column == position.Column);
        }

        #region List<MoveScore> MoveScores

        /// <summary>
        /// The <see cref="MoveScores" /> property's name.
        /// </summary>
        public const string MoveScoresPropertyName = "MoveScores";

        private List<MoveScore> _moveScores = new List<MoveScore>();

        /// <summary>
        /// Sets and gets the MoveScores property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<MoveScore> MoveScores
        {
            get
            {
                return _moveScores;
            }
            set
            {
                Set(() => MoveScores, ref _moveScores, value);
            }
        }

        #endregion
    }
}
