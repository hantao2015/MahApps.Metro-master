using MiniUiAppCode;
using System.Runtime.InteropServices;

namespace MetroDemo.ExampleWindows
{
    public partial class InteropDemo
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        public string m_cookie = "";
        public InteropDemo()
        {
            InitializeComponent();
            string url = "http://121.199.9.136:8082/rispweb/rispservice/ajaxSvrLogin.aspx";
            //WebBrowser.Document.Cookie = RealsunClientNet.m_CookieContainer.GetCookies(new System.Uri(url));
            m_cookie = RealsunClientNet.m_CookieContainer.GetCookies(new System.Uri(url))[0].ToString();
            setcookie(m_cookie);
            WebBrowser.Url = new System.Uri("http://121.199.9.136:8082/rispweb/risphost/MiniFrame.aspx");
        }
        private void setcookie(string c)
        {
            string[] item = c.Split('=');
            if (item.Length == 2)
            {
                string name = item[0];
                string value = item[1];
                InternetSetCookie("http://121.199.9.136:8082/rispweb/risphost/MiniFrame.aspx", name, value);
               
        }

    }
}
}
