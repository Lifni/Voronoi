using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map.Structs
{
    class Center
    {
        public int id;

        public Point point;  // location
        public bool water;  // lake or ocean
        public bool ocean;  // ocean
        public bool coast;  // land polygon touching an ocean
        public bool border;  // at the edge of the map
        public String biome;  // biome type (see article)
        public double elevation;  // 0.0-1.0
        public double moisture;  // 0.0-1.0

        public List<Center> neighbours;
        public List<Edge> borders;
        public List<Corner> corners;
    }
}
