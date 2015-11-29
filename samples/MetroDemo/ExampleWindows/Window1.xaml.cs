using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
namespace MetroDemo.ExampleWindows
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private MediaClock clock;
        public Window1()
        {
            InitializeComponent();
            MediaTimeline timeline = new MediaTimeline(new Uri("Wildlife.wmv", UriKind.RelativeOrAbsolute));
            clock = timeline.CreateClock();//创建控制时钟

            MediaElement mediaElement = Resources["video"] as MediaElement;//得到资源
            orgin.Child = mediaElement;
            mediaElement.Clock = clock;
            clock.Controller.Seek(new TimeSpan(0, 0, 0, 2), TimeSeekOrigin.BeginTime);//跳过固定的时间线

            clock.Controller.Stop();

            clock.Controller.Begin();
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {

        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void resume_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
