using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace HeyThatsMyFishWpf.Model
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
                if (_penguin == value)
                {
                    return;
                }

                _penguin = value;
                RaisePropertyChanged(() => Penguin);
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

        /// <summary>
        /// The <see cref="IsHighlighted" /> property's name.
        /// </summary>
        public const string IsHighlightedPropertyName = "IsHighlighted";

        private bool _isHighlighted = false;

        /// <summary>
        /// Sets and gets the IsHighlighted property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsHighlighted
        {
            get
            {
                return _isHighlighted;
            }
            set
            {
                Set(() => IsHighlighted, ref _isHighlighted, value);
            }
        }

        #endregion

        #region bool IsSelected

        /// <summary>
        /// The <see cref="IsSelected" /> property's name.
        /// </summary>
        public const string IsSelectedPropertyName = "IsSelected";

        private bool _isSelected = false;

        /// <summary>
        /// Sets and gets the IsSelected property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                Set(() => IsSelected, ref _isSelected, value);
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
