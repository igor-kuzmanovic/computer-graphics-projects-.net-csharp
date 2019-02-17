using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double noviSadLat = 45.267136;
        const double noviSadLng = 19.833549;
        const int noviSadUTMZone = 34;

        readonly GMarkerGoogleType substationMarkerType = GMarkerGoogleType.red_small;
        readonly GMarkerGoogleType nodeMarkerType = GMarkerGoogleType.green_small;
        readonly GMarkerGoogleType switchMarkerType = GMarkerGoogleType.blue_small;
        readonly Color lineColor = Color.Yellow;

        private NetworkModel networkModel = new Data().NetworkModel;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGMap();
            InitializeMarkersAndRoutes();
        }

        #region Initialization

        private void InitializeGMap()
        {
            GMapProvider.WebProxy = WebRequest.GetSystemWebProxy();
            GMapProvider.WebProxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            gmap.MapProvider = BingHybridMapProvider.Instance;

            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            gmap.DragButton = MouseButtons.Left;
            gmap.Position = new PointLatLng(noviSadLat, noviSadLng);
            gmap.ShowCenter = false;
        }

        private void InitializeMarkersAndRoutes()
        {
            GenerateSubstationMarkers();
            GenerateNodeMarkers();
            GenerateSwitchMarkers();
            GenerateLineRoutes();
        }

        #endregion

        #region MarkerAndRouteGeneration

        private void GenerateSubstationMarkers()
        {
            GMapOverlay markers = new GMapOverlay("substations")
            {
                IsVisibile = false
            };

            networkModel.Substations.SubstationEntity.ForEach(s => 
            {
                PointLatLng pointLatLng = GetLatLngPoint(s.X, s.Y);
                GMapMarker marker = new GMarkerGoogle(pointLatLng, substationMarkerType)
                {
                    ToolTipText = s.ToString()
                };

                markers.Markers.Add(marker);
            });

            gmap.Overlays.Add(markers);
        }

        private void GenerateNodeMarkers()
        {
            GMapOverlay markers = new GMapOverlay("nodes")
            {
                IsVisibile = false
            };

            networkModel.Nodes.NodeEntity.ForEach(n => 
            {
                PointLatLng pointLatLng = GetLatLngPoint(n.X, n.Y);
                GMapMarker marker = new GMarkerGoogle(pointLatLng, nodeMarkerType)
                {
                    ToolTipText = n.ToString()
                };

                markers.Markers.Add(marker);
            });

            gmap.Overlays.Add(markers);
        }

        private void GenerateSwitchMarkers()
        {
            GMapOverlay markers = new GMapOverlay("switches")
            {
                IsVisibile = false
            };

            networkModel.Switches.SwitchEntity.ForEach(s => 
            {
                PointLatLng pointLatLng = GetLatLngPoint(s.X, s.Y);
                GMapMarker marker = new GMarkerGoogle(pointLatLng, switchMarkerType)
                {
                    ToolTipText = s.ToString()
                };

                markers.Markers.Add(marker);
            });

            gmap.Overlays.Add(markers);
        }

        private void GenerateLineRoutes()
        {
            GMapOverlay routes = new GMapOverlay("lines")
            {
                IsVisibile = false
            };

            networkModel.Lines.LineEntity.ForEach(l => 
            {
                List<PointLatLng> points = new List<PointLatLng>();
                l.Vertices.Point.ForEach(p =>
                {
                    points.Add(GetLatLngPoint(p.X, p.Y));
                });

                GMapRoute route = new GMapRoute(points, l.Name)
                {
                    Stroke = new Pen(lineColor, 1)
                };

                routes.Routes.Add(route);
            });

            gmap.Overlays.Add(routes);
        }

        #endregion

        #region UTMPointHelper

        private PointLatLng GetLatLngPoint(double utmX, double utmY, int utmZone = noviSadUTMZone)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = utmZone;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

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

            var latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
            var longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;

            return new PointLatLng(latitude, longitude);
        }

        #endregion

        #region CheckBoxEvents

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ToggleButton checkBox = (ToggleButton)sender;
            gmap.Overlays.Single(o => o.Id == checkBox.Name).IsVisibile = true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ToggleButton checkBox = (ToggleButton)sender;
            gmap.Overlays.Single(o => o.Id == checkBox.Name).IsVisibile = false;
        }

        #endregion
    }
}
