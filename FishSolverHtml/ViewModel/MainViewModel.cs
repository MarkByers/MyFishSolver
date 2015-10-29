using System;
using System.Collections.Generic;
using System.Linq;
using HeyThatsMyFishSolver;
using System.Collections.ObjectModel;
using System.ComponentModel;
using FishSolverHtml.Helpers;
using FishSolverHtml.Model;

namespace FishSolverHtml.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        public RelayCommand LoadBoardCommand { get; set; }
        public RelayCommand SolveCommand { get; set; }

        public MainViewModel()
        {
            SolveCommand = new RelayCommand(Solve);
            LoadBoardCommand = new RelayCommand(LoadBoard);
            CreateBoard();
        }

        #region Board Board

        private Board _board = null;
        public Board Board
        {
            get { return _board; }
            set
            {
                if (_board != value)
                {
                    _board = value;
                    NotifyPropertyChanged("Board");
                    Board.PropertyChanged += Board_PropertyChanged;
                    MoveScores = new List<MoveScore>();
                }
            }
        }

        #endregion

        #region string BoardText

        private string _boardText = "";

        public string BoardText
        {
            get
            {
                return _boardText;
            }
            set
            {
                if (_boardText != value)
                {
                    _boardText = value;
                    NotifyPropertyChanged("BoardText");
                }
            }
        }
        
        #endregion

        private void CreateBoard()
        {
            Board = ParseBoard(@"
 3 1 0 0 0
R 0 B 0 0 0
 3 1 0 0 0
0 0 0 0 3 0
 R 1 1 B 0
3 0 0 0 0 0
");
        }

        private void Board_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MoveScores = new List<MoveScore>();
        }

        private Board ParseBoard(string p)
        {
            Board board = new Board();
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
                        tiles.Add(new Tile(board) { Row = row, Column = column, Fish = c - '0' });
                    }
                    else if (c == 'R')
                    {
                        tiles.Add(new Tile(board) { Row = row, Column = column, Fish = 1, Penguin = 1 });
                    }
                    else if (c == 'B')
                    {
                        tiles.Add(new Tile(board) { Row = row, Column = column, Fish = 1, Penguin = 2 });
                    }
                    column++;
                }
                row++;
            }
            board.Tiles = new ObservableCollection<Tile>(tiles);
            return board;
        }

        private void Solve()
        {
            Solver solver = Board.CreateSolver();
            MoveScores = solver.GetMoveScores().OrderByDescending(x => x.Score).ToList();
        }

        private void LoadBoard()
        {
            Board = ParseBoard(BoardText);
        }

        public void UnhighlightMove(Move move)
        {
            Tile source = Board.GetTile(move.Source);
            source.IsHighlighted = false;
            Tile target = Board.GetTile(move.Target);
            target.IsHighlighted = false;
        }

        public void HighlightMove(Move move)
        {
            Tile source = Board.GetTile(move.Source);
            source.IsHighlighted = true;
            Tile target = Board.GetTile(move.Target);
            target.IsHighlighted = true;
        }

        #region List<MoveScore> MoveScores

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
                if (_moveScores != value)
                {
                    _moveScores = value;
                    NotifyPropertyChanged("MoveScores");
                }
            }
        }

        #endregion
    }
}
