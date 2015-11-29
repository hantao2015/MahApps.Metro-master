using System;
using System.Windows.Controls;
using System.Windows;

namespace MetroDemo.ExampleViews
{
    /// <summary>
    /// Interaction logic for TilesExample.xaml
    /// </summary>
    public partial class TilesExample : UserControl
    {
        public TilesExample()
        {
            InitializeComponent();
        }

        private void Tile_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("title");
        }
    }
}
