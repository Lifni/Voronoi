using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VoronoiDiagram;
using VoronoiDiagram.Structs;

namespace Voronoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int NumberOfVertices = 50;
        private Main map;
        private List<VoronoiDiagram.Structs.Point> verts;
        public MainWindow()
        {
            InitializeComponent();
            map = new Main();
        }

        public void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            this.verts = new List<VoronoiDiagram.Structs.Point>();
            this.MakeRandom(NumberOfVertices, verts);
            this.Create(verts);
        }

        public void btnRun_Click(object sender, RoutedEventArgs e)
        {
            this.map.AddPoints(this.verts);
            this.map.BuildVoronoi();
            this.DrawLines();
        }

        private void DrawLines()
        {
            foreach (VoronoiDiagram.Structs.Edge e in map.vor.edges)
            {
                Line myLine = new Line();
                myLine.Stroke = System.Windows.Media.Brushes.LightSteelBlue;
                myLine.X1 = e.borders.Item1.x;
                myLine.X2 = e.borders.Item2.x;
                myLine.Y1 = e.borders.Item1.y;
                myLine.Y2 = e.borders.Item2.y;
                myLine.StrokeThickness = 1;
                drawingCanvas.Children.Add(myLine);
            }
        }
        private void MakeRandom(int n, List<VoronoiDiagram.Structs.Point> verts)
        {
            /*VoronoiDiagram.Structs.Point pi = new VoronoiDiagram.Structs.Point(28.5, 111.7);
            verts.Add(pi);

            pi = new VoronoiDiagram.Structs.Point(69, 312.2);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(120.7, 100);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(150.4, 66.2);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(159.6, 213);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(251.1, 16.7);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(309.1, 366.7);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(312, 31.3);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(349.7, 196.5);
            verts.Add(pi);
            pi = new VoronoiDiagram.Structs.Point(429.5, 44.7);
            verts.Add(pi);*/
            Random r = new Random();
            double sizeX = drawingCanvas.ActualWidth;
            double sizeY = drawingCanvas.ActualHeight;
            for (int i = 0; i < n; ++i)
            {
                VoronoiDiagram.Structs.Point pi = new VoronoiDiagram.Structs.Point(
                    Math.Round(sizeX * r.NextDouble(), 1),
                    Math.Round(sizeY * r.NextDouble(), 1));
                verts.Add(pi);
            }
        }

        private void Create(List<VoronoiDiagram.Structs.Point> verts)
        {
            drawingCanvas.Children.Clear();
            ShowVertices(verts);
        }

        void ShowVertices(List<VoronoiDiagram.Structs.Point> verts)
        {
            foreach(VoronoiDiagram.Structs.Point v in verts)
            {
                Ellipse el = new Ellipse();
                el.Width = 3;
                el.Height = 3;
                el.Fill = Brushes.Blue;
                Canvas.SetLeft(el, v.x);
                Canvas.SetTop(el, v.y);
                drawingCanvas.Children.Add(el);
            }
        }
        
        void VertsToPoints(List<Vertex> verts, List<VoronoiDiagram.Structs.Point> points)
        {
            foreach(Vertex vertex in verts)
            {
                points.Add(new VoronoiDiagram.Structs.Point(vertex.Position[0], vertex.Position[1]));
            }
        }

    }
}
