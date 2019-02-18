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

namespace Project3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int networkGridColumns = 267;
        const int networkGridRows = 150;

        public MainWindow()
        {
            InitializeComponent();

            Random random = new Random();
            byte[] rgbValues = new byte[3];

            ICollection<Rectangle> rectangles = new List<Rectangle>(networkGridColumns * networkGridRows);
            for (int i = 0; i < networkGridColumns; i++)
            {
                for (int j = 0; j < networkGridRows; j++)
                {
                    random.NextBytes(rgbValues);
                    Color color = Color.FromRgb(rgbValues[0], rgbValues[1], rgbValues[2]);

                    Rectangle rectangle = new Rectangle();
                    rectangle.Fill = new SolidColorBrush(color);
                    rectangle.SetValue(Grid.ColumnProperty, i);
                    rectangle.SetValue(Grid.RowProperty, j);
                    NetworkGrid.Children.Add(rectangle);                  
                }
            }

            for (int i = 0; i < networkGridColumns; i++)
            {
                ColumnDefinition column = new ColumnDefinition();
                column.Width = new GridLength(1, GridUnitType.Star);
                NetworkGrid.ColumnDefinitions.Add(column);
            }

            for (int i = 0; i < networkGridRows; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                NetworkGrid.RowDefinitions.Add(row);
            }
        }
    }
}
