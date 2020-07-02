using SSPC_One_HCP.AutofacManager;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.KBS;
using SSPC_One_HCP.KBS.Webs.Clients;
using SSPC_One_HCP.WebApi.Models;
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SSPC_One_HCP.WebApi.Controllers
{
    public class DownloadController : Controller
    {

        // GET: Default
        public ActionResult Index(string userId, string dataInfoId)
        {
            return View(new DownloadInfo() { UserId = userId, DataInfoId = dataInfoId });
        }


        [HttpPost]
        public async Task<ActionResult> Download(DownloadInfo model)
        {
            try
            {
                byte[] b = System.Text.Encoding.UTF8.GetBytes(model.PassWord);
                //转成 Base64 形式的 System.String  

                var pwd = Convert.ToBase64String(b);
                string _host = ConfigurationManager.AppSettings["ADVerifyUrl"];
                var result = await new WebClient<AdResult>()
                            .Post(_host)
                            .JsonData(new { adaccount = model.UserName, password = pwd })
                            .ResultFromJsonAsync();
                //成功
                if (result?.Error_code == 0)
                {
                    var _rep = ContainerManager.Resolve<IEfRepository>();

                    var data = _rep.FirstOrDefault<HcpDataInfo>(o => o.Id == model.DataInfoId && o.IsDeleted == 0 && o.IsDownload == true);
                    if (data == null)
                        return Content(" <script language=javascript>alert('文件已删除或文件未开放下载。');history.go(-1); </script> ");

                    var saleUserInfo = _rep.FirstOrDefault<BotSaleUserInfo>(o => o.ADAccount == model.UserName && o.IsDeleted == 0);

                    _rep.Insert(new HcpDownloadInfo
                    {
                        Id = Guid.NewGuid().ToString(),
                        Sender = model.UserId,
                        DownloadPeople = model.UserName,
                        HcpDataInfoId = data.Id,
                        DownloadFileName = data.Title,
                        CreateTime = DateTime.Now,
                        CreateUser = saleUserInfo?.Id,
                    });
                    _rep.SaveChanges();
                    string fileName = $"{data.Title}{".pdf"}";//客户端保存的文件名
                    //var filePath = Server.MapPath("~/" + data.DataUrl);
                    var filePath = HostingEnvironment.MapPath("/" + data.DataUrl);
                    return File(new FileStream(filePath, FileMode.Open), "application/octet-stream", fileName);
                }
                if (result?.Error_code == -1)
                    return Content(" <script language=javascript>alert('您输入的账号无权限访问或账号密码不正确。'); history.go(-1);</script> ");

                if (result?.Error_code == 10001001)
                    return Content(" <script language=javascript>alert('您输入的账号无权限访问或账号密码不正确。'); history.go(-1);</script> ");

                if (result?.Error_code == 10031001)
                    return Content(" <script language=javascript>alert('您输入的账号无权限访问或账号密码不正确。'); history.go(-1);</script> ");

                if (result?.Error_code == 10031004)
                    return Content(" <script language=javascript>alert('您输入的账号无权限访问或账号密码不正确。'); history.go(-1);</script> ");

                if (result?.Error_code == 10031011)
                    return Content(" <script language=javascript>alert('您输入的账号无权限访问或账号密码不正确。'); history.go(-1);</script> ");

                if (result?.Error_code == 10031012)
                    return Content(" <script language=javascript>alert('您输入的账号无权限访问或账号密码不正确。'); history.go(-1);</script> ");

                else
                    return Content(" <script language=javascript>alert('您输入的账号无权限访问或账号密码不正确。'); history.go(-1);</script> ");

            }
            catch (Exception e)
            {
                return Content(" <script language=javascript>alert('系统忙，请稍后再试！" + e.Message + "'); history.go(-1);</script> ");
            }

        }

    }
}