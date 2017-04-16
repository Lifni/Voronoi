using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;

using VoronoiDiagram;

namespace Voronoi
{
    class Vertex : Shape
    {
        /// Gets or sets the coordinates.
        public double[] Position { get; set; }

        protected override Geometry DefiningGeometry
        {
            get
            {
                return new EllipseGeometry
                {
                    Center = new System.Windows.Point(Position[0], Position[1]),
                    RadiusX = 1.5,
                    RadiusY = 1.5
                };
            }
        }

        public Vertex(Brush fill = null)
        {
            Fill = fill ?? Brushes.Red;
        }

        /// Initializes a new instance of the <see cref="Vertex"/> class.
        public Vertex(double x, double y, Brush fill = null)
            : this(fill)
        {
            Position = new double[] { x, y };
        }

        public System.Windows.Point ToPoint()
        {
            return new System.Windows.Point(Position[0], Position[1]);
        }

    }
}
