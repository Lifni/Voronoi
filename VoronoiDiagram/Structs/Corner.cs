using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram.Structs
{
    public class Corner
    {
        public Point point;  // location
        
        public List<Center> touches;
        public List<Edge> protrudes;
        public List<Corner> adjacent;
    }
}
