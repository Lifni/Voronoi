using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Structs
{
    class Edge
    {
        public int index;
        public Center d0, d1;  // Delaunay edge
        public Center v0, v1;  // Voronoi edge
        public Point midpoint;  // halfway between v0,v1
        public Point start, end; 
        public int river;  // volume of water, or 0
    }
}
