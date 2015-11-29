using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using MiniUiAppCode.Utils;

namespace MiniUiAppCode
{
   public static class RealsunClientNet
    {
        public  static CookieContainer m_CookieContainer = new CookieContainer();
       
        private static string strBaseUrl = "http://121.199.9.136:8082/rispweb/";
        private const  string strLoginPage = "rispservice/ajaxSvrLogin.aspx";
        public static Task<Hashtable>  Login(string user, string upass)
        {
            var result = Task<Hashtable>.Factory.StartNew(() =>
            {
                Hashtable loginReturnData = new Hashtable() ;
                try
                {
                    Encoding encoding = Encoding.UTF8;
                    string strurl = strBaseUrl + strLoginPage;

                    string strJason = PostHttpResponse.GetStream(PostHttpResponse.CreatePostHttpResponseJson(strurl, "", "user="+user+"&upass="+upass+"&clienttype=mobile", null, "", encoding, "", ref m_CookieContainer, true), encoding);
                    loginReturnData = (Hashtable)MiniUiAppCode.JSON.Decode(strJason);

                }
                catch (Exception ex)
                {

                    loginReturnData.Clear();
                    loginReturnData.Add("error", "-1");
                    loginReturnData.Add("message", "client error:"+ex.Message.ToString());
                }
                return loginReturnData;
            });
            return result;

        }
        

    }
}
