﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Structs
{
    class Corner
    {
        public int id;

        public Point point;  // location
        public bool ocean;  // ocean
        public bool water;  // lake or ocean
        public bool coast;  // touches ocean and land polygons
        public bool border;  // at the edge of the map
        public double elevation;  // 0.0-1.0
        public double moisture;  // 0.0-1.0

        public List<Center> touches;
        public List<Edge> protrudes;
        public List<Corner> adjacent;

        public int river;  // 0 if no river, or volume of water in river
        public Corner downslope;  // pointer to adjacent corner most downhill
        public Corner watershed;  // pointer to coastal corner, or null
        public int watershed_size;
    }
}
