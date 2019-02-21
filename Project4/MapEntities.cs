using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project4
{
    public enum PointType
    {
        None,
        Connector,
        Substation,
        Node,
        Switch
    }

    public enum Connectivity
    {
        None,
        Low,
        Medium,
        High
    }

    [XmlRoot(ElementName = "Point")]
    public class MapPoint
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "Type")]
        public PointType Type { get; set; }
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y { get; set; }
        [XmlElement(ElementName = "Connections")]
        public List<long> Connections { get; set; }
        [XmlElement(ElementName = "Connectivity")]
        public Connectivity Connectivity
        {
            get
            {
                if (Type != PointType.Connector)
                {
                    double connectionCount = Connections.Count;
                    if (connectionCount >= 0 && connectionCount < 3)
                    {
                        return Connectivity.Low;
                    }
                    if (connectionCount >= 3 && connectionCount < 5)
                    {
                        return Connectivity.Medium;
                    }
                    if (connectionCount >= 5)
                    {
                        return Connectivity.High;
                    }
                }
                return Connectivity.None;
            }
        }
    }

    [XmlRoot(ElementName = "Line")]
    public class MapLine
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "Point")]
        public List<MapPoint> Points { get; set; }
    }

    [XmlRoot(ElementName = "NetworkModel")]
    public class MapNetworkModel
    {
        [XmlElement(ElementName = "Point")]
        public List<MapPoint> Points { get; set; }
        [XmlElement(ElementName = "Line")]
        public List<MapLine> Lines { get; set; }
    }
}

