using System;
using System.Windows.Controls;
using System.Windows;
 
using MetroDemo.ExampleWindows;

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
            new InteropDemo().Show();
        }
        private   void Tile_Click2(object sender, System.Windows.RoutedEventArgs e)
        {
          //  new InteropDemo2().Show();
         
        }
    }
}
