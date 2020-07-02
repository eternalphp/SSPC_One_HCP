using Newtonsoft.Json;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Core.Utils;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace SSPC_One_HCP.Services.Implementations
{
    public class QRCodeService : IQRCodeService
    {
        private readonly IEfRepository _rep;
        public QRCodeService(IEfRepository rep)
        {
            _rep = rep;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="agentModel"></param>
        private void GenerateQRCode(string rootPath, Tuple<Tuple<string, string>, Dictionary<Tuple<string, string>, List<string>>> agentModel)
        {
            if (!string.IsNullOrEmpty(rootPath) && agentModel != null)
            {
                Tuple<string, string> agentTuple = agentModel.Item1;
                string agentKey = agentTuple.Item1.ToLower();
                string agentName = agentTuple.Item2;
                string agentPath = rootPath + "/" + agentKey;
                Stream stream = null;
                Bitmap bmpt = null;
                if (!Directory.Exists(agentPath))
                {
                    //先生成代理目录
                    Directory.CreateDirectory(agentPath);
                    dynamic codeJson = new { AgentBatchQR = agentKey };
                    stream = QRCodeUtils.GetQrCode(JsonConvert.SerializeObject(codeJson));
                    bmpt = new Bitmap(stream);
                    //保存流到文件
                    bmpt.Save(string.Format("{0}/{1}.jpg", agentPath, agentName));
                    bmpt.Dispose();

                    string agentDir = agentPath + "/" + agentName;
                    if (!Directory.Exists(agentDir))
                    {
                        Directory.CreateDirectory(agentDir);
                    }

                    var shopList = agentModel.Item2;
                    string shopKey = null;
                    string shopName = null;
                    string shopPath = null;
                    string shopFilePath = null;
                    string proFilePath = null;
                    foreach (var item in shopList)
                    {
                        shopKey = item.Key.Item1.ToLower();
                        shopName = item.Key.Item2;
                        shopPath = agentDir + "/" + shopKey;
                        if (!Directory.Exists(shopPath))
                        {
                            //创建店铺目录
                            Directory.CreateDirectory(shopPath);
                            shopFilePath = $"{shopPath}/{shopName}.jpg";
                            codeJson = new { SellerBatchQR = shopKey };
                            stream = QRCodeUtils.GetQrCode(JsonConvert.SerializeObject(codeJson));
                            bmpt = new Bitmap(stream);
                            //保存文件
                            bmpt.Save(shopFilePath);
                            bmpt.Dispose();

                            if (!Directory.Exists($"{shopPath}/{shopName}"))
                            {
                                //创建店铺名称目录
                                Directory.CreateDirectory($"{shopPath}/{shopName}");
                                var proCodeList = shopList[item.Key];
                                //生成所有商品的二维码
                                foreach (var proItem in proCodeList)
                                {
                                    proFilePath = $"{shopPath}/{shopName}/{proItem}.jpg";
                                    codeJson = new { ProductQRCodeUrl = proItem };
                                    stream = QRCodeUtils.GetQrCode(JsonConvert.SerializeObject(codeJson));
                                    bmpt = new Bitmap(stream);
                                    bmpt.Save(proFilePath);
                                    bmpt.Dispose();
                                }
                            }
                        }

                    }
                }
                else
                {
                    dynamic codeJson = new { AgentBatchQR = agentKey };
                    string agentDir = agentPath + "/" + agentName;
                    if (!Directory.Exists(agentDir))
                    {
                        Directory.CreateDirectory(agentDir);
                    }
                    else
                    {
                        var shopList = agentModel.Item2;
                        string shopKey = null;
                        string shopName = null;
                        string shopPath = null;
                        string shopFilePath = null;
                        string proFilePath = null;
                        foreach (var item in shopList)
                        {
                            shopKey = item.Key.Item1.ToLower();
                            shopName = item.Key.Item2;
                            shopPath = agentDir + "/" + shopKey;
                            if (!Directory.Exists(shopPath))
                            {
                                //创建店铺目录
                                Directory.CreateDirectory(shopPath);
                                shopFilePath = $"{shopPath}/{shopName}.jpg";
                                codeJson = new { SellerBatchQR = shopKey };
                                stream = QRCodeUtils.GetQrCode(JsonConvert.SerializeObject(codeJson));
                                bmpt = new Bitmap(stream);
                                //保存文件
                                bmpt.Save(shopFilePath);
                                bmpt.Dispose();

                                if (!Directory.Exists($"{shopPath}/{shopName}"))
                                {
                                    //创建店铺名称目录
                                    Directory.CreateDirectory($"{shopPath}/{shopName}");
                                    var proCodeList = shopList[item.Key];
                                    //生成所有商品的二维码
                                    foreach (var proItem in proCodeList)
                                    {
                                        proFilePath = $"{shopPath}/{shopName}/{proItem}.jpg";
                                        codeJson = new { ProductQRCodeUrl = proItem };
                                        stream = QRCodeUtils.GetQrCode(JsonConvert.SerializeObject(codeJson));
                                        bmpt = new Bitmap(stream);
                                        bmpt.Save(proFilePath);
                                        bmpt.Dispose();
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string GenerateQRCode(string content)
        {
            var _path = AppDomain.CurrentDomain.BaseDirectory;
            string fileName = "QRCode\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";

            string HostUrl = ConfigurationManager.AppSettings["QRCodeAddress"];

            var fs = QRCodeUtils.DownQrCodeFile(content, _path + fileName, "");
            if (fs)
            {
                return fileName.Replace('\\', '/');
            }
            return "";
        }

        /// <summary>
        /// 新增或修改推广二维码信息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddOrUpdateAdQRCode(AdQRCode viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (viewModel == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid parameters.";
                return rvm;
            }

            //推广二维码中转H5页面地址
            string AdQRCodeHandlerPage = ConfigurationManager.AppSettings["AdQRCodeHandlerPage"];
            if (!string.IsNullOrEmpty(AdQRCodeHandlerPage))
            {
                if (VirtualPathUtility.IsAppRelative(AdQRCodeHandlerPage))
                {
                    Uri requestUri = HttpContext.Current.Request.Url;
                    string host = requestUri.AbsoluteUri.Replace(requestUri.AbsolutePath, "");
                    AdQRCodeHandlerPage = host + VirtualPathUtility.ToAbsolute(AdQRCodeHandlerPage);
                }
            }

            AdQRCode model = null;
            if (!string.IsNullOrEmpty(viewModel.Id))
            {
                model = _rep.FirstOrDefault<AdQRCode>(s => s.IsDeleted != 1 && s.Id == viewModel.Id);
            }
            bool isNew = model == null;
            if (isNew)
            {
                model = new AdQRCode();
                model.Id = Guid.NewGuid().ToString();
                string query = "?id=" + model.Id;

                model.QRCodePicUrl = GenerateQRCode(AdQRCodeHandlerPage + query);
                model.VisitAmount = 0;

                model.CreateTime = DateTime.Now;
                model.CreateUser = workUser.User.Id;
            }
            else
            {
                model.UpdateTime = DateTime.Now;
                model.UpdateUser = workUser.User.Id;
            }

            model.AppName = viewModel.AppName;
            model.AppUrl = viewModel.AppUrl;
            model.BuName = viewModel.BuName;

            if (isNew)
            {
                _rep.Insert(model);
            }
            else
            {
                _rep.Update(model);
            }
            _rep.SaveChanges();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = model;
            return rvm;
        }

        /// <summary>
        /// 删除推广二维码信息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeleteAdQRCode(AdQRCode viewModel, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(viewModel?.Id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }

            AdQRCode model = _rep.FirstOrDefault<AdQRCode>(s => s.IsDeleted != 1 && s.Id == viewModel.Id);
            if (model == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id.";
                return rvm;
            }

            model.IsDeleted = 1;
            _rep.Update(model);
            _rep.SaveChanges();

            rvm.Success = true;
            rvm.Msg = "success";
            return rvm;
        }

        /// <summary>
        /// 解析推广二维码, 获取推广的公众号或小程序的相关信息, 同时二维码访问次数+1
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public ReturnValueModel AnalyzeAdQRCode(string id)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            if (string.IsNullOrEmpty(id))
            {
                rvm.Success = false;
                rvm.Msg = "The parameter 'Id' is required.";
                return rvm;
            }

            AdQRCode model = _rep.FirstOrDefault<AdQRCode>(s => s.IsDeleted != 1 && s.Id == id);

            if (model == null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id.";
                return rvm;
            }

            model.VisitAmount += 1;
            _rep.Update(model);
            _rep.SaveChanges();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                model.AppName,
                model.AppUrl,
                model.BuName
            };
            return rvm;
        }

        /// <summary>
        /// 获取推广二维码的列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ReturnValueModel GetAdQRCodeList(RowNumModel<AdQRCode> rowNum)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var list = _rep.Where<AdQRCode>(s => s.IsDeleted != 1);

            if (rowNum?.SearchParams != null)
            {
                if (!string.IsNullOrEmpty(rowNum.SearchParams.AppName))
                {
                    list = list.Where(s => s.AppName.Contains(rowNum.SearchParams.AppName));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.AppUrl))
                {
                    list = list.Where(s => s.AppName.Contains(rowNum.SearchParams.AppUrl));
                }
                if (!string.IsNullOrEmpty(rowNum.SearchParams.BuName))
                {
                    list = list.Where(s => s.AppName.Contains(rowNum.SearchParams.BuName));
                }
            }

            var total = list.Count();
            var rows = list.OrderByDescending(s => s.CreateTime).ToPaginationList(rowNum?.PageIndex, rowNum?.PageSize).ToList();

            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                total,
                rows
            };
            return rvm;
        }
    }
}
