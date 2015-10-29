using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HeyThatsMyFishSolver;
using FishSolverHtml.ViewModel;
using System.ComponentModel;
using FishSolverHtml.Helpers;

namespace FishSolverHtml.Model
{
    public class Board : ObservableObject
    {
        #region int RedScore
        
        private int _redScore = 0;

        public int RedScore
        {
            get
            {
                return _redScore;
            }
            set
            {
                if (_redScore != value)
                {
                    _redScore = value;
                    NotifyPropertyChanged("RedScore");
                }
            }
        }

        #endregion

        #region int BlueScore

        private int _blueScore = 0;

        /// <summary>
        /// Sets and gets the BlueScore property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int BlueScore
        {
            get
            {
                return _blueScore;
            }
            set
            {
                if (_blueScore != value)
                {
                    _blueScore = value;
                    NotifyPropertyChanged("BlueScore");
                }
            }
        }

        #endregion

        public ObservableCollection<Tile> Tiles { get; set; }

        private Tile selectedPenguin;

        public void SelectTile(Tile tile)
        {
            if (tile.Penguin == 1)
            {
                DeselectPenguin();
                selectedPenguin = tile;
                tile.IsSelected = true;
            }
            else if (selectedPenguin != null /*tile.CanMoveToHere*/)
            {
                selectedPenguin.Penguin = 0;
                Tiles.Remove(selectedPenguin);
                DeselectPenguin();
                tile.Penguin = 1;
                BlueScore += tile.Fish;

                // Computer's turn!
                PlayComputerMove();

                RemoveDeadPenguins();

                // If neither player can move, end the game.

                // TODO: Remove tiles that are not connected to any other tile.

                // TODO: Remove solve results.
                
            }
            else
            {
                DeselectPenguin();
            }
        }

        private void PlayComputerMove()
        {
            while (true)
            {
                Solver solver = CreateSolver();
                solver.SwapSides();

                // Get the best move.
                Move move = solver.GetMoveScores()
                    .OrderByDescending(x => x.Score)
                    .Select(x => x.Move)
                    .FirstOrDefault();

                if (move == null)
                {
                    break;
                }
                
                // Play the move.
                Tiles.Remove(GetTile(move.Source));
                Tile target = GetTile(move.Target);
                target.Penguin = 2;
                RedScore += target.Fish;
                
                // If the opponent has no legal move, then move again.
                solver = CreateSolver();
                if (solver.GetAvailableMoves().Any())
                {
                    break;
                }
            }
        }

        private void RemoveDeadPenguins()
        {
            Solver solver = CreateSolver();
            foreach (Position penguin in solver.GetDeadPenguins())
            {
                Tiles.Remove(GetTile(penguin));
            }
            solver.SwapSides();
            foreach (Position penguin in solver.GetDeadPenguins())
            {
                Tiles.Remove(GetTile(penguin));
            }
        }

        public Tile GetTile(Position position)
        {
            return Tiles.Single(t => t.Row == position.Row && t.Column == position.Column);
        }

        private void DeselectPenguin()
        {
            if (selectedPenguin != null)
            {
                selectedPenguin.IsSelected = false;
                selectedPenguin = null;
            }
        }

        public Solver CreateSolver()
        {
            Solver solver = new Solver();
            foreach (Tile tile in Tiles)
            {
                solver.Fish[tile.Row][tile.Column] = tile.Fish;
            }
            solver.Blue = Tiles.Where(tile => tile.Penguin == 1).Select(tile => new Position(tile.Row, tile.Column)).ToArray();
            solver.Red = Tiles.Where(tile => tile.Penguin == 2).Select(tile => new Position(tile.Row, tile.Column)).ToArray();

            foreach (Position penguin in solver.Blue)
            {
                solver.Fish[penguin.Row][penguin.Column] = 0;
            }

            foreach (Position penguin in solver.Red)
            {
                solver.Fish[penguin.Row][penguin.Column] = 0;
            }

            return solver;
        }
    }
}
