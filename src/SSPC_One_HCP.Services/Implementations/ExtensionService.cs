
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace SSPC_One_HCP.Services.Implementations
{
   public class ExtensionService:IExtensionService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;
        private readonly ISystemService _systemService;

        public ExtensionService(IEfRepository rep, ICommonService commonService, ISystemService systemService)
        {
            _rep = rep;
            _commonService = commonService;
            _systemService = systemService;
        }
        /// <summary>
        /// 获取所有公众号推广信息
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetPublicAccount(RowNumModel<PublicAccount> publicaccount, WorkUser workUser)
        {
            ReturnValueModel rvm = new ReturnValueModel();

            //获取所有有效的公众号推广信息
            var PublicAccount = from a in _rep.SqlQuery<PublicAccount>("select * from PublicAccount where IsDeleted=0")
                                join b in _rep.SqlQuery<UserInfo>("select * from UserInfo where IsDeleted=2 ") on a.CreateUser equals b.Id
                                select new PublicAccount
                                {
                                    Id = a.Id,
                                    AppId = a.AppId,
                                    PublicAccountName = a.PublicAccountName,
                                    AppUrl = a.AppUrl,
                                    Iseffective = a.Iseffective,
                                    Dept = a.Dept,
                                    Remark = a.Remark,
                                    CreateUser = b.ChineseName,
                                    CreateTime = a.CreateTime,
                                    ImageUrl = a.ImageUrl,
                                    ImageName = a.ImageName
                                };

            if (!string.IsNullOrEmpty(publicaccount.SearchParams.PublicAccountName))
            {
                PublicAccount = PublicAccount.Where(x => x.PublicAccountName.Contains(publicaccount.SearchParams.PublicAccountName));
            }

            if (PublicAccount == null)
            {
                rvm.Success = true;
                rvm.Msg = "No public number promotional information yet";

            }
            else
            {
                var total = PublicAccount.Count();
                //先按照Sort字段升序排列，再按照创建时间倒序排列
                var rows = PublicAccount.OrderByDescending(s => s.CreateTime).ToPaginationList(publicaccount.PageIndex, publicaccount.PageSize);
                rvm.Success = true;
                rvm.Msg = "Success";
                rvm.Result = new
                {
                    total = total,
                    rows = rows
                };
            }
            return rvm;
        }
        /// <summary>
        /// 新增或更新公众号推广信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddorUpdatePublicAccount(PublicAccount publicAccount,WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            //首先查询是否数据库是否存在公众号推广的有效信息
            var IsTrueUpdate = _rep.FirstOrDefault<PublicAccount>(s=>s.Id==publicAccount.Id && s.IsDeleted!=1/*"select * from PublicAccount where Id='"+publicaccount.Id+"' and IsDeleted=0"*/);
            PublicAccount acountNumber = new PublicAccount();
            //判断该动作是新增还是删除
            if (IsTrueUpdate != null)
            {
                try
                {
                    IsTrueUpdate.Id = publicAccount.Id;
                    IsTrueUpdate.AppId = publicAccount.AppId;
                    IsTrueUpdate.AppUrl = publicAccount.AppUrl;
                    IsTrueUpdate.PublicAccountName = publicAccount.PublicAccountName;
                    IsTrueUpdate.Iseffective = publicAccount.Iseffective;
                    if (publicAccount.DeptList.Count()>0)
                    {
                        List<string> arr = new List<string>();
                        foreach (var item in publicAccount.DeptList)
                        {
                            var deptName = _rep.Where<DepartmentInfo>(s => s.Id == item.ToString() && s.IsDeleted != 1).FirstOrDefault();
                            arr.Add(deptName.DepartmentName);
                        }
                        IsTrueUpdate.Dept = string.Join(",", arr);
                    }
                    else {
                        IsTrueUpdate.Dept = null;
                    }

                    IsTrueUpdate.Remark = publicAccount.Remark;
                    IsTrueUpdate.UpdateUser = workUser.User.Id;
                    IsTrueUpdate.DeptList = publicAccount.DeptList;
                    IsTrueUpdate.UpdateTime = DateTime.Now;
                    IsTrueUpdate.ImageUrl = publicAccount.ImageUrl;
                    IsTrueUpdate.ImageName = publicAccount.ImageName;
                    _rep.Update(IsTrueUpdate);
                    var send = _rep.SaveChanges();
                }
                catch (Exception ex)
                {
                    rvm.Success = false;
                    rvm.Msg = "'" + ex + "'";
                    throw ex;
                }
                rvm.Msg = "success";
                rvm.Success = true;
                rvm.Result = IsTrueUpdate ?? IsTrueUpdate;
            }
                //}
            //新增
            else {
                var IsCopy = _rep.SqlQuery<PublicAccount>("select * from PublicAccount where AppId='" + publicAccount.AppId + "' and Iseffective=0  and IsDeleted=0").FirstOrDefault();
                //判断新增的数据在数据库是否存在重复
                if (IsCopy != null)
                {
                    rvm.Success = true;
                    rvm.Msg = "The public number promotional information already exists";
                }
                else {
                   
                        try
                        {
                            acountNumber.Id = Guid.NewGuid().ToString();
                            acountNumber.AppId = publicAccount.AppId;
                            acountNumber.AppUrl = publicAccount.AppUrl;
                            acountNumber.PublicAccountName = publicAccount.PublicAccountName;
                            acountNumber.Iseffective = publicAccount.Iseffective;
                           ////获取主键id进行插入数据
                           // int Sortstart;
                           // var Sortsend= _rep.SqlQuery<PublicAccount>("select * from  PublicAccount").OrderByDescending(s => Convert.ToInt32(s.IsSort)).FirstOrDefault();
                           // if (Sortsend == null) {
                           //     Sortstart = 1;
                           // }
                           // else {
                           //     Sortstart = (Convert.ToInt32(Sortsend.IsSort) + 1);
                           // }
                           
                           // acountNumber.IsSort = Sortstart.ToString();
                        //通过用户传递的科室Id遍历科室名称
                        if (publicAccount.DeptList.Count()>0)
                        {
                            List<string> arr = new List<string>();
                            foreach (var item in publicAccount.DeptList)
                            {
                                var deptName = _rep.Where<DepartmentInfo>(s => s.Id == item.ToString() && s.IsDeleted != 1).FirstOrDefault();
                                arr.Add(deptName.DepartmentName);
                            }
                            acountNumber.Dept = string.Join(",", arr);
                        }
                        else {
                            acountNumber.Dept = null;
                        }
                            acountNumber.Remark = publicAccount.Remark;
                            acountNumber.CreateUser = workUser.User.Id;
                            acountNumber.CreateTime = DateTime.Now;
                            acountNumber.ImageUrl = publicAccount.ImageUrl;
                            acountNumber.ImageName = publicAccount.ImageName;
                            acountNumber.IsDeleted = 0;
                            _rep.Insert(acountNumber);
                            _rep.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            rvm.Success = false;
                            rvm.Msg = "'" + ex + "'";
                            throw ex;
                        }
                        rvm.Msg = "success";
                        rvm.Success = true;
                        rvm.Result = IsTrueUpdate ?? acountNumber;
                       
                }
             
            }
            
            return rvm;
        }
        /// <summary>
        /// 删除公众号推广信息
        /// </summary>
        /// <param name="publicaccount"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel DeletePublicAccount(PublicAccount publicAccount,WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            //查询要删除的对象
            var deletePublic = _rep.FirstOrDefault<PublicAccount>(s=>s.Id==publicAccount.Id && s.IsDeleted!=1/*"select * from PublicAccount where Id='"+publicaccount.Id+"' and IsDeleted=0"*/);
            //判断需要删除的数据是否存在
            if (deletePublic != null)
            {
                    try
                    {
                    deletePublic.IsDeleted = 1;
                    deletePublic.UpdateTime = DateTime.Now;
                    deletePublic.UpdateUser = workUser.User.Id;
                    _rep.Delete(deletePublic);
                      var send=_rep.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                       
                        rvm.Success = false;
                        rvm.Msg = "'"+ex+"'";
                        throw;
                    }
                    rvm.Success = true;
                    rvm.Msg = "success";
                    rvm.Result = deletePublic ?? deletePublic;
                   
            }
            else {
                rvm.Success = true;
                rvm.Msg = "No such data was found.";
            }
            return rvm;
        }
        /// <summary>
        /// 新增或更新小程序或其他公众号二维码推广信息
        /// </summary>
        /// <param name="qrCodeExtension"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel AddorUpdateQRCodeExtension(QRcodeExtension qrCodeExtension, WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            //获取参数查询数据库是否已存在有效的重复数据
            var qrRepeated = _rep.FirstOrDefault<QRcodeExtension>(s=>s.Id==qrCodeExtension.Id && s.IsDeleted!=1);
            //如果存在则该操作为编辑操作if
            if (qrRepeated!=null)
            {
                //判断数据库是否存在内容重复的有效数据
                var contentRepetition = _rep.FirstOrDefault<QRcodeExtension>(s=>s.AppId== qrCodeExtension.AppId && s.AppName== qrCodeExtension.AppName && s.AppType== qrCodeExtension.AppType&& s.IsDeleted!=1);
                //判断重复有效数据是否存在，存在则执行修改操作，不存在则跳出程序
                if (contentRepetition != null)
                {
                    rvm.Success = false;
                    rvm.Msg = "There are duplicate data in this information.";
                }
                else {
                    qrRepeated.Id = qrCodeExtension.Id;
                    qrRepeated.AppId = qrCodeExtension.AppId;
                    qrRepeated.AppName = qrCodeExtension.AppName;
                    qrRepeated.AppType = qrCodeExtension.AppType;
                    qrRepeated.UpdateTime = DateTime.Now;
                    qrRepeated.UpdateUser = workUser.User.Id;
                    //给记录进行更新操作
                    _rep.Update(qrRepeated);
                    //保存执行的操作
                  var send=  _rep.SaveChanges();
                    //判断是否操作成功
                    if (send> 0)
                    {
                        rvm.Success = true;
                        rvm.Msg = "success";
                        rvm.Result = contentRepetition ?? qrRepeated;
                    }
                    else {
                        rvm.Success = false;
                        rvm.Msg = "Failure to modify";
                       
                    }
                }
            }
            //如果不存在，则该操作为新增操作
            else {
                //判断重复有效数据是否存在，存在则执行修改操作，不存在则跳出程序
                var contentRepetition = _rep.FirstOrDefault<QRcodeExtension>(s => s.AppId == qrCodeExtension.AppId && s.AppName == qrCodeExtension.AppName && s.AppType == qrCodeExtension.AppType && s.IsDeleted != 1);
                if (contentRepetition != null)
                {
                    rvm.Success = false;
                    rvm.Msg = "There are duplicate data in this information.";
                }
                else {
                    
                    //string strCode = "http://buo.fresenius-kabi.com.cn/admin?APPID=0&Type=1&ActivityID=12";
                    //QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
                    //QRCodeData qrCodeData = qrGenerator.CreateQrCode(strCode, QRCodeGenerator.ECCLevel.Q);
                    //QRCode qrcode = new QRCode(qrCodeData);
                    //Bitmap qrCodeImage = qrcode.GetGraphic(5, Color.Black, Color.White, null, 15, 6, false);
                    //if (!Directory.Exists("F:\\QRCode"))
                    //    Directory.CreateDirectory("F:\\QRCode");
                    //string sq = Guid.NewGuid().ToString(); 
                    //qrCodeImage.Save("F:\\QRCode\\"+sq+".jpg");
                    QRcodeExtension qRcode = new QRcodeExtension();
                    qRcode.Id = Guid.NewGuid().ToString();
                    qRcode.AppId = qrCodeExtension.AppId;
                    qRcode.AppName = qrCodeExtension.AppName;
                    qRcode.AppType = qrCodeExtension.AppType;
                    qRcode.CreateTime = DateTime.Now;
                    qRcode.CreateUser = workUser.User.Id;
                    qRcode.IsDeleted = 0;
                    //添加一条新数据
                    _rep.Insert(qRcode);
                    //保存新增数据
                  var send=  _rep.SaveChanges();
                    //判断操作是否成功
                    if (send> 0)
                    {
                        rvm.Success = true;
                        rvm.Msg = "success";
                        rvm.Result = contentRepetition ?? qRcode;

                    }
                    else {
                        rvm.Success = false;
                        rvm.Msg = "New failure";
                    }
                }
            }
            return rvm;
        }
        
    }
}
