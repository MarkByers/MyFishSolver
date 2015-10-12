using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace HeyThatsMyFishWpf.Model
{
    public class Tile : ObservableObject
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Fish { get; set; }

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

        private int tileSize = 50;

        public int X
        {
            get
            {
                return tileSize * Column + tileSize / 2 * (Row % 2);
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
