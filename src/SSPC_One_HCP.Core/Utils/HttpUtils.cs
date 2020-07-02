using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// 模拟请求
    /// </summary>
    public static class HttpUtils
    {
        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="postDataStr">请求数据</param>
        /// <param name="contentType">数据类型</param>
        /// <returns></returns>
        public static string HttpPost(string Url, string postDataStr,string contentType="application/json")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            request.ContentType = contentType;
            request.ContentLength = Encoding.UTF8.GetByteCount(postDataStr);
            Stream myRequestStream = request.GetRequestStream();
            StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding("gb2312"));
            myStreamWriter.Write(postDataStr);
            myStreamWriter.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="postDataStr">请求数据</param>
        /// <returns></returns>
        public static string HttpGet(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url + (postDataStr == "" ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }

        // 泛型：Post请求
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string postData, string contentType = "application/json") where T : class, new()
        {
            T result = default(T);

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
            httpContent.Headers.ContentType.CharSet = "utf-8";
            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    Task<string> t = response.Content.ReadAsStringAsync();
                    string s = t.Result;
                    //Newtonsoft.Json
                    string json = JsonConvert.DeserializeObject(s).ToString();
                    result = JsonConvert.DeserializeObject<T>(json);

                    if (result == null)
                    {
                        ILog _logger = LogManager.GetLogger("DebugFileLogger");
                        _logger.Warn($"PostResponse  failed begin ---------------------------------------------");
                        _logger.Warn($"PostResponse url: {url}");
                        _logger.Warn($"PostResponse postData: {postData}");
                        _logger.Warn($"PostResponse contentType: {contentType}");
                        _logger.Warn($"PostResponse StatusCode: {response.StatusCode}");
                        _logger.Warn($"PostResponse Content: {s}");
                        _logger.Warn($"PostResponse  failed end   ---------------------------------------------");
                    }
                }
                else
                {
                    ILog _logger = LogManager.GetLogger("DebugFileLogger");
                    _logger.Warn($"PostResponse  failed begin ---------------------------------------------");
                    _logger.Warn($"PostResponse url: {url}");
                    _logger.Warn($"PostResponse postData: {postData}");
                    _logger.Warn($"PostResponse contentType: {contentType}");
                    _logger.Warn($"PostResponse StatusCode: {response.StatusCode}");
                    _logger.Warn($"PostResponse  failed end   ---------------------------------------------");
                }

                
            }
            return result;
        }
    }
}
