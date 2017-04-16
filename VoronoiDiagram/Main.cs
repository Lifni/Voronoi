using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiDiagram.Structs;

namespace VoronoiDiagram
{
    public class Main
    {
        public List<Point> points;
        public Voronoi vor;

        public void BuildVoronoi()
        {
            vor = new Voronoi();
            vor.Generate(this.points);
        }

        public void AddPoints(List<Point> ps)
        {
            if (points == null) this.points = new List<Point>();
            this.points = new List<Point>(ps);
            this.points = points.Distinct().ToList();
        }

        //Check limits!!!!!!
    }
}
