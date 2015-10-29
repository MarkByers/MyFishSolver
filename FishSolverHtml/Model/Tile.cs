using FishSolverHtml.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishSolverHtml.Model
{
    public class Tile : ObservableObject
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Fish { get; set; }

        #region int Penguin

        /// <summary>
        /// The <see cref="Penguin" /> property's name.
        /// </summary>
        public const string PenguinPropertyName = "Penguin";

        private int _penguin = 0;



        /// <summary>
        /// Sets and gets the Penguin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Penguin
        {
            get
            {
                return _penguin;
            }

            set
            {
                if (_penguin != value)
                {
                    _penguin = value;
                    NotifyPropertyChanged("Penguin");
                }
            }
        }

        #endregion

        public RelayCommand SelectCommand { get; set; }

        private Board board;

        public Tile(Board board)
        {
            this.board = board;
            SelectCommand = new RelayCommand(Select);
        }

        private void Select()
        {
            board.SelectTile(this);
        }

        #region bool IsHighlighted

        private bool _isHighlighted = false;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                if (value != _isHighlighted)
                {
                    _isHighlighted = value;
                    NotifyPropertyChanged("IsHighlighted");
                }
            }
        }

        #endregion

        #region bool IsSelected

        private bool _isSelected = false;

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        private int tileSize = 50;

        public int X
        {
            get
            {
                return (tileSize - 1) * Column + (tileSize) / 2 * (Row % 2);
            }
        }

        public int Y
        {
            get
            {
                return ((int)(tileSize * 0.74)) * Row;
            }
        }
    }
}
