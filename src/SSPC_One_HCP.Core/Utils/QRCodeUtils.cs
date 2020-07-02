using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// 二维码帮助
    /// </summary>
    public static class QRCodeUtils
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="msg">二维码内容</param>
        /// <returns></returns>
        public static MemoryStream GetQrCode(string msg)
        {
            MultiFormatWriter writer = new MultiFormatWriter();
            Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>
            {
                //设置二维码为utf-8编码
                { EncodeHintType.CHARACTER_SET, "utf-8" },
                //设置纠错等级， 高
                { EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H }
            };
            BitMatrix bm = writer.encode(msg, BarcodeFormat.QR_CODE, 300, 300, hint);
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            var bitmap = barcodeWriter.Write(bm);
            //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
            int[] rectangle = bm.getEnclosingRectangle();
            //计算插入图片的大小和位置
            int middleImgW = Math.Min((int)(rectangle[2] / 3.5), 300);
            int middleImgH = Math.Min((int)(rectangle[3] / 3.5), 300);
            int middleImgL = (bitmap.Width - middleImgW) / 2;
            int middleImgT = (bitmap.Height - middleImgH)+45;
            Bitmap bmpimg = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(bmpimg))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(bitmap, 0, 0);
            }
            //在二维码中插入图片
            Graphics myGraphic = Graphics.FromImage(bmpimg);
            //白底
            myGraphic.FillRectangle(Brushes.White, middleImgL, middleImgT, middleImgW, middleImgH);
            myGraphic.DrawString(msg, new Font("宋体", 12, FontStyle.Bold), new SolidBrush(Color.Black), 30, middleImgT);
            MemoryStream ms = new MemoryStream(BitmapToByte(bmpimg));
            return ms;
        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static byte[] BitmapToByte(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Jpeg);
                byte[] data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return data;
            }
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="fileName">文件路径</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool DownQrCodeFile(string content, string fileName,string msg)
        {
            try
            {
                MultiFormatWriter writer = new MultiFormatWriter();
                Dictionary<EncodeHintType, object> hint = new Dictionary<EncodeHintType, object>
                {
                    //设置二维码为utf-8编码
                    { EncodeHintType.CHARACTER_SET, "utf-8" },
                    //设置纠错等级， 高
                    { EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H }
                };
                BitMatrix bm = writer.encode(content, BarcodeFormat.QR_CODE, 300, 300, hint);
                BarcodeWriter barcodeWriter = new BarcodeWriter();
                var bitmap = barcodeWriter.Write(bm);
                //获取二维码实际尺寸（去掉二维码两边空白后的实际尺寸）
                int[] rectangle = bm.getEnclosingRectangle();
                //计算插入图片的大小和位置
                int middleImgW = Math.Min((int)(rectangle[2] / 3.5), 300);
                int middleImgH = Math.Min((int)(rectangle[3] / 3.5), 300);
                int middleImgL = (bitmap.Width - middleImgW) / 2;
                int middleImgT = (bitmap.Height - middleImgH)+45;
                Bitmap bmpimg = new Bitmap(bitmap.Width, bitmap.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(bmpimg))
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.DrawImage(bitmap, 0, 0);
                }
                //在二维码中插入图片
                Graphics myGraphic = Graphics.FromImage(bmpimg);
                //白底
                myGraphic.FillRectangle(Brushes.White, middleImgL, middleImgT, middleImgW, middleImgH);
                myGraphic.DrawString(msg, new Font("宋体", 10, FontStyle.Bold), new SolidBrush(Color.Black), 20, middleImgT);
                //MemoryStream ms = new MemoryStream(BitmapToByte(bmpimg));
                bmpimg.Save(fileName);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }


        }
    }
}
