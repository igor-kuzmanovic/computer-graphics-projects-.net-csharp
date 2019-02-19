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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int networkGridColumns = 300;
        const int networkGridRows = 170;

        const string filePath =  @"../../Data/Geographic.xml";
        Network network;

        public MainWindow()
        {
            InitializeComponent();
        }

        void Window_Loaded(object sender, RoutedEventArgs e)
        {
            network = new Network(filePath);
            LoadPoints();
            LoadLines();
        }

        private void LoadPoints()
        {
            network.Points.ForEach(p =>
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Height = 4;
                rectangle.Width = 4;
                rectangle.SetValue(Canvas.LeftProperty, p.X);
                rectangle.SetValue(Canvas.TopProperty, p.Y);

                ToolTip toolTip = new ToolTip();
                toolTip.Content = p.Name;
                rectangle.ToolTip = toolTip;

                switch (p.Type)
                {
                    case GridPointType.Substation:
                        rectangle.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                        break;
                    case GridPointType.Node:
                        rectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                        break;
                    case GridPointType.Switch:
                        rectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 255));
                        break;
                }

                NetworkGrid.Children.Add(rectangle);
            });
        }

        private void LoadLines()
        {
            network.Lines.ForEach(l =>
            {
                l.Points.ForEach(p => Trace.Write($"{p.X} {p.Y}"));
                Trace.WriteLine("");

                Line line = new Line();
                line.X1 = l.Points.First().X;
                line.Y1 = l.Points.First().Y;
                line.X2 = l.Points.Last().X;
                line.Y2 = l.Points.Last().Y;

                line.Stroke = new SolidColorBrush(Color.FromRgb(127, 127, 127));
                line.StrokeThickness = 2;

                NetworkGrid.Children.Add(line);
            });
        }
    }
}
