using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Drawing;
using Spire.Pdf;
using Spire.Pdf.Exporting;
using Spire.Pdf.Graphics;
using SSPC_One_HCP.Core.GDI;

namespace SSPC_One_HCP.WebApi.Controllers
{
    /// <summary>
    /// 上传文件
    /// </summary>
    public class UploadFileController : ApiController
    {
        /// <summary>
        /// 上传照片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadImg()
        {
            var root = HostingEnvironment.MapPath("~/");
            var req = HttpContext.Current.Request;
            var files = req.Files;
            List<string> list = new List<string>();
            var path = root + @"Upload";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            for (int i = 0; i < files.Count; i++)
            {
                var ext = files[i].FileName.Substring(files[i].FileName.LastIndexOf("."));
                var fileName = Guid.NewGuid().ToString() + ext;
                files[i].SaveAs(path + @"/" + fileName);
                list.Add($"Upload/{fileName}");
            }
            return Ok(list);
        }

        /// <summary>
        /// 上传会议文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadMeetData()
        {
            var root = HostingEnvironment.MapPath("~/");
            var req = HttpContext.Current.Request;
            var files = req.Files;
            //List<string> list = new List<string>();
            List<FileInfoModel> list = new List<FileInfoModel>();
            var path = root + @"Upload/Meet";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileInfoModel fileInfo;
            for (int i = 0; i < files.Count; i++)
            {
                fileInfo = new FileInfoModel();
                var ext = files[i].FileName.Substring(files[i].FileName.LastIndexOf("."));
                var fileName = Guid.NewGuid().ToString() + ext;
                fileInfo.name = fileName;
                files[i].SaveAs(path + @"/" + fileName);
                fileInfo.url = $"Upload/Meet/{fileName}";
                list.Add(fileInfo);
            }
            ReturnValueModel rvm = new ReturnValueModel();
            if (list.Count > 0)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    list = list
                };
            }
            return Ok(rvm);
        }

        /// <summary>
        /// 上传产品资料图标
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadProductIcon()
        {
            //var root = HostingEnvironment.MapPath("~/");
            //var req = HttpContext.Current.Request;
            //var files = req.Files;
            ////List<string> list = new List<string>();
            //List<FileInfoModel> list = new List<FileInfoModel>();
            //var path = root + @"Upload/Product/Icon";
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}

            //FileInfoModel fileInfo;
            //for (int i = 0; i < files.Count; i++)
            //{
            //    fileInfo = new FileInfoModel();
            //    var ext = files[i].FileName.Substring(files[i].FileName.LastIndexOf("."));
            //    var fileName = Guid.NewGuid().ToString() + ext;
            //    fileInfo.name = fileName;
            //    files[i].SaveAs(path + @"/" + fileName);
            //    fileInfo.url = $"Upload/Product/Icon/{fileName}";
            //    list.Add(fileInfo);
            //}
            //ReturnValueModel rvm = new ReturnValueModel();
            //if (list.Count > 0)
            //{
            //    rvm.Msg = "success";
            //    rvm.Success = true;
            //    rvm.Result = new
            //    {
            //        list = list
            //    };
            //}
            //return Ok(rvm);

            var root = HostingEnvironment.MapPath("~/");
            var req = HttpContext.Current.Request;
            var files = req.Files;
            //List<string> list = new List<string>();
            List<FileInfoModel> list = new List<FileInfoModel>();
            var filePath = @"Upload/Product/Icon";
            var path = root + filePath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileInfoModel fileInfo;
            for (int i = 0; i < files.Count; i++)
            {

                string mineType = MimeMapping.GetMimeMapping(files[i].FileName);
                var inputStream = files[i].InputStream;
                var stream = GDIHelp.StreamToBytes(inputStream);
                var bitmap = GDIHelp.BytToImg(stream);

                int width = 750;
                int fitWidth;
                int fitHeight;

                if (bitmap.Width > width)
                {
                    fitWidth = width;
                    fitHeight = (bitmap.Height * width) / bitmap.Width;//(bitmap.Height * height) / bitmap.Width;
                }
                else
                {
                    fitWidth = bitmap.Width;
                    fitHeight = bitmap.Height;
                }
                bitmap = GDIHelp.Resize(bitmap, fitWidth, fitHeight);
                var newName = Guid.NewGuid().ToString();
                path = path + "/" + newName;
                var saveName = GDIHelp.Save(bitmap, path, mineType);

                var ext = saveName.Substring(saveName.LastIndexOf("."));

                fileInfo = new FileInfoModel();
                var fileName = $"{newName}{ext}";
                fileInfo.name = fileName;
                fileInfo.url = $"{filePath}/{newName}{ext}";
                list.Add(fileInfo);
            }
            ReturnValueModel rvm = new ReturnValueModel();
            if (list.Count > 0)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    list = list
                };
            }
            return Ok(rvm);
        }

        /// <summary>
        /// 上传播客文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadPodcastFile()
        {
            var root = HostingEnvironment.MapPath("~/");
            var req = HttpContext.Current.Request;
            var files = req.Files;
            //List<string> list = new List<string>();
            List<MediaInfoModel> list = new List<MediaInfoModel>();
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var path = root + @"Upload/Podcast";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                MediaInfoModel fileInfo;
                for (int i = 0; i < files.Count; i++)
                {
                    fileInfo = new MediaInfoModel();
                    var ext = files[i].FileName.Substring(files[i].FileName.LastIndexOf("."));
                    var fileName = Guid.NewGuid().ToString() + ext;
                    fileInfo.name = fileName;
                    files[i].SaveAs(path + @"/" + fileName);
                    //获取文件时长
                    //fileInfo.filetime = DocumentHelper.GetMp3FileTime(path + @"/" + fileName);

                    fileInfo.filetime = DocumentHelper.GetMediaFileTime(path + @"/" + fileName);

                    fileInfo.url = $"Upload/Podcast/{fileName}";
                    list.Add(fileInfo);
                }
                if (list.Count > 0)
                {
                    rvm.Msg = "success";
                    rvm.Success = true;
                    rvm.Result = new
                    {
                        list = list
                    };
                }

            }
            catch (Exception e)
            {
                rvm.Msg = e.Message;
                rvm.Success = false;
            }


            return Ok(rvm);
        }

        /// <summary>
        /// 上传产品资料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadProductFile()
        {
            var root = HostingEnvironment.MapPath("~/");
            var req = HttpContext.Current.Request;
            var files = req.Files;
            //List<string> list = new List<string>();
            List<FileInfoModel> list = new List<FileInfoModel>();
            var path = root + @"Upload/ProductData";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileInfoModel fileInfo;
            for (int i = 0; i < files.Count; i++)
            {
                fileInfo = new FileInfoModel();
                var ext = files[i].FileName.Substring(files[i].FileName.LastIndexOf("."));
                var fileName = Guid.NewGuid().ToString() + ext;
                fileInfo.name = fileName;
                files[i].SaveAs(path + @"/" + fileName);
                /*
                if (ext.Equals(".PDF", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        //内容压缩
                        PdfDocument doc = new PdfDocument(path + @"/" + fileName);
                        //禁用incremental update
                        doc.FileInfo.IncrementalUpdate = false;
                        //设置PDF文档的压缩级别
                        doc.CompressionLevel = PdfCompressionLevel.Best;

                        //遍历文档所有页面
                        foreach (PdfPageBase page in doc.Pages)
                        {
                            //提取页面中的图片
                            Image[] images = page.ExtractImages();
                            if (images != null && images.Length > 0)
                            {
                                //遍历所有图片
                                for (int j = 0; j < images.Length; j++)
                                {
                                    Image image = images[j];
                                    PdfBitmap bp = new PdfBitmap(image);
                                    //降低图片的质量
                                    bp.Quality = 20;
                                    //用压缩后的图片替换原文档中的图片
                                    page.ReplaceImage(j, bp);
                                }
                            }
                        }
                        fileName = fileName + "_Z" + ext;
                        //保存文档
                        doc.SaveToFile(path + @"/" + fileName);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                */
                fileInfo.url = $"Upload/ProductData/{fileName}";
                list.Add(fileInfo);
            }

            ReturnValueModel rvm = new ReturnValueModel();
            if (list.Count > 0)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    list = list
                };
            }
            return Ok(rvm);


        }
        /// <summary>
        /// 会议封面图片
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadMeetCoverPic()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var root = HostingEnvironment.MapPath("~/");
            var req = HttpContext.Current.Request;
            var files = req.Files;
            var path = root + @"Upload/MeetPic";
            var ext = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."));
            var fileName = Guid.NewGuid().ToString() + ext;
            files[0].SaveAs(path + @"/" + fileName);

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = new
            {
                url = path + @"/" + fileName
            };
            return Ok(rvm);
        }
        /// <summary>
        /// 播放图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadMeetLivePicture()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            try
            {
                var root = HostingEnvironment.MapPath("~/");
                var req = HttpContext.Current.Request;
                var files = req.Files;
                var inputStream = files[0].InputStream;

                var stream = GDIHelp.StreamToBytes(inputStream);

                var bitmap = GDIHelp.BytToImg(stream);
                bitmap = GDIHelp.Resize(bitmap, 375, 210);
                //按钮图片
                var buttonBitmap = GDIHelp.ReadImageFile(root + @"\Content\images\LivePicture.png");

                var width = bitmap.Width / 2 - buttonBitmap.Width / 2;
                var height = bitmap.Height / 2 - buttonBitmap.Height / 2 - 3;
                bitmap = GDIHelp.DrawImage(bitmap, bitmap.Width, bitmap.Height, buttonBitmap, buttonBitmap.Width, buttonBitmap.Height, width, height);

                var filePath = @"Upload/MeetLivePicture";
                var newName = Guid.NewGuid().ToString();
                var path = root + filePath;

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "/" + newName;
                var saveName = GDIHelp.Save(bitmap, path);


                //var ext = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."));
                //var fileName = Guid.NewGuid().ToString() + ext;
                //files[0].SaveAs(path + @"/" + fileName);

                var ext = saveName.Substring(saveName.LastIndexOf("."));
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    url = $"{filePath}/{newName}{ext}",
                };
            }
            catch (Exception e)
            {
                rvm.Msg = "";
                rvm.Success = false;
                rvm.Result = e.Message;
            }
            return Ok(rvm);
        }
        /// <summary>
        /// 横幅上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UploadBanner()
        {

            var root = HostingEnvironment.MapPath("~/");
            var req = HttpContext.Current.Request;
            var files = req.Files;
            //List<string> list = new List<string>();
            List<FileInfoModel> list = new List<FileInfoModel>();
            var filePath = @"Upload/WeChatBanner";
            var path = root + filePath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            FileInfoModel fileInfo;
            for (int i = 0; i < files.Count; i++)
            {

                string mineType = MimeMapping.GetMimeMapping(files[i].FileName);
                var inputStream = files[i].InputStream;
                var stream = GDIHelp.StreamToBytes(inputStream);
                var bitmap = GDIHelp.BytToImg(stream);

                int width = 750;
                int fitWidth;
                int fitHeight;

                if (bitmap.Width > width)
                {
                    fitWidth = width;
                    fitHeight = (bitmap.Height * width) / bitmap.Width;//(bitmap.Height * height) / bitmap.Width;
                }
                else
                {
                    fitWidth = bitmap.Width;
                    fitHeight = bitmap.Height;
                }

                //if (bitmap.Width / bitmap.Height >= width / height)
                //{
                //    if (bitmap.Width > width)
                //    {
                //        fitWidth = width;
                //        fitHeight = (bitmap.Height * width) / bitmap.Width;//(bitmap.Height * height) / bitmap.Width;
                //    }
                //    else
                //    {
                //        fitWidth = bitmap.Width;
                //        fitHeight = bitmap.Height;
                //    }
                //}
                //else
                //{
                //    if (bitmap.Height > height)
                //    {
                //        fitHeight = height;
                //        fitWidth = (bitmap.Width * height) / bitmap.Height;
                //    }
                //    else
                //    {
                //        fitWidth = bitmap.Width;
                //        fitHeight = bitmap.Height;
                //    }
                //}

                bitmap = GDIHelp.Resize(bitmap, fitWidth, fitHeight);
                var newName = Guid.NewGuid().ToString();
                path = path + "/" + newName;
                var saveName = GDIHelp.Save(bitmap, path, mineType);

                var ext = saveName.Substring(saveName.LastIndexOf("."));

                fileInfo = new FileInfoModel();
                var fileName = $"{newName}{ext}";
                fileInfo.name = fileName;
                fileInfo.url = $"{filePath}/{newName}{ext}";
                list.Add(fileInfo);
            }
            ReturnValueModel rvm = new ReturnValueModel();
            if (list.Count > 0)
            {
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = new
                {
                    list = list
                };
            }
            return Ok(rvm);

        }


        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage DownloadFile(string url)
        {
            var browser = String.Empty;
            if (HttpContext.Current.Request.UserAgent != null)
            {
                browser = HttpContext.Current.Request.UserAgent.ToUpper();
            }
            //string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, url);
            string filePath = AppDomain.CurrentDomain.BaseDirectory + url;
            HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            httpResponseMessage.Content = new StreamContent(fileStream);
            httpResponseMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            httpResponseMessage.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName =
                    browser.Contains("FIREFOX")
                        ? Path.GetFileName(filePath)
                        : HttpUtility.UrlEncode(Path.GetFileName(filePath))
                //FileName = HttpUtility.UrlEncode(Path.GetFileName(filePath))
            };

            return httpResponseMessage;
        }
    }
}
