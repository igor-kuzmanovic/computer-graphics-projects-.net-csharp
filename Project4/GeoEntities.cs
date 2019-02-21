using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project4
{
    [XmlRoot(ElementName = "Substation")]
    public class SubstationGeo
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Latitude")]
        public double Latitude { get; set; }
        [XmlElement(ElementName = "Longitude")]
        public double Longitude { get; set; }
    }

    [XmlRoot(ElementName = "Substations")]
    public class SubstationsGeo
    {
        [XmlElement(ElementName = "Substation")]
        public List<SubstationGeo> Substations { get; set; }
    }

    [XmlRoot(ElementName = "Node")]
    public class NodeGeo
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Latitude")]
        public double Latitude { get; set; }
        [XmlElement(ElementName = "Longitude")]
        public double Longitude { get; set; }
    }

    [XmlRoot(ElementName = "Nodes")]
    public class NodesGeo
    {
        [XmlElement(ElementName = "Node")]
        public List<NodeGeo> Nodes { get; set; }
    }

    [XmlRoot(ElementName = "Switch")]
    public class SwitchGeo
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "Latitude")]
        public double Latitude { get; set; }
        [XmlElement(ElementName = "Longitude")]
        public double Longitude { get; set; }
    }

    [XmlRoot(ElementName = "Switches")]
    public class SwitchesGeo
    {
        [XmlElement(ElementName = "Switch")]
        public List<SwitchGeo> Switches { get; set; }
    }

    [XmlRoot(ElementName = "Point")]
    public class PointGeo
    {
        [XmlElement(ElementName = "Latitude")]
        public double Latitude { get; set; }
        [XmlElement(ElementName = "Longitude")]
        public double Longitude { get; set; }
    }

    [XmlRoot(ElementName = "Vertices")]
    public class VerticesGeo
    {
        [XmlElement(ElementName = "Point")]
        public List<PointGeo> Points { get; set; }
    }

    [XmlRoot(ElementName = "Line")]
    public class LineGeo
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
        public VerticesGeo Vertices { get; set; }
    }

    [XmlRoot(ElementName = "Lines")]
    public class LinesGeo
    {
        [XmlElement(ElementName = "Line")]
        public List<LineGeo> Lines { get; set; }
    }

    [XmlRoot(ElementName = "NetworkModel")]
    public class NetworkModelGeo
    {
        [XmlElement(ElementName = "Substations")]
        public SubstationsGeo Substations { get; set; }
        [XmlElement(ElementName = "Nodes")]
        public NodesGeo Nodes { get; set; }
        [XmlElement(ElementName = "Switches")]
        public SwitchesGeo Switches { get; set; }
        [XmlElement(ElementName = "Lines")]
        public LinesGeo Lines { get; set; }
    }
}

