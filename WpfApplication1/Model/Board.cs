using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HeyThatsMyFishSolver;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using HeyThatsMyFishWpf.ViewModel;

namespace HeyThatsMyFishWpf.Model
{
    public class Board : ObservableObject
    {
        #region int RedScore

        /// <summary>
        /// The <see cref="RedScore" /> property's name.
        /// </summary>
        public const string RedScorePropertyName = "RedScore";

        private int _redScore = 0;

        /// <summary>
        /// Sets and gets the RedScore property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int RedScore
        {
            get
            {
                return _redScore;
            }
            set
            {
                Set(() => RedScore, ref _redScore, value);
            }
        }

        #endregion

        #region int BlueScore

        /// <summary>
        /// The <see cref="BlueScore" /> property's name.
        /// </summary>
        public const string BlueScorePropertyName = "BlueScore";

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
                Set(() => BlueScore, ref _blueScore, value);
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
                solver.Fish[tile.Row, tile.Column] = tile.Fish;
            }
            solver.Blue = Tiles.Where(tile => tile.Penguin == 1).Select(tile => new Position(tile.Row, tile.Column)).ToList();
            solver.Red = Tiles.Where(tile => tile.Penguin == 2).Select(tile => new Position(tile.Row, tile.Column)).ToList();

            foreach (Position penguin in solver.Blue.Concat(solver.Red))
            {
                solver.Fish[penguin.Row, penguin.Column] = 0;
            }

            return solver;
        }
    }
}
