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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();         
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataHelper dataHelper = new DataHelper();
            NetworkModelUTM networkModelUTM = dataHelper.ImportNetworkModelUTMFromXML();
            NetworkModelGeo networkModelGeo = dataHelper.ConvertNetworkModelUTMToGeo(networkModelUTM);
            dataHelper.ExportNetworkModelGeoToXml(networkModelGeo);
            MapNetworkModel mapNetworkModel = dataHelper.ConvertNetworkModelGeoToMap(networkModelGeo);
            dataHelper.ExportMapNetworkModelToXml(mapNetworkModel);
            Draw3DLines(mapNetworkModel);
            Draw3DPoints(mapNetworkModel);
        }

        private void Draw3DPoints(MapNetworkModel mapNetworkModel)
        {
            mapNetworkModel.Points.ForEach(point =>
            {
                int side = 6;
                int height = 0;
                if (point.Type == PointType.Connector)
                {
                    side = 2;
                    height = 2;
                }

                GeometryModel3D model = new GeometryModel3D();
                model.Geometry = new MeshGeometry3D()
                {
                    Positions = new Point3DCollection()
                    {
                        new Point3D(point.X + side, point.Y + side, height + side),
                        new Point3D(point.X, point.Y + side, height + side),
                        new Point3D(point.X, point.Y, height + side),
                        new Point3D(point.X + side, point.Y, height + side),
                        new Point3D(point.X + side, point.Y + side, height),
                        new Point3D(point.X, point.Y + side, height),
                        new Point3D(point.X, point.Y, height),
                        new Point3D(point.X + side, point.Y, height),
                    },
                    TriangleIndices = new Int32Collection()
                    {
                        0,1,2,
                        0,2,3,
                        4,7,6,
                        4,6,5,
                        4,0,3,
                        4,3,7,
                        1,5,6,
                        1,6,2,
                        1,0,4,
                        1,4,5,
                        2,6,7,
                        2,7,3
                    }
                };
                model.Material = new DiffuseMaterial()
                {
                    Brush = new SolidColorBrush()
                    {
                        Color = GetColorFromConnectivity(point.Connectivity)
                    }
                };
                Map.Children.Add(model);
            });          
        }

        private void Draw3DLines(MapNetworkModel mapNetworkModel)
        {
            int side = 2;
            int height = 2;

            mapNetworkModel.Lines.ForEach(line =>
            {
                for (int i = 0; i < line.Points.Count - 1; i++)
                {
                    MapPoint startPoint = line.Points[i];
                    MapPoint endPoint = line.Points[i + 1];
                    GeometryModel3D model = new GeometryModel3D();
                    model.Geometry = new MeshGeometry3D()
                    {
                        Positions = new Point3DCollection()
                        {
                            new Point3D(startPoint.X + side, startPoint.Y + side, height + side),
                            new Point3D(startPoint.X, startPoint.Y + side, height + side),
                            new Point3D(endPoint.X, endPoint.Y, height + side),
                            new Point3D(endPoint.X + side, endPoint.Y, height + side),
                            new Point3D(startPoint.X + side, startPoint.Y + side, height),
                            new Point3D(startPoint.X, startPoint.Y + side, height),
                            new Point3D(endPoint.X, endPoint.Y, height),
                            new Point3D(endPoint.X + side, endPoint.Y, height),
                        },
                        TriangleIndices = new Int32Collection()
                        {
                            0,1,2,
                            0,2,3,
                            4,7,6,
                            4,6,5,
                            4,0,3,
                            4,3,7,
                            1,5,6,
                            1,6,2,
                            1,0,4,
                            1,4,5,
                            2,6,7,
                            2,7,3
                        }
                    };
                    model.Material = new DiffuseMaterial()
                    {
                        Brush = new SolidColorBrush()
                        {
                            Color = Color.FromRgb(0, 127, 255)
                        }
                    };
                    Map.Children.Add(model);
                }
            });
        }

        private Color GetColorFromConnectivity(Connectivity connectivity)
        {
            switch (connectivity)
            {
                case Connectivity.High:
                    return Color.FromRgb(127, 0, 0);
                case Connectivity.Medium:
                    return Color.FromRgb(255, 0, 0);
                case Connectivity.Low:
                    return Color.FromRgb(255, 127, 127);
                case Connectivity.None:
                default:
                    return Color.FromRgb(0, 127, 255);
            }
        }
    }
}
