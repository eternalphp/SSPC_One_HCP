using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.GDI
{
    public static class GDIHelp
    {
        /// <summary>
        /// 将 Stream 转成 byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }
        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        // <summary> 
        /// 字节流转换成图片 
        /// </summary> 
        /// <param name="byt">要转换的字节流</param> 
        /// <returns>转换得到的Image对象</returns> 
        public static Bitmap BytToImg(byte[] byt)
        {
            try
            {
                MemoryStream ms = new MemoryStream(byt);
                Image result = Image.FromStream(ms);
                Bitmap bit = new Bitmap(result);
                return bit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        ///  图片转换成字节流 
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ImageToByteArray(Image img)
        {
            ImageConverter imgconv = new ImageConverter();
            byte[] b = (byte[])imgconv.ConvertTo(img, typeof(byte[]));
            return b;
        }
        /// <summary>
        /// 把图片Url转化成Image对象
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static Image Url2Img(string imageUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                {
                    return null;
                }

                WebRequest webreq = WebRequest.Create(imageUrl);
                WebResponse webres = webreq.GetResponse();
                Stream stream = webres.GetResponseStream();
                Image image;
                image = Image.FromStream(stream);
                stream.Close();

                return image;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary>
        /// 根据路径读取图变
        /// 通过FileStream 来打开文件，这样就可以实现不锁定Image文件，到时可以让多用户同时访问Image文件
        /// </summary>
        /// <param name="path">图片路径</param>
        /// <returns>返回Bitmap </returns>
        public static Bitmap ReadImageFile(string path)
        {
            FileStream fs = File.OpenRead(path); //OpenRead
            int filelength = (int)fs.Length;
            byte[] image = new byte[filelength]; //建立一个字节数组 
            fs.Read(image, 0, filelength); //按字节流读取 
            Image result = Image.FromStream(fs);
            fs.Close();
            Bitmap bit = new Bitmap(result);
            return bit;
        }
        /// <summary>
        /// 叠加合并图变
        /// </summary>
        /// <param name="bBitmap">背景图</param>
        /// <param name="bWidth">背景图-宽</param>
        /// <param name="bHeight">背景图-高</param>
        /// <param name="uBitmap">图</param>
        /// <param name="uWidth">图-宽</param>
        /// <param name="uHeight">图-高</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Bitmap DrawImage(Bitmap bBitmap, int bWidth, int bHeight, Bitmap uBitmap, int uWidth, int uHeight, float x, float y)
        {
            Bitmap bitmap = new Bitmap(bBitmap, bWidth, bHeight);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                //设置画布的描绘质量
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                g.DrawImage(uBitmap, x, y, new Rectangle(0, 0, uWidth, uHeight), GraphicsUnit.Pixel);
            }
            return bitmap;
        }

        /// <summary>
        /// 图片添加文字
        /// </summary>
        /// <param name="bBitmap">背景图</param>
        /// <param name="content">内容</param>
        /// <param name="x">x坐标</param>
        /// <param name="y">y坐标</param>
        /// <param name="width">内容矩形宽</param>
        /// <param name="height">内容矩形高</param>
        /// <param name="alignment">内容对齐方式 默认左对齐</param>
        /// <param name="lineAlignment">内容换行对齐方式 默认左对齐</param>
        /// <returns></returns>
        public static Bitmap DrawString(Bitmap bBitmap, string content, int x, int y, int width, int height,
            Font font,
            Brush fontBrush,
            StringAlignment alignment = StringAlignment.Near,
            StringAlignment lineAlignment = StringAlignment.Near)
        {
            using (Graphics g = Graphics.FromImage(bBitmap))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                //Font font = SystemFonts.DefaultFont;
                //Brush fontBrush = SystemBrushes.ControlText;
                StringFormat sf = new StringFormat
                {
                    Alignment = alignment,
                    LineAlignment = lineAlignment
                };
                g.DrawString(content, font, fontBrush, new Rectangle(x, y, width, height), sf);
            }
            return bBitmap;
        }

        /// <summary>
        /// 直角图变转圆图
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CutEllipse(Image img, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                using (TextureBrush br = new TextureBrush(img, System.Drawing.Drawing2D.WrapMode.Clamp, new Rectangle(0, 0, width, height)))
                {
                    br.ScaleTransform(bitmap.Width / (float)width, bitmap.Height / (float)height);
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillEllipse(br, new Rectangle(Point.Empty, new Size(width, height)));
                }
            }
            return bitmap;
        }

        /// <summary>
        /// 调整图片大小
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width">目标宽</param>
        /// <param name="height">目标高</param>
        /// <returns></returns>
        public static Bitmap Resize(Bitmap img, int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                // 插值算法的质量
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Rectangle(0, 0, width, height), new Rectangle(0, 0, img.Width, img.Height), GraphicsUnit.Pixel);

            }
            return bitmap;
        }

        /// <summary>
        /// 高质量保存图片
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="path">保存路径 不带后缀名</param>
        public static string Save(Bitmap bitmap, string path, string mineType = "image/jpeg")
        {
            using (MemoryStream mem = new MemoryStream())
            {

                Bitmap myBitmap = bitmap;

                EncoderParameters myEncoderParameters = new EncoderParameters(1);

                EncoderParameter myEncoderParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 80L);

                myEncoderParameters.Param[0] = myEncoderParameter;

                ImageCodecInfo myImageCodecInfo = GetEncoderInfo(mineType);

                string ext = myImageCodecInfo.FilenameExtension.Split(';')[0];
                ext = Path.GetExtension(ext).ToLower();
                string saveName = Path.ChangeExtension(path, ext);
                //保存
                myBitmap.Save(saveName, myImageCodecInfo, myEncoderParameters);

                return saveName;
            }
        }
        /// <summary>
        /// 获取MimeType
        /// </summary>
        /// <param name="mineType"></param>
        /// <returns></returns>
        static ImageCodecInfo GetEncoderInfo(string mineType)
        {

            ImageCodecInfo[] myEncoders = ImageCodecInfo.GetImageEncoders();

            foreach (ImageCodecInfo myEncoder in myEncoders)
                if (myEncoder.MimeType == mineType)
                    return myEncoder;
            return null;
        }
    }
}
