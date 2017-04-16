using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VoronoiDiagram.Structs;

namespace VoronoiDiagram
{
    public class Voronoi
    {
        private List<Center> centers;
        public List<Edge> edges;
        private List<Corner> corners;

        public void Generate(List<Point> points)
        {
            this.centers = new List<Center>();
            foreach(Point p in points)
            {
                this.centers.Add(new Center(p));
            }
            this.centers = this.centers.OrderBy(i => i.point.x).ThenBy(i => i.point.y).ToList();
            this.edges = new List<Edge>();
            this.corners = new List<Corner>();
            List<Point> convexHall = new List<Point>();
            this.DivideAndConquer(ref centers, ref edges, ref convexHall);
        }

        private void DivideAndConquer(ref List<Center> centers, ref List<Edge> edges, ref List<Point> convexHall)
        {
            if (centers.Count == 0) return;
            if (centers.Count == 1)
            {
                edges = new List<Edge>();
                convexHall = new List<Point>();
                convexHall.Add(centers[0].point);
                return;
            }
            if (centers.Count == 2)
            {
                edges = new List<Edge>();
                edges.Add(new Edge(centers[0], centers[1]));
                centers[0].borders = new List<Edge>();
                centers[1].borders = new List<Edge>();
                centers[0].borders.Add(edges[0]);
                centers[1].borders.Add(edges[0]);
                centers[0].neighbours = new List<Center>();
                centers[1].neighbours = new List<Center>();
                centers[0].neighbours.Add(centers[1]);
                centers[1].neighbours.Add(centers[0]);
                convexHall = new List<Point>();
                convexHall.Add(centers[0].point);
                convexHall.Add(centers[1].point);
                return;
            }
            List<Center> leftCenters = new List<Center>();
            List<Center> rightCenters = new List<Center>();
            List<Edge> leftEdges = new List<Edge>();
            List<Edge> rightEdges = new List<Edge>();
            this.SplitByMedX(ref centers, ref leftCenters, ref leftEdges, ref rightCenters, ref rightEdges);

            List<Point> leftCH = new List<Point>();
            List<Point> rightCH = new List<Point>();
            this.DivideAndConquer(ref leftCenters, ref leftEdges, ref leftCH);
            this.DivideAndConquer(ref rightCenters, ref rightEdges, ref rightCH);

            Tuple<Point, Point> upSegm = new Tuple<Point, Point>(null, null);
            Tuple<Point, Point> downSegm = new Tuple<Point, Point>(null, null);
            this.UpDownSegments(leftCH, rightCH, ref upSegm, ref downSegm);


            this.MergeVoronoi(ref centers, ref edges, ref leftCenters, ref leftEdges,
                                ref rightCenters, ref rightEdges, ref upSegm, ref downSegm);

            convexHall = this.MergeCH(leftCH, rightCH, upSegm, downSegm);
        }

        private void SplitByMedX(ref List<Center> centers, ref List<Center> leftCenters, ref List<Edge> leftEdges,
                                ref List<Center> rightCenters, ref List<Edge> rightEdges)
        {
            double x = 0;
            foreach (Center c in centers)
                x += c.point.x;
            x /= centers.Count;
            foreach (Center c in centers)
            {
                if (c.point.x <= x)
                {
                    leftCenters.Add(c);
                    foreach (Edge b in c.borders)
                        if (!leftEdges.Contains(b))
                            leftEdges.Add(b);
                }
                else
                {
                    rightCenters.Add(c);
                    foreach (Edge b in c.borders)
                        if (!rightEdges.Contains(b))
                            rightEdges.Add(b);
                }
            }
            if(rightCenters.Count == 0)
            {
                leftCenters.Clear();
                int i = 0;
                for (; i < centers.Count / 2; i++)
                    leftCenters.Add(centers[i]);
                for (; i < centers.Count; i++)
                    rightCenters.Add(centers[i]);
            }
        }

        private void UpDownSegments(List<Point> leftCH, List<Point> rightCH,
                                    ref Tuple<Point, Point> upSegm, ref Tuple<Point, Point> downSegm)
        {
            int leftStart = leftCH.FindIndex(i => i.x == leftCH.Max(t => t.x));
            int rightStart = rightCH.FindIndex(i => i.x == rightCH.Min(t => t.x));
            
            int lenL = leftCH.Count;
            int lenR = rightCH.Count;

            //for up
            int left = leftStart;
            int right = rightStart;
            int prevLeft = -1;
            int prevRight = -1;
            while (prevLeft != left || prevRight != right)
            {
                prevLeft = left;
                prevRight = right;
                while (this.PositionPoint(leftCH[left], rightCH[right], leftCH[(left - 1 + lenL) % lenL]) == 1)
                    left = (left - 1 + lenL) % lenL;
                while (this.PositionPoint(leftCH[left], rightCH[right], rightCH[(right + 1) % lenR]) == 1)
                    right = (right + 1) % lenR;
            }
            upSegm = new Tuple<Point, Point>(leftCH[left], rightCH[right]);

            // for down
            left = leftStart;
            right = rightStart;
            prevLeft = -1;
            prevRight = -1;
            while (prevLeft != left || prevRight != right)
            {
                prevLeft = left;
                prevRight = right;
                while (this.PositionPoint(leftCH[left], rightCH[right], rightCH[(right - 1 + lenR) % lenR]) == -1)
                    right = (right - 1 + lenR) % lenR;
                while (this.PositionPoint(leftCH[left], rightCH[right], leftCH[(left + 1) % lenL]) == -1)
                    left = (left + 1) % lenL;
            }
            downSegm = new Tuple<Point, Point>(leftCH[left], rightCH[right]);


            /*Point leftIn = this.PointInside(leftCH);
            Point rightIn = this.PointInside(rightCH);

            Tuple<Point, Point> leftSegm = this.UpDownPoints(leftCH, rightIn);
            Tuple<Point, Point> rightSegm = this.UpDownPoints(rightCH, leftIn);

            upSegm = new Tuple<Point, Point>(leftSegm.Item2, rightSegm.Item2);
            downSegm = new Tuple<Point, Point>(leftSegm.Item1, rightSegm.Item1);*/
        }

        private void MergeVoronoi(ref List<Center> resCenters, ref List<Edge> resEdges,
                                    ref List<Center> leftCenters, ref List<Edge> leftEdges,
                                    ref List<Center> rightCenters, ref List<Edge> rightEdges,
                                    ref Tuple<Point, Point> upSegm, ref Tuple<Point, Point> downSegm)
        {
            Tuple<Point, Point> curSegm = upSegm;
            Point st = new Point(Math.Round((curSegm.Item1.x + curSegm.Item2.x) / 2, StaticNums.round),
                                Math.Round((curSegm.Item1.y + curSegm.Item2.y) / 2, StaticNums.round));
            Point temp;
            if (curSegm.Item2.x == curSegm.Item1.x)
            {
                temp = new Point(curSegm.Item1.x, StaticNums.limY);
            }
            else
            {
                double k = (curSegm.Item2.y - curSegm.Item1.y) / (curSegm.Item2.x - curSegm.Item1.x);
                temp = new Point(Math.Round(-k * StaticNums.limY + st.x + st.y * k, StaticNums.round), StaticNums.limY);
            }
            Center leftCenter = leftCenters.Find(i => (i.point.AreEqual(curSegm.Item1)));
            Center rightCenter = rightCenters.Find(i => (i.point.AreEqual(curSegm.Item2)));
            while (!(curSegm.Item1.AreEqual(downSegm.Item1) && curSegm.Item2.AreEqual(downSegm.Item2)))
            {
                bool left = false;
                Edge interEdge = null;
                Edge interEdge2 = null;
                st = this.PerpPoint(curSegm, temp);
                st = new Point(Math.Round(st.x, StaticNums.round), Math.Round(st.y, StaticNums.round));
                Point interPoint = this.firstInterEdge(new Tuple<Point, Point>(st, temp),
                                                        leftCenter.borders, rightCenter.borders, 
                                                        ref interEdge, ref left, temp.y, ref interEdge2);

                Tuple<Point, Point> newEdgeBorders = new Tuple<Point, Point>(temp, interPoint);
                if (left)
                    this.ChangeEdges(ref leftEdges, leftCenter, newEdgeBorders);
                if(!left || interEdge2 != null)
                    this.ChangeEdges(ref rightEdges, rightCenter, newEdgeBorders);
                Edge newEdge = new Edge(leftCenter, rightCenter, newEdgeBorders);
                leftCenter.borders.Add(newEdge);
                rightCenter.borders.Add(newEdge);
                leftCenter.neighbours.Add(rightCenter);
                rightCenter.neighbours.Add(leftCenter);
                leftEdges.Add(newEdge);
                rightEdges.Add(newEdge);

                if (left)
                {
                    if (interEdge.v0.point.AreEqual(leftCenter.point))
                    {
                        curSegm = new Tuple<Point, Point>(interEdge.v1.point, curSegm.Item2);
                        leftCenter = interEdge.v1;
                    }
                    else
                    {
                        curSegm = new Tuple<Point, Point>(interEdge.v0.point, curSegm.Item2);
                        leftCenter = interEdge.v0;

                    }
                }
                if(!left || interEdge2 != null)
                {
                    if (interEdge.v0.point.AreEqual(rightCenter.point))
                    {
                        curSegm = new Tuple<Point, Point>(curSegm.Item1, interEdge.v1.point);
                        rightCenter = interEdge.v1;
                    }
                    else
                    {
                        curSegm = new Tuple<Point, Point>(curSegm.Item1, interEdge.v0.point);
                        rightCenter = interEdge.v0;

                    }
                }
                temp = interPoint;
            }
            Point perp = this.PerpPoint(curSegm, temp);
            perp = new Point(Math.Round((perp.x - temp.x) / (perp.y - temp.y) * (-StaticNums.limY) +
                                (temp.x * perp.y - perp.x * temp.y) / (perp.y - temp.y), StaticNums.round), -StaticNums.limY);
            Edge lastEdge = new Edge(leftCenter, rightCenter, new Tuple<Point, Point>(temp, perp));
            leftCenter.borders.Add(lastEdge);
            rightCenter.borders.Add(lastEdge);
            leftCenter.neighbours.Add(rightCenter);
            rightCenter.neighbours.Add(leftCenter);
            leftEdges.Add(lastEdge);
            rightEdges.Add(lastEdge);

            resCenters = new List<Center>();
            resEdges = new List<Edge>();
            resCenters.AddRange(leftCenters);
            resCenters.AddRange(rightCenters);
            resEdges.AddRange(leftEdges);
            foreach (Edge b in rightEdges)
                if (!resEdges.Contains(b))
                    resEdges.Add(b);
        }

        public List<Point> MergeCH(List<Point> leftCH, List<Point> rightCH, Tuple<Point, Point> upSegm, Tuple<Point, Point> downSegm)
        {
            List<Point> res = new List<Point>();
            Point p = this.PointInside(leftCH);
            List<int> delPoints = new List<int>();

            res.Add(upSegm.Item1);
            int len = rightCH.Count;
            if (len > 1)
            {
                int st = rightCH.FindIndex(i => i.AreEqual(upSegm.Item2));
                int end = rightCH.FindIndex(i => i.AreEqual(downSegm.Item2));
                if (st != end)
                    for (; st != end; st = (st + 1) % len)
                        res.Add(rightCH[st]);
                res.Add(rightCH[end]);
            }
            else
                res.Add(rightCH[0]);

            len = leftCH.Count;
            if (len > 1)
            {
                int st = leftCH.FindIndex(i => i.AreEqual(upSegm.Item1));
                int end = leftCH.FindIndex(i => i.AreEqual(downSegm.Item1));
                if (st != end)
                    for (; end != st; end = (end + 1) % len)
                        res.Add(leftCH[end]);
            }
            return res;
            /*if (len > 2)
            {
                int st = rightCH.FindIndex(i => i.AreEqual(upSegm.Item2));
                int end = rightCH.FindIndex(i => i.AreEqual(downSegm.Item2));
                if (this.Angle(p, rightCH[st], rightCH[(st + 1) % len]) < 0)
                {
                    for (int i = (st + 1) % len; i != end; i = (i + 1) % len)
                        delPoints.Add(i);
                }
                else
                {
                    for (int i = (st - 1 + len) % len; i != end; i = (i - 1 + len) % len)
                        delPoints.Add(i);
                }
                delPoints.Sort();
                for (int i = delPoints.Count - 1; i >= 0; i--)
                {
                    rightCH.RemoveAt(delPoints[i]);
                }
            }

            p = this.PointInside(rightCH);
            len = leftCH.Count;
            if (len > 2)
            {
                int st = leftCH.FindIndex(i => i.AreEqual(upSegm.Item1));
                int end = leftCH.FindIndex(i => i.AreEqual(downSegm.Item1));
                if (this.Angle(p, leftCH[st], leftCH[(st + 1) % len]) > 0)
                {
                    for (int i = (st + 1) % len; i != end; i = (i + 1) % len)
                        delPoints.Add(i);
                }
                else
                {
                    for (int i = (st - 1 + len) % len; i != end; i = (i - 1 + len) % len)
                        delPoints.Add(i);
                }
                delPoints.Sort();
                for (int i = delPoints.Count - 1; i >= 0; i--)
                {
                    leftCH.RemoveAt(delPoints[i]);
                }
            }

            res.AddRange(leftCH);
            res.AddRange(rightCH);
            p = leftCH[0];
            res = res.OrderBy(i => -Math.Atan2(i.y - p.y, i.x - p.x)).
                                ThenByDescending(i => Math.Sqrt((i.x - p.x) * (i.x - p.x) + (i.y - p.y) * (i.y - p.y))).ToList();
            res.Remove(p);
            res.Insert(0, p);
            return res;*/
        }

        private void ChangeEdges(ref List<Edge> edges, 
                                Center center, Tuple<Point, Point> segm)
        {
            int posCenter = this.PositionPoint(segm.Item1, segm.Item2, center.point);
            List<int> nums = new List<int>();
            for(int i=0;i<center.borders.Count;i++)
            {
                int pos1 = this.PositionPoint(segm.Item1, segm.Item2, center.borders[i].borders.Item1);
                int pos2 = this.PositionPoint(segm.Item1, segm.Item2, center.borders[i].borders.Item2);
                if (pos1 == pos2 || pos1*pos2 == 0)
                {
                    if((pos1*pos2 != 0 && posCenter != pos2) || 
                        (pos1 == 0 && pos2 != posCenter) || (pos2 == 0 && pos1 != posCenter))
                    {
                        nums.Add(i);
                        Center secondCenter = center.borders[i].v0;
                        if (secondCenter.point.AreEqual(center.point))
                            secondCenter = center.borders[i].v1;
                        secondCenter.borders.Remove(center.borders[i]);
                        edges.Remove(center.borders[i]);
                    }
                    else continue;
                    
                }
                else
                {
                    if(pos1 == posCenter)
                    {
                        center.borders[i].borders = new Tuple<Point, Point>(
                                                        center.borders[i].borders.Item1, segm.Item2);
                    }
                    else
                    {
                        center.borders[i].borders = new Tuple<Point, Point>(
                                                        segm.Item2, center.borders[i].borders.Item2);
                    }
                }
            }
            for(int i = nums.Count-1;i>=0;i--)
            {
                center.borders.RemoveAt(nums[i]);
            }
        }

        private Point firstInterEdge(Tuple<Point, Point> points, List<Edge> leftEdges, List<Edge> rightEdges, 
                                    ref Edge edgeRes, ref bool left, double thresholdY, ref Edge edgeRes2)
        {
            Edge leftEdge = null;
            Edge rightEdge = null;
            Point leftInter = firstInterEdge(points, leftEdges, ref leftEdge, thresholdY);
            Point rightInter = firstInterEdge(points, rightEdges, ref rightEdge, thresholdY);
            if(leftInter.y == rightInter.y)
            {
                edgeRes = leftEdge;
                edgeRes2 = rightEdge;
                left = true;
                return leftInter;
            }
            if (leftInter.y > rightInter.y)
            {
                edgeRes = leftEdge;
                left = true;
                return leftInter;
            }
            edgeRes = rightEdge;
            return rightInter;
        }

        private Point firstInterEdge(Tuple<Point, Point> points, List<Edge> edges, ref Edge edgeRes, double thresholdY)
        {
            Point res = new Point(-StaticNums.limX, -StaticNums.limY);
            if (edges == null || edges.Count == 0) return res;
            foreach (Edge edge in edges)
            {
                Point inter = this.LineIntersection(points, edge.borders);
                inter = new Point(Math.Round(inter.x, StaticNums.round), Math.Round(inter.y, StaticNums.round));
                if (inter == null) continue;
                if (!this.PointOnSegm(edge.borders, inter)) continue;
                if (inter.y > res.y && inter.y < thresholdY)
                {
                    edgeRes = edge;
                    res = inter;
                }
            }
            return res;
        }

        private Point PerpPoint(Tuple<Point, Point> segm, Point p)
        {
            if (segm.Item1.x == segm.Item2.x)
                return new Point(segm.Item1.x, p.y);
            if (segm.Item1.y == segm.Item2.y)
                return new Point(p.x, segm.Item1.y);
            return LineIntersection(segm, new Tuple<double, double, double>((segm.Item2.x - segm.Item1.x), (segm.Item2.y - segm.Item1.y), 
                                    (-p.x * (segm.Item2.x - segm.Item1.x) - p.y * (segm.Item2.y - segm.Item1.y))));
                        
        }

        private Point LineIntersection(Tuple<Point, Point> l1, Tuple<double, double,double> coef)
        {
            double a1 = l1.Item1.y - l1.Item2.y;
            double b1 = l1.Item2.x - l1.Item1.x;
            double c1 = l1.Item1.x * l1.Item2.y - l1.Item2.x * l1.Item1.y;

            double a2 = coef.Item1;
            double b2 = coef.Item2;
            double c2 = coef.Item3;

            double den = a1 * b2 - a2 * b1;
            if (den == 0) return null;
            return new Point(Math.Round(-(c1 * b2 - c2 * b1) / den, StaticNums.round), 
                            Math.Round(-(a1 * c2 - a2 * c1) / den, StaticNums.round));
        }

        private Point LineIntersection(Tuple<Point,Point> l1, Tuple<Point, Point> l2)
        {
            double a1 = l1.Item1.y - l1.Item2.y;
            double b1 = l1.Item2.x - l1.Item1.x;
            double c1 = l1.Item1.x * l1.Item2.y - l1.Item2.x * l1.Item1.y;

            double a2 = l2.Item1.y - l2.Item2.y;
            double b2 = l2.Item2.x - l2.Item1.x;
            double c2 = l2.Item1.x * l2.Item2.y - l2.Item2.x * l2.Item1.y;

            double den = a1 * b2 - a2 * b1;
            if (den == 0) return null;
            return new Point(-(c1*b2-c2*b1)/den, -(a1*c2-a2*c1)/den);
        }

        private Tuple<Point,Point> UpDownPoints(List<Point> ch, Point p)
        {
            if(ch.Count == 1)
                return new Tuple<Point, Point>(ch[0], ch[0]);
            List<Point> points = new List<Point>();
            int len = ch.Count;
            for (int i = 0; i < len; i++)
            {
                if (this.PositionPoint(p, ch[(i - 1 + len) % len], ch[i]) != this.PositionPoint(p, ch[i], ch[(i + 1) % len]))
                    points.Add(ch[i]);
            }
            points = points.OrderBy(i => i.y).ThenBy(i => i.x).ToList();
            return new Tuple<Point, Point>(points[0], points[1]);
        }

        private Point PointInside(List<Point> ps)
        {
            if (ps == null || ps.Count == 0) return null;
            if (ps.Count == 1) return ps[0];
            if (ps.Count == 2)
                return new Point((ps[0].x + ps[1].x) / 2, (ps[0].y + ps[1].y) / 2);
            return new Point((ps[0].x + ps[1].x + ps[2].x) / 3, (ps[0].y + ps[1].y + ps[2].y) / 3);
        }

        // 1 - up
        // -1 - down
        // 0 - on
        private int PositionPoint(Point a, Point b, Point c)
        {
            double D = a.x * (b.y - c.y) + b.x * (c.y - a.y) + c.x * (a.y - b.y);
            if (D == 0) return 0;
            return D > 0 ? 1 : -1;
        }

        private double Angle(Point left, Point center, Point right)
        {
            Point v1 = new Point(left.x - center.x, left.y - center.y);
            Point v2 = new Point(right.x - center.x, right.y - center.y);
            double dot = v1.x * v2.x + v1.y * v2.y;
            double det = v1.x * v2.y - v1.y * v2.x;
            return -Math.Atan2(det, dot) * 180 / Math.PI;
        }

        private bool PointOnSegm(Tuple<Point, Point> segm, Point p)
        {
            double maxX = Math.Max(segm.Item1.x, segm.Item2.x);
            double minX = Math.Min(segm.Item1.x, segm.Item2.x);
            double maxY = Math.Max(segm.Item1.y, segm.Item2.y);
            double minY = Math.Min(segm.Item1.y, segm.Item2.y);

            return minY <= p.y && p.y <= maxY && minX <= p.x && p.x <= maxX;
        }
    }
}
