using System;
using System.Collections.Generic;
using HeyThatsMyFishSolver;

namespace HeyThatsMyFishWpf.Model
{
    public class Board
    {
        public List<Tile> Tiles { get; set; }

        public List<Position> BluePenguins { get; set; }

        public List<Position> RedPenguins { get; set; }
    }
}
