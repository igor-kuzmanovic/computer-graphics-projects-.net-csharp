using Project3.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project3
{
    public enum GridPointType
    {
        Substation,
        Node,
        Switch
    }

    public class GridPoint
    {
        public long Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Name { get; set; }
        public GridPointType Type { get; set; }
    }

    public class GridLine
    {
        public long Id { get; set; }
        public double StartX { get; set; }
        public double StartY { get; set; }
        public double EndX { get; set; }
        public double EndY { get; set; }
        public double Distance {
            get
            {
                return Math.Sqrt(Math.Pow(StartX - EndX, 2) + Math.Pow(StartY - EndY, 2));
            }
        }
    }

    public class Network
    {
        private const int utmZone = 34;

        public Network()
        {
            Points = new List<GridPoint>();
            Lines = new List<GridLine>();
        }

        public Network(string filePath) : this()
        {
            NetworkModel networkModel = LoadNetworkModelFromXml(filePath);
            LoadSubstations(networkModel.Substations);
            LoadNodes(networkModel.Nodes);
            LoadSwitches(networkModel.Switches);
            LoadLines(networkModel.Lines);
            CompressCoordinates();
        }

        public List<GridPoint> Points { get; set; }
        public List<GridLine> Lines { get; set; }

        private NetworkModel LoadNetworkModelFromXml(string filePath)
        {
            return NetworkModel.GenerateFromXml(filePath);
        }

        private void LoadSubstations(Substations substations)
        {
            substations.SubstationEntity.ForEach(e =>
            {
                GridPoint point = new GridPoint()
                {
                    Id = e.Id,
                    X = e.X,
                    Y = e.Y,
                    Name = e.Name,
                    Type = GridPointType.Substation
                };

                Points.Add(point);
            });
        }

        private void LoadNodes(Nodes nodes)
        {
            nodes.NodeEntity.ForEach(e =>
            {
                GridPoint point = new GridPoint()
                {
                    Id = e.Id,
                    X = e.X,
                    Y = e.Y,
                    Name = e.Name,
                    Type = GridPointType.Node
                };

                Points.Add(point);
            });
        }

        private void LoadSwitches(Switches switches)
        {
            switches.SwitchEntity.ForEach(e =>
            {
                GridPoint point = new GridPoint()
                {
                    Id = e.Id,
                    X = e.X,
                    Y = e.Y,
                    Name = e.Name,
                    Type = GridPointType.Switch
                };

                Points.Add(point);
            });
        }

        private void LoadLines(Lines lines)
        {
            lines.LineEntity.ForEach(e =>
            {
                if (Points.Any(p => p.Id == e.FirstEnd) && Points.Any(p => p.Id == e.SecondEnd))
                {
                    GridPoint startPoint = Points.Single(p => p.Id == e.FirstEnd);
                    GridPoint endPoint = Points.Single(p => p.Id == e.SecondEnd);

                    GridLine line = new GridLine()
                    {
                        Id = e.Id,
                        StartX = startPoint.X,
                        StartY = startPoint.Y,
                        EndX = endPoint.X,
                        EndY = endPoint.Y
                    };

                    Lines.Add(line);
                }
            });
        }

        private void CompressCoordinates()
        {
            double scaleX = 1.0 / 35;
            double scaleY = 1.0 / 35;

            Points.ForEach(p =>
            {
                p.X = Math.Round(p.X * scaleX, 0);
                p.Y = Math.Round(p.Y * scaleY, 0);
            });

            do {
                List<Tuple<double, double>> usedPoints = new List<Tuple<double, double>>();

                double minX = Points.Min(p => p.X);
                double minY = Points.Min(p => p.Y);

                Points.ForEach(p =>
                {
                    double tempX = p.X - minX;
                    double tempY = p.Y - minY;

                    int offsetStart = 0;
                    int offsetEnd = 0;
                    int offsetX = 0;
                    int offsetY = 0;

                    while (usedPoints.Any(pt => pt.Item1 == tempX + offsetX && pt.Item2 == tempY + offsetY))
                    {
                        bool hasFoundFree = false;

                        offsetStart--;
                        offsetEnd++;

                        for (int i = offsetStart; !hasFoundFree && i <= offsetEnd; i++)
                        {
                            for (int j = offsetStart; !hasFoundFree && j <= offsetEnd; j++)
                            {
                                if (!usedPoints.Any(pt => pt.Item1 == tempX + i && pt.Item2 == tempY + j))
                                {
                                    offsetX = i;
                                    offsetY = j;
                                    hasFoundFree = true;
                                }
                            }
                        }
                    }

                    p.X = tempX + offsetX;
                    p.Y = tempY + offsetY;

                    usedPoints.Add(Tuple.Create(p.X, p.Y));
                });

                Trace.WriteLine($"" +
                    $"MinX = {Points.Min(p => p.X)}\n" +
                    $"MinY = {Points.Min(p => p.Y)}\n" +
                    $"MaxX = {Points.Max(p => p.X)}\n" +
                    $"MaxY = {Points.Max(p => p.Y)}");

                Trace.WriteLine("Number of points " + Points.Count.ToString());
                Trace.WriteLine("Number of used points " + usedPoints.Count.ToString());
            } while (Points.Any(p => p.X < 0 || p.Y < 0));          
        }
    }
}

