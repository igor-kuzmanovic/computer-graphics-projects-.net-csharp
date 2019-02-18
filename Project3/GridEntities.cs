using System;
using System.Collections.Generic;
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
        private double distance;

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
        public Network()
        {
            Points = new List<GridPoint>();
            Lines = new List<GridLine>();
        }

        public Network(NetworkModel networkModel) : this()
        {
            LoadSubstations(networkModel.Substations);
            LoadNodes(networkModel.Nodes);
            LoadSwitches(networkModel.Switches);
            LoadLines(networkModel.Lines);       
        }

        public List<GridPoint> Points { get; set; }
        public List<GridLine> Lines { get; set; }

        private void LoadSubstations(Substations substations)
        {
            substations.SubstationEntity.ForEach(e =>
            {
                GridPoint point = new GridPoint()
                {
                    Id = e.Id,
                    X = Math.Round(e.X),
                    Y = Math.Round(e.Y),
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
                    X = Math.Round(e.X),
                    Y = Math.Round(e.Y),
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
                    X = Math.Round(e.X),
                    Y = Math.Round(e.Y),
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
    }
}

