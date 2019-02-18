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

            network.Points.ForEach(p =>
            {
                Rectangle rectangle = new Rectangle();
                rectangle.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                rectangle.Height = 1;
                rectangle.Width = 1;
                rectangle.SetValue(Canvas.LeftProperty, p.X);
                rectangle.SetValue(Canvas.BottomProperty, p.Y);
                NetworkGrid.Children.Add(rectangle);
            });
        }
    }
}
