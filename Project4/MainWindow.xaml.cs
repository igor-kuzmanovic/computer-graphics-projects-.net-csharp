using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Point start = new Point();
        private Point diffOffset = new Point();
        private double startAngleX = 0;
        private double startAngleY = 0;
        private int zoomCurrent = 1;
        private int zoomMax = 7;
        private GeometryModel3D hitGeo;
        private MapNetworkModel mapNetworkModel;
        private Dictionary<GeometryModel3D, string> toolTips;

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
            mapNetworkModel = dataHelper.ConvertNetworkModelGeoToMap(networkModelGeo);
            dataHelper.ExportMapNetworkModelToXml(mapNetworkModel);
            Draw3DLines();
            Draw3DPoints();
        }

        private void Draw3DPoints()
        {
            toolTips = new Dictionary<GeometryModel3D, string>();
            mapNetworkModel.Points.ForEach(point =>
            {
                if (point.Type != PointType.Connector)
                {
                    int side = 6;
                    int height = 0;

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
                    toolTips.Add(model, point.Id + " " + point.Type + " " + point.Description);
                    Map.Children.Add(model);
                }
            });          
        }

        private void Draw3DLines()
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

        private void Viewport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Viewport.CaptureMouse();
            start = e.GetPosition(this);
            diffOffset.X = Translation.OffsetX;
            diffOffset.Y = Translation.OffsetY;

            Point mouseposition = e.GetPosition(Viewport);
            Point3D testpoint3D = new Point3D(mouseposition.X, mouseposition.Y, 0);
            Vector3D testdirection = new Vector3D(mouseposition.X, mouseposition.Y, 10);

            PointHitTestParameters pointparams =
                     new PointHitTestParameters(mouseposition);
            RayHitTestParameters rayparams =
                     new RayHitTestParameters(testpoint3D, testdirection);

            hitGeo = null;
            VisualTreeHelper.HitTest(Viewport, null, HTResult, pointparams);
        }

        private void Viewport_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Viewport.ReleaseMouseCapture();
        }

        private void Viewport_MouseMove(object sender, MouseEventArgs e)
        {
            Point end = e.GetPosition(this);
            double offsetX = end.X - start.X;
            double offsetY = end.Y - start.Y;
            double w = this.Width;
            double h = this.Height;
            double translateX = (offsetX * 100) / w;
            double translateY = -(offsetY * 100) / h;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Viewport.IsMouseCaptured)
                {
                    Translation.OffsetX = diffOffset.X + (translateX * 1175 / (100 * Scaling.ScaleX));
                    Translation.OffsetY = diffOffset.Y + (translateY * 775 / (100 * Scaling.ScaleX));
                }
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            { 
                if (Viewport.IsMouseCaptured)
                {
                    XRotation.Angle = startAngleX + offsetY * 0.1;
                    YRotation.Angle = startAngleY + offsetX * 0.1;
                }
            }
        }

        private void Viewport_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.MouseDevice.GetPosition(this);
            double scaleX = 1;
            double scaleY = 1;
            if (e.Delta > 0 && zoomCurrent < zoomMax)
            {
                scaleX = Scaling.ScaleX + 0.1;
                scaleY = Scaling.ScaleY + 0.1;
                zoomCurrent++;
                Scaling.ScaleX = scaleX;
                Scaling.ScaleY = scaleY;
            }
            else if (e.Delta <= 0 && zoomCurrent > -zoomMax)
            {
                scaleX = Scaling.ScaleX - 0.1;
                scaleY = Scaling.ScaleY - 0.1;
                zoomCurrent--;
                Scaling.ScaleX = scaleX;
                Scaling.ScaleY = scaleY;
            }
        }

        private void Viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                Viewport.CaptureMouse();
                start = e.GetPosition(this);
                startAngleX = XRotation.Angle;
                startAngleY = YRotation.Angle;
            }
        }

        private void Viewport_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Released)
            {
                Viewport.ReleaseMouseCapture();
            }
        }

        private HitTestResultBehavior
        HTResult(HitTestResult rawresult)
        {
            RayHitTestResult rayResult = rawresult as RayHitTestResult;

            if (rayResult != null)
            {
                bool gasit = false;

                try
                {
                    for (int i = 1; i < Map.Children.Count; i++)
                    {
                        GeometryModel3D model = (GeometryModel3D)Map.Children[i];

                        if (model == rayResult.ModelHit)
                        {
                            hitGeo = (GeometryModel3D)rayResult.ModelHit;
                            gasit = true;

                            string text = toolTips[model];
                            TooltipText.Visibility = Visibility.Visible;
                            TooltipText.Text = text;
                        }
                    }
                } catch (Exception) { }
                
                if (!gasit)
                {
                    hitGeo = null;

                    TooltipText.Visibility = Visibility.Hidden;
                    TooltipText.Text = "";
                }
            }

            return HitTestResultBehavior.Stop;
        }
    }
}
