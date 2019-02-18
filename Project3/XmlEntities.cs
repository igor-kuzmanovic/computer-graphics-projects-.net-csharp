using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project3
{
    [XmlRoot(ElementName = "SubstationEntity")]
    public class SubstationEntity
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y { get; set; }
    }

    [XmlRoot(ElementName = "Substations")]
    public class Substations
    {
        [XmlElement(ElementName = "SubstationEntity")]
        public List<SubstationEntity> SubstationEntity { get; set; }
    }

    [XmlRoot(ElementName = "NodeEntity")]
    public class NodeEntity
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y { get; set; }
    }

    [XmlRoot(ElementName = "Nodes")]
    public class Nodes
    {
        [XmlElement(ElementName = "NodeEntity")]
        public List<NodeEntity> NodeEntity { get; set; }
    }

    [XmlRoot(ElementName = "SwitchEntity")]
    public class SwitchEntity
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y { get; set; }
    }

    [XmlRoot(ElementName = "Switches")]
    public class Switches
    {
        [XmlElement(ElementName = "SwitchEntity")]
        public List<SwitchEntity> SwitchEntity { get; set; }
    }

    [XmlRoot(ElementName = "Point")]
    public class Point
    {
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y { get; set; }
    }

    [XmlRoot(ElementName = "Vertices")]
    public class Vertices
    {
        [XmlElement(ElementName = "Point")]
        public List<Point> Point { get; set; }
    }

    [XmlRoot(ElementName = "LineEntity")]
    public class LineEntity
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "IsUnderground")]
        public bool IsUnderground { get; set; }
        [XmlElement(ElementName = "R")]
        public double R { get; set; }
        [XmlElement(ElementName = "ConductorMaterial")]
        public string ConductorMaterial { get; set; }
        [XmlElement(ElementName = "LineType")]
        public string LineType { get; set; }
        [XmlElement(ElementName = "ThermalConstantHeat")]
        public long ThermalConstantHeat { get; set; }
        [XmlElement(ElementName = "FirstEnd")]
        public long FirstEnd { get; set; }
        [XmlElement(ElementName = "SecondEnd")]
        public long SecondEnd { get; set; }
        [XmlElement(ElementName = "Vertices")]
        public Vertices Vertices { get; set; }
    }

    [XmlRoot(ElementName = "Lines")]
    public class Lines
    {
        [XmlElement(ElementName = "LineEntity")]
        public List<LineEntity> LineEntity { get; set; }
    }

    [XmlRoot(ElementName = "NetworkModel")]
    public class NetworkModel
    {
        [XmlElement(ElementName = "Substations")]
        public Substations Substations { get; set; }
        [XmlElement(ElementName = "Nodes")]
        public Nodes Nodes { get; set; }
        [XmlElement(ElementName = "Switches")]
        public Switches Switches { get; set; }
        [XmlElement(ElementName = "Lines")]
        public Lines Lines { get; set; }
    }
}

