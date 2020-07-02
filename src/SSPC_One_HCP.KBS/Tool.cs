using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SSPC_One_HCP.KBS
{
    public class Tool
    {
        public static string Sign(Dictionary<string, object> dic, string secretkey)
        {
            var dicSort = dic.OrderByDescending(r => r.Key).ToDictionary(r => r.Key, r => r.Value);
            StringBuilder sb = new StringBuilder();
            sb.Append(secretkey);
            foreach (var item in dicSort)
            {
                sb.Append(item.Key + item.Value);
            }
            sb.Append(secretkey);
            return GenerateMD5(sb.ToString().ToUpper());
        }
        /// <summary>
        /// MD5字符串加密
        /// </summary>
        /// <param name="txt"></param>
        /// <returns>加密后字符串</returns>
        private static string GenerateMD5(string txt)
        {
            using (MD5 mi = MD5.Create())
            {
                byte[] buffer = Encoding.Default.GetBytes(txt);
                //开始加密
                byte[] newBuffer = mi.ComputeHash(buffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < newBuffer.Length; i++)
                {
                    sb.Append(newBuffer[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 时间戳Timestamp转换成日期 到毫秒
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeMilliseconds(long timeStamp)
        {
            DateTime _dtStart = new DateTime(1970, 1, 1, 8, 0, 0);
            return _dtStart.AddMilliseconds(timeStamp);
        }
        /// <summary>
        /// 秒转换成时，分，秒
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static string SecondsToDateTime(int duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = ts.Hours.ToString() + "小时 " + ts.Minutes.ToString() + "分钟 " + ts.Seconds + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = ts.Minutes.ToString() + "分钟 " + ts.Seconds + "秒";
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = ts.Seconds + "秒";
            }
            return str;
        }
    }
}
