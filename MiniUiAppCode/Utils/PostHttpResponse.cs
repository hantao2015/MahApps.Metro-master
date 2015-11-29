
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace MiniUiAppCode.Utils
{


    
    public class PostHttpResponse
        {
        private static CookieContainer m_Cookie = new CookieContainer();
        #region Static Field
        private static readonly string DefaultUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
            #endregion

            #region public Method
            public static string PostHttpResponseJson(string url, ref CookieContainer Cookie)
            {
                string json = string.Empty;
                Encoding encoding = Encoding.UTF8;
                HttpWebResponse Response = CreatePostHttpResponseJson(url, null, null, null, null, encoding,null,ref Cookie);
                json = GetStream(Response, encoding);
                return json;
            }

            public static string PostHttpResponseJson(string url, string postJson, ref CookieContainer Cookie)
            {
                string json = string.Empty;
                Encoding encoding = Encoding.UTF8;
                HttpWebResponse Response = CreatePostHttpResponseJson(url, postJson, null, null, null, encoding, null,ref  Cookie);
                json = GetStream(Response, encoding);
                return json;
            }



            /// <summary>
            /// 创建POST方式Json数据的HTTP请求（包括了https站点请求） 
            /// </summary>
            /// <param name="url">请求的URL</param>
            /// <param name="parameters">随同请求POST的参数名称及参数值字典</param>
            /// <param name="timeout">请求的超时时间</param>
            /// <param name="userAgent">请求的客户端浏览器信息，可以为空</param>
            /// <param name="requestEncoding">发送HTTP请求时所用的编码</param>
            /// <param name="cookies">随同HTTP请求发送的Cookie信息，如果不需要身份验证可以为空</param>
            /// <returns></returns>
            public static   HttpWebResponse CreatePostHttpResponseJson(string url, string postJson, string parameters, int? timeout, string userAgent, Encoding requestEncoding, string referer, ref CookieContainer M_Cookie,bool lbnLogin=false)
        {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentNullException("url");
                }
                if (requestEncoding == null)
                {
                    throw new ArgumentNullException("requestEncoding");
                }

                HttpWebRequest request = null;
                //如果是发送HTTPS请求  
                if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                    request = WebRequest.Create(url) as HttpWebRequest;
                    request.ProtocolVersion = HttpVersion.Version10;
                }
                else
                {
                    request = WebRequest.Create(url) as HttpWebRequest;
                }
            request.CookieContainer = M_Cookie;
              //  if (!(Cookie == null)) { request.CookieContainer.Add(Cookie); }
                
                request.Method = "POST";
            //服务端 判断 客户端 提交的是否是 JSON数据 时
             
                 if (!string.IsNullOrEmpty(postJson))
                         { request.ContentType = "application/json;charset=UTF-8"; }
                 else
                         { request.ContentType = "application/x-www-form-urlencoded"; }
                   
          
                request.KeepAlive = true;
                if (!string.IsNullOrEmpty(userAgent))
                {
                    request.UserAgent = userAgent;
                }
                else
                {
                    request.UserAgent = DefaultUserAgent;
                }

                if (timeout.HasValue)
                {
                    request.Timeout = timeout.Value;
                }

                //如果需要POST数据  
                #region post parameter  类似querystring格式
                if (parameters != null)
                {
                    byte[] data = requestEncoding.GetBytes(parameters);
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                        stream.Close();
                    }
                }
                #endregion

                #region post json
                if (!string.IsNullOrEmpty(postJson))
                {
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        //string json = "{\"user\":\"test\"," +
                        //              "\"password\":\"bla\"}";

                        streamWriter.Write(postJson);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                #endregion


                if (!string.IsNullOrEmpty(referer))
                {
                    request.Referer = referer;
                }

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            if (request.CookieContainer != null)
            {
                response.Cookies = request.CookieContainer.GetCookies(request.RequestUri);
                if (lbnLogin)
                {
                    string cookieheader = request.CookieContainer.GetCookieHeader(new Uri(url));
                    m_Cookie.SetCookies(new Uri(url), cookieheader);
                    m_Cookie = request.CookieContainer;
                }

                //  string cookieheader = request.CookieContainer.GetCookieHeader(request.RequestUri);

                // Cookie = response.Cookies;}

            }
                    return response;
            }

            #endregion

            #region Private Method
            private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true; //总是接受  
            }

            /// <summary>
            /// 将response转换成文本
            /// </summary>
            /// <param name="response"></param>
            /// <param name="encoding"></param>
            /// <returns></returns>
            public static string GetStream(HttpWebResponse response, Encoding encoding)
            {
                try
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        switch (response.ContentEncoding.ToLower())
                        {
                            case "gzip":
                                {
                                    string result = Decompress(response.GetResponseStream(), encoding);
                                    response.Close();
                                    return result;
                                }
                            default:
                                {
                                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), encoding))
                                    {
                                        string result = sr.ReadToEnd();
                                        sr.Close();
                                        sr.Dispose();
                                        response.Close();
                                        return result;
                                    }
                                }
                        }
                    }
                    else
                    {
                        response.Close();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return "";
            }

            private static string Decompress(Stream stream, Encoding encoding)
            {
                byte[] buffer = new byte[100];
                //int length = 0;

                using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
                {
                    //GZipStream gzip = new GZipStream(res.GetResponseStream(), CompressionMode.Decompress);
                    using (StreamReader reader = new StreamReader(gz, encoding))
                    {
                        return reader.ReadToEnd();
                    }
                    /*
                    using (MemoryStream msTemp = new MemoryStream())
                    {
                        //解压时直接使用Read方法读取内容，不能调用GZipStream实例的Length等属性，否则会出错：System.NotSupportedException: 不支持此操作；
                        while ((length = gz.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            msTemp.Write(buffer, 0, length);
                        }

                        return encoding.GetString(msTemp.ToArray());
                    }
                     * */
                }
            }
        public static string GetHttp(string url, HttpContext httpContext)
        {
            string queryString = "?";

            foreach (string key in httpContext.Request.QueryString.AllKeys)
            {
                queryString += key + "=" + httpContext.Request.QueryString[key] + "&";
            }

            queryString = queryString.Substring(0, queryString.Length - 1);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url + queryString);

            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.Timeout = 20000;

            //byte[] btBodys = Encoding.UTF8.GetBytes(body);
            //httpWebRequest.ContentLength = btBodys.Length;
            //httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();

            return responseContent;
        }


            #endregion
    }

    }
