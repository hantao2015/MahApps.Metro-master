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
using MiniUiAppCode;
 


namespace MetroDemo
{
    /// <summary>
    /// Interaction logic for Test.xaml
    /// </summary>
    public partial class Test 
    {

        private MediaClock clock;

        private readonly MainWindowViewModel _viewModel;
        private bool  isLoginbuttonClicked=false;
        private static CookieContainer m_CookieContainer = new CookieContainer();
        private static string m_strLoginUrl = "http://121.199.9.136:8082/rispweb/rispservice/ajaxSvrLogin.aspx";
        public Test()
        {
            _viewModel = new MainWindowViewModel(DialogCoordinator.Instance);
            DataContext = _viewModel;
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
            clock.Completed += Clock_Completed;
           


        }

        private void Clock_Completed(object sender, EventArgs e)
        {
            //button_Click.
         if (isLoginbuttonClicked==false)
            {
                LoginAsyn();
            }
          

        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (welcomepage.IsSelected)
            {
                clock.Controller.Stop();

                clock.Controller.Begin();
            }
        }

         

        private   void button_Click(object sender, RoutedEventArgs e)

        {
            isLoginbuttonClicked = true;
              LoginAsyn();
        }
        private async void LoginAsyn()
        {
            LoginDialogSettings aSetting = new LoginDialogSettings { ColorScheme = MetroDialogOptions.ColorScheme, InitialUsername = "001", NegativeButtonVisibility = Visibility.Visible, EnablePasswordPreview = true };
            LoginDialogData result = await this.ShowLoginAsync("登入验证", "输入用户名和密码", aSetting);

            if (result == null)
            {
                //User pressed cancel
            }
            else
            {
                string strurl = m_strLoginUrl;
                Encoding encoding = Encoding.UTF8;

                try
                {
                    var controller = await this.ShowProgressAsync("请稍后...", "正在登入系统!");
                    controller.SetIndeterminate();
                    controller.SetCancelable(true);
                    // Hashtable loginReturnData = await RealsunClientNet.Login(result.Username, result.Password);
                    Hashtable loginReturnData = new Hashtable () ;
                    loginReturnData.Add("error", 0);
                    if (Convert.ToInt16(loginReturnData["error"]) == 0)
                    {
                        await controller.CloseAsync();
                        homepage.IsEnabled = true;
                        homepage.IsSelected = true;
                        clock.Controller.Stop();
                        Tiles.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        await controller.CloseAsync();
                        homepage.IsEnabled = false;
                        homepage.IsSelected = false;
                        welcomepage.IsSelected = true;
                        clock.Controller.Begin();
                        Tiles.Visibility = Visibility.Hidden;

                        await this.ShowMessageAsync("登入失败", Convert.ToString(loginReturnData["message"]));

                    }



                }
                catch (Exception)
                {


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