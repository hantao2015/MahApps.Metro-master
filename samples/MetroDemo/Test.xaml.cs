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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MiniUiAppCode.Utils;
using System.Web;
using System.Collections;
using System.Net;
using System.Threading.Tasks;
using MetroDemo.ExampleViews;
using MetroDemo.ExampleWindows;
using MahApps.Metro;
using System.Timers;
using Renderer.Core;
using System.Runtime.InteropServices;
using System.Windows.Media.Animation;
namespace MetroDemo
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test : MetroWindow
    {

        private MediaClock clock;


 
        private static CookieContainer m_CookieContainer = new CookieContainer();
        private static string m_strLoginUrl = "http://121.199.9.136:8082/rispweb/rispservice/ajaxSvrLogin.aspx";
        public Test()
        {
            InitializeComponent();
            var accent = ThemeManager.Accents.First(x => x.Name == "Purple");
            var theme = ThemeManager.GetAppTheme("BaseLight");
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme);

            TaskEx.Delay(2000);
            homepage.IsEnabled = false;

            MediaTimeline timeline = new MediaTimeline(new Uri("Wildlife.wmv", UriKind.RelativeOrAbsolute));
            clock = timeline.CreateClock();//创建控制时钟

            MediaElement mediaElement = Resources["video"] as MediaElement;//得到资源
            orgin.Child = mediaElement;
            mediaElement.Clock = clock;
            clock.Controller.Seek(new TimeSpan(0, 0, 0, 2), TimeSeekOrigin.BeginTime);//跳过固定的时间线

            clock.Controller.Stop();

            clock.Controller.Begin();

            MainTabControl.SelectionChanged += MainTabControl_SelectionChanged;

        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (welcomepage.IsSelected)
            {
                clock.Controller.Stop();

                clock.Controller.Begin();
            }
        }

         

        private async void button_Click(object sender, RoutedEventArgs e)

        {
            LoginDialogSettings a = new LoginDialogSettings { ColorScheme = MetroDialogOptions.ColorScheme, InitialUsername = "MahApps", NegativeButtonVisibility = Visibility.Visible, EnablePasswordPreview = true };
            LoginDialogData result = await this.ShowLoginAsync("登入验证", "输入用户名和密码", a);

            if (result == null)
            {
                //User pressed cancel
            }
            else
            {
                // result.Username
                //result.Password
                string strurl = m_strLoginUrl;//?user=001&upass=123456";
                Encoding encoding = Encoding.UTF8;
                //MessageDialogResult messageResult = await this.ShowMessageAsync("Authentication Information", String.Format("Username: {0}\nPassword: {1}", result.Username, result.Password));

                /*ttp://121.199.9.136:8082/rispweb/DYBService/homepage.asmx/saveMeasureB"*/
                try
                {
                    var controller = await this.ShowProgressAsync("请稍后...", "正在登入系统!");
                    controller.SetIndeterminate();
                    await TaskEx.Delay(5000);
                    controller.SetCancelable(true);
                    string strJason = PostHttpResponse.GetStream(PostHttpResponse.CreatePostHttpResponseJson(strurl, "", "user=001&upass=123456&clienttype=mobile", null, "", encoding, "", ref m_CookieContainer, true), encoding);
                    Hashtable hs = (Hashtable)MiniUiAppCode.JSON.Decode(strJason);




                    await controller.CloseAsync();


                    //MessageDialogResult messageResult = await this.ShowMessageAsync("Authentication Information", "登入成功");
                    //  this.timer.Stop();
                    homepage.IsEnabled = true;
                    homepage.IsSelected = true;
                    clock.Controller.Stop();
                    Tiles.Visibility = Visibility.Visible;


                }
                catch (Exception)
                {

                    throw;
                }

            }
        }

        private async void button2_Click(object sender, RoutedEventArgs e)
        {
            // long resid = 498259129115;
            Encoding encoding = Encoding.UTF8;
            string strurl = "http://121.199.9.136:8082/rispweb/risphost/data/AjaxService.aspx";
            // ?method = ShowHostTableDatas_Ajax
            if (!(m_CookieContainer == null))
            {

                try
                {
                    string strJason = PostHttpResponse.GetStream(PostHttpResponse.CreatePostHttpResponseJson(strurl, "", "method=ShowHostTableDatas_Ajax&resid=498259129115", null, "", encoding, "", ref m_CookieContainer), encoding);
                    Hashtable hs = (Hashtable)MiniUiAppCode.JSON.Decode(strJason);

                }
                catch (Exception ex)
                {
                    await this.ShowMessageAsync("", ex.Message.ToString());

                    throw;
                }
            }

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w_main = new MainWindow();
            w_main.Show();

        }

       
    }
}