using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project4
{
    [XmlRoot(ElementName = "SubstationEntity")]
    public class SubstationUTM
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y{ get; set; }
    }

    [XmlRoot(ElementName = "Substations")]
    public class SubstationsUTM
    {
        [XmlElement(ElementName = "SubstationEntity")]
        public List<SubstationUTM> Substations { get; set; }
    }

    [XmlRoot(ElementName = "NodeEntity")]
    public class NodeUTM
    {
        [XmlElement(ElementName = "Id")]
        public long Id { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y{ get; set; }
    }

    [XmlRoot(ElementName = "Nodes")]
    public class NodesUTM
    {
        [XmlElement(ElementName = "NodeEntity")]
        public List<NodeUTM> Nodes { get; set; }
    }

    [XmlRoot(ElementName = "SwitchEntity")]
    public class SwitchUTM
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
        public double Y{ get; set; }
    }

    [XmlRoot(ElementName = "Switches")]
    public class SwitchesUTM
    {
        [XmlElement(ElementName = "SwitchEntity")]
        public List<SwitchUTM> Switches { get; set; }
    }

    [XmlRoot(ElementName = "Point")]
    public class PointUTM
    {
        [XmlElement(ElementName = "X")]
        public double X { get; set; }
        [XmlElement(ElementName = "Y")]
        public double Y{ get; set; }
    }

    [XmlRoot(ElementName = "Vertices")]
    public class VerticesUTM
    {
        [XmlElement(ElementName = "Point")]
        public List<PointUTM> Points { get; set; }
    }

    [XmlRoot(ElementName = "LineEntity")]
    public class LineUTM
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
        public VerticesUTM Vertices { get; set; }
    }

    [XmlRoot(ElementName = "Lines")]
    public class LinesUTM
    {
        [XmlElement(ElementName = "LineEntity")]
        public List<LineUTM> Lines { get; set; }
    }

    [XmlRoot(ElementName = "NetworkModel")]
    public class NetworkModelUTM
    {
        [XmlElement(ElementName = "Substations")]
        public SubstationsUTM Substations { get; set; }
        [XmlElement(ElementName = "Nodes")]
        public NodesUTM Nodes { get; set; }
        [XmlElement(ElementName = "Switches")]
        public SwitchesUTM Switches { get; set; }
        [XmlElement(ElementName = "Lines")]
        public LinesUTM Lines { get; set; }
    }
}

