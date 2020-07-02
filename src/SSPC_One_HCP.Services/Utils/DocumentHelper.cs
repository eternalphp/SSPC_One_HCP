using log4net;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Services.Utils
{
    public class DocumentHelper
    {
        private static readonly ILog _logger = LogManager.GetLogger("WarnFileLogger"); 
        /// <summary>
        /// 获取MP3文件播放时长
        /// </summary>
        /// <param name="SongPath">文件的绝对地址</param>
        /// <returns></returns>
        public static string GetMp3FileTime(string SongPath)
        {
            WMPLib.WindowsMediaPlayerClass wmp = new WMPLib.WindowsMediaPlayerClass();
            WMPLib.IWMPMedia media = wmp.newMedia(SongPath);
            var totalLong = media.durationString;
            var arr = totalLong.Split(':');
            var timeLong = "";
            if (arr.Count() == 3)
            {
                timeLong = arr[0] + "时" + arr[1] + "分" + arr[2] + "秒";
            }
            if (arr.Count() == 2)
            {
                timeLong = arr[0] + "分" + arr[1] + "秒";
            }
            if (arr.Count() == 1)
            {
                timeLong = arr[0] + "秒";
            }
            return timeLong;
            //return media.getItemInfo("Duration");
        }

        public static   string GetMediaFileTime(string SongPath)
        {

         
            MediaInfoDotNet.MediaFile mediaFile = new MediaInfoDotNet.MediaFile(SongPath);
            var totalLong = 0;
            if (mediaFile.Audio.Count > 0)
            {
                totalLong = mediaFile.Audio[0].Duration;
            }

            if (mediaFile.Video.Count > 0)
            {
                totalLong = mediaFile.Video[0].Duration;
            }
            
            StringBuilder strBuilder = new StringBuilder();
            long temp = totalLong;
            long hper = 60 * 60 * 1000;
            long mper = 60 * 1000;
            long sper = 1000;
            if (temp / hper > 0)
            {
                strBuilder.Append(temp / hper).Append("时");
            }
            temp = temp % hper;

            if (temp / mper > 0)
            {
                strBuilder.Append(temp / mper).Append("分");
            }
            temp = temp % mper;
            if (temp / sper > 0)
            {
                strBuilder.Append(temp / sper).Append("秒");
            }
            return strBuilder.ToString();

        }

        public static bool WriterLog(string message)
        {
            var logPath = ConfigurationManager.AppSettings["LogPath"];
            logPath += DateTime.Today.ToString("yyyyMMdd") + ".log";
            try
            {
                FileStream fs = new FileStream(logPath, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.Flush();
                // 使用StreamWriter来往文件中写入内容

                sw.BaseStream.Seek(0, SeekOrigin.End);
                string strLogStr = "Start: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n ";
                strLogStr += message + "\r\n";
                strLogStr += "End: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n ";

                sw.WriteLine(strLogStr);
                sw.Flush();
                //关闭文件
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception er)
            {
                return false;
            }

        }



    }
}
