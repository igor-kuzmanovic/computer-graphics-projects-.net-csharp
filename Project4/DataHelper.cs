using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Project4
{
    class DataHelper
    {
        public NetworkModelUTM ImportNetworkModelUTMFromXML()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkModelUTM));
            using (FileStream fileStream = new FileStream(Config.dataUTMPath, FileMode.Open))
            {
                return (NetworkModelUTM)serializer.Deserialize(fileStream);
            }
        }

        public NetworkModelGeo ConvertNetworkModelUTMToGeo(NetworkModelUTM networkModelUTM)
        {
            NetworkModelGeo networkModelGeo = new NetworkModelGeo();

            networkModelGeo.Substations = new SubstationsGeo() { Substations = new List<SubstationGeo>() };

            networkModelUTM.Substations.Substations.ForEach(substationUTM =>
            {
                UTMToLatitudeLongitude(substationUTM.X, substationUTM.Y, out double latitude, out double longitude);

                if (!double.IsNaN(latitude) && !double.IsNaN(longitude))
                {
                    SubstationGeo substationGeo = new SubstationGeo();
                    substationGeo.Id = substationUTM.Id;
                    substationGeo.Name = substationUTM.Name;
                    substationGeo.Latitude = latitude;
                    substationGeo.Longitude = longitude;

                    networkModelGeo.Substations.Substations.Add(substationGeo);
                }
            });

            networkModelGeo.Nodes = new NodesGeo() { Nodes = new List<NodeGeo>() };

            networkModelUTM.Nodes.Nodes.ForEach(nodeUTM =>
            {
                UTMToLatitudeLongitude(nodeUTM.X, nodeUTM.Y, out double latitude, out double longitude);

                if (!double.IsNaN(latitude) && !double.IsNaN(longitude))
                {
                    NodeGeo nodeGeo = new NodeGeo();
                    nodeGeo.Id = nodeUTM.Id;
                    nodeGeo.Name = nodeUTM.Name;
                    nodeGeo.Latitude = latitude;
                    nodeGeo.Longitude = longitude;

                    networkModelGeo.Nodes.Nodes.Add(nodeGeo);
                }
            });

            networkModelGeo.Switches = new SwitchesGeo() { Switches = new List<SwitchGeo>() };

            networkModelUTM.Switches.Switches.ForEach(switchUTM =>
            {
                UTMToLatitudeLongitude(switchUTM.X, switchUTM.Y, out double latitude, out double longitude);

                if (!double.IsNaN(latitude) && !double.IsNaN(longitude))
                {
                    SwitchGeo switchGeo = new SwitchGeo();
                    switchGeo.Id = switchUTM.Id;
                    switchGeo.Name = switchUTM.Name;
                    switchGeo.Status = switchUTM.Status;
                    switchGeo.Latitude = latitude;
                    switchGeo.Longitude = longitude;

                    networkModelGeo.Switches.Switches.Add(switchGeo);
                }
            });

            networkModelGeo.Lines = new LinesGeo() { Lines = new List<LineGeo>() };

            networkModelUTM.Lines.Lines.ForEach(lineUTM =>
            {
                bool isFirstEndSubstation = networkModelGeo.Substations.Substations.Any(s => s.Id == lineUTM.FirstEnd);
                bool isSecondEndSubstation = networkModelGeo.Substations.Substations.Any(s => s.Id == lineUTM.SecondEnd);
                bool isFirstEndNode = networkModelGeo.Nodes.Nodes.Any(n => n.Id == lineUTM.FirstEnd);
                bool isSecondEndNode = networkModelGeo.Nodes.Nodes.Any(n => n.Id == lineUTM.SecondEnd);
                bool isFirstEndSwitch = networkModelGeo.Switches.Switches.Any(s => s.Id == lineUTM.FirstEnd);
                bool isSecondEndSwitch = networkModelGeo.Switches.Switches.Any(s => s.Id == lineUTM.SecondEnd);
                bool hasFirstEnd = isFirstEndSubstation || isFirstEndNode || isFirstEndSwitch;
                bool hasSecondEnd = isSecondEndSubstation || isSecondEndNode || isSecondEndSwitch;

                if (hasFirstEnd && hasSecondEnd)
                {
                    LineGeo lineGeo = new LineGeo();
                    lineGeo.Vertices = new VerticesGeo() { Points = new List<PointGeo>() };

                    lineUTM.Vertices.Points.ForEach(pointUTM =>
                    {
                        UTMToLatitudeLongitude(pointUTM.X, pointUTM.Y, out double latitude, out double longitude);

                        if (!double.IsNaN(latitude) && !double.IsNaN(longitude))
                        {
                            PointGeo pointGeo = new PointGeo();
                            pointGeo.Latitude = latitude;
                            pointGeo.Longitude = longitude;

                            lineGeo.Vertices.Points.Add(pointGeo);
                        }
                    });

                    if (lineGeo.Vertices.Points.Count >= 2)
                    {
                        lineGeo.Id = lineUTM.Id;
                        lineGeo.Name = lineUTM.Name;
                        lineGeo.IsUnderground = lineUTM.IsUnderground;
                        lineGeo.R = lineUTM.R;
                        lineGeo.ConductorMaterial = lineUTM.ConductorMaterial;
                        lineGeo.LineType = lineUTM.LineType;
                        lineGeo.ThermalConstantHeat = lineUTM.ThermalConstantHeat;
                        lineGeo.FirstEnd = lineUTM.FirstEnd;
                        lineGeo.SecondEnd = lineUTM.SecondEnd;

                        networkModelGeo.Lines.Lines.Add(lineGeo);
                    }
                }
            });

            return networkModelGeo;
        }

        public void ExportNetworkModelGeoToXml(NetworkModelGeo networkModelGeo)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NetworkModelGeo));
            using (FileStream fileStream = new FileStream(Config.dataGeoPath, FileMode.Create))
            {
                serializer.Serialize(fileStream, networkModelGeo);
            }
        }

        public MapNetworkModel ConvertNetworkModelGeoToMap(NetworkModelGeo networkModelGeo)
        {
            MapNetworkModel mapNetworkModel = new MapNetworkModel();

            mapNetworkModel.Points = new List<MapPoint>();

            networkModelGeo.Substations.Substations.ForEach(substationGeo =>
            {
                MapPoint mapPoint = new MapPoint();
                mapPoint.Id = substationGeo.Id;
                mapPoint.Description = substationGeo.Name;
                mapPoint.Type = PointType.Substation;

                LatitudeLongitudeToPixels(substationGeo.Latitude, substationGeo.Longitude, out double x, out double y);
                mapPoint.X = x;
                mapPoint.Y = y;

                mapPoint.Connections = new List<long>();

                networkModelGeo.Lines.Lines.ForEach(lineGeo =>
                {
                    if (lineGeo.FirstEnd == mapPoint.Id || lineGeo.SecondEnd == mapPoint.Id)
                    {
                        mapPoint.Connections.Add(lineGeo.Id);
                    }
                });

                mapNetworkModel.Points.Add(mapPoint);
            });

            networkModelGeo.Nodes.Nodes.ForEach(nodeGeo =>
            {
                MapPoint mapPoint = new MapPoint();
                mapPoint.Id = nodeGeo.Id;
                mapPoint.Description = nodeGeo.Name;
                mapPoint.Type = PointType.Node;

                LatitudeLongitudeToPixels(nodeGeo.Latitude, nodeGeo.Longitude, out double x, out double y);
                mapPoint.X = x;
                mapPoint.Y = y;

                mapPoint.Connections = new List<long>();

                networkModelGeo.Lines.Lines.ForEach(lineGeo =>
                {
                    if (lineGeo.FirstEnd == mapPoint.Id || lineGeo.SecondEnd == mapPoint.Id)
                    {
                        mapPoint.Connections.Add(lineGeo.Id);
                    }
                });

                mapNetworkModel.Points.Add(mapPoint);
            });

            networkModelGeo.Switches.Switches.ForEach(switchGeo =>
            {
                MapPoint mapPoint = new MapPoint();
                mapPoint.Id = switchGeo.Id;
                mapPoint.Description = switchGeo.Name;
                mapPoint.Type = PointType.Switch;

                LatitudeLongitudeToPixels(switchGeo.Latitude, switchGeo.Longitude, out double x, out double y);
                mapPoint.X = x;
                mapPoint.Y = y;

                mapPoint.Connections = new List<long>();

                networkModelGeo.Lines.Lines.ForEach(lineGeo =>
                {
                    if (lineGeo.FirstEnd == mapPoint.Id || lineGeo.SecondEnd == mapPoint.Id)
                    {
                        mapPoint.Connections.Add(lineGeo.Id);
                    }
                });

                mapNetworkModel.Points.Add(mapPoint);
            });

            mapNetworkModel.Lines = new List<MapLine>();

            networkModelGeo.Lines.Lines.ForEach(lineGeo => 
            {
                MapLine mapLine = new MapLine();
                mapLine.Id = lineGeo.Id;
                mapLine.Description = lineGeo.Name;

                mapLine.Points = new List<MapPoint>();

                MapPoint firstMapPoint = mapNetworkModel.Points.Find(p => p.Id == lineGeo.FirstEnd);
                mapLine.Points.Add(firstMapPoint);

                lineGeo.Vertices.Points.ForEach(pointGeo =>
                {
                    LatitudeLongitudeToPixels(pointGeo.Latitude, pointGeo.Longitude, out double x, out double y);

                    MapPoint mapPoint = mapNetworkModel.Points.FirstOrDefault(p => p.X == x && p.Y == y);

                    if (mapPoint == null)
                    {
                        mapPoint = new MapPoint();
                        mapPoint.Id = 0;
                        mapPoint.Description = lineGeo.Name + "_connector";
                        mapPoint.Type = PointType.Connector;
                        mapPoint.X = x;
                        mapPoint.Y = y;

                        mapNetworkModel.Points.Add(mapPoint);
                    }

                    mapLine.Points.Add(mapPoint);
                });

                MapPoint secondMapPoint = mapNetworkModel.Points.Find(p => p.Id == lineGeo.SecondEnd);
                mapLine.Points.Add(secondMapPoint);

                mapNetworkModel.Lines.Add(mapLine);
            });

            return mapNetworkModel;
        }

        public void ExportMapNetworkModelToXml(MapNetworkModel mapNetworkModel)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MapNetworkModel));
            using (FileStream fileStream = new FileStream(Config.dataMapPath, FileMode.Create))
            {
                serializer.Serialize(fileStream, mapNetworkModel);
            }
        }

        private void UTMToLatitudeLongitude(double xUTM, double yUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = Config.zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = xUTM - 500000;
            var y = isNorthHemisphere ? yUTM : yUTM - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;

            if (latitude <= Config.minLatitude || latitude >= Config.maxLatitude)
                latitude = double.NaN;

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;

            if (longitude <= Config.minLongitude || longitude >= Config.maxLongitude)
                longitude = double.NaN;
        }

        private void LatitudeLongitudeToPixels(double latitude, double longitude, out double x, out double y)
        {
            double xMultiplier = Config.mapWidth / (Config.maxLongitude - Config.minLongitude);
            double yMultiplier = Config.mapHeight / (Config.maxLatitude - Config.minLatitude);

            x = Math.Round((longitude - Config.minLongitude) * xMultiplier, 0);
            y = Math.Round((latitude - Config.minLatitude) * yMultiplier, 0);
        }
    }
}
