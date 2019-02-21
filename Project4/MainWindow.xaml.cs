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
            Generate2DMap(mapNetworkModel);
        }

        private void Generate2DMap(MapNetworkModel mapNetworkModel)
        {
            mapNetworkModel.Lines.ForEach(l =>
            {
                for (int i = 0; i < l.Points.Count - 1; i++)
                {
                    Line line = new Line();
                    line.X1 = l.Points[i].X;
                    line.Y1 = l.Points[i].Y;
                    line.X2 = l.Points[i + 1].X;
                    line.Y2 = l.Points[i + 1].Y;

                    line.Stroke = new SolidColorBrush(Color.FromRgb(0, 72, 186));
                    line.Stroke.Opacity = 0.5;
                    line.StrokeThickness = 2;

                    NetworkGrid.Children.Add(line);
                }
            });

            mapNetworkModel.Points.ForEach(p =>
            {
                Ellipse ellipse = new Ellipse();
                ellipse.Height = 5;
                ellipse.Width = 5;
                ellipse.SetValue(Canvas.LeftProperty, p.X - 2);
                ellipse.SetValue(Canvas.TopProperty, p.Y - 2);

                ToolTip toolTip = new ToolTip();
                toolTip.Content = p.Description;
                ellipse.ToolTip = toolTip;

                switch (p.Connectivity)
                {
                    case Connectivity.Low:
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        ellipse.Opacity = 1.0 / 3.0;
                        break;
                    case Connectivity.Medium:
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        ellipse.Opacity = 2.0 / 3.0;
                        break;
                    case Connectivity.High:
                        ellipse.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        ellipse.Opacity = 1;
                        break;
                }

                NetworkGrid.Children.Add(ellipse);
            });
        }
    }
}
