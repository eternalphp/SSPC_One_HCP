using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.ViewModels;
using SSPC_One_HCP.Core.Domain.ViewModels.Approval;
using SSPC_One_HCP.Core.LinqExtented;
using SSPC_One_HCP.Services.Interfaces;
using SSPC_One_HCP.Services.Utils;
using System;
using System.Configuration;
using System.Linq;


namespace SSPC_One_HCP.Services.Implementations
{
  public class ManagementService:IManagementService
    {
        private readonly IEfRepository _rep;
        private readonly ICommonService _commonService;
        private readonly ISystemService _systemService;

        public ManagementService(IEfRepository rep, ICommonService commonService, ISystemService systemService) {
            _rep = rep;
            _commonService = commonService;
            _systemService = systemService;
        }
        /// <summary>
        ///添加或修改短信模板
        /// </summary>
        /// <param name="manage">添加的对象</param>
        /// <param name="workUser">创建人或者修改人</param>
        /// <returns></returns>
        public ReturnValueModel AddorUpdateManagement(Management manage,WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            //首先查询数据库是否已经存在该短信模板
            var data = _rep.FirstOrDefault<Management>(s => s.ManagementId == manage.ManagementId );

            var approvalEnabled = _systemService.AdminApprovalEnabled; //是否启用审核
            if (data == null)
            {
                manage.CreateTime = DateTime.Now;
                manage.CreateUser = workUser.User.Id;
                manage.IsCompleted = approvalEnabled ? EnumComplete.AddedUnapproved : EnumComplete.Approved;
            }
            else {
                int num_ber;
                var send1 = _rep.SqlQuery<Management>("select * from Management").OrderByDescending(s => Convert.ToInt16(s.Id)).FirstOrDefault();
                if (send1 == null)
                {
                    num_ber = 1;
                }
                else
                {
                    num_ber = Convert.ToInt32(send1.Id) + 1;
                }
               
                if (data.IsDeleted == 1)
                {
                    rvm.Msg = "This DataInfo has been deleted.";
                    rvm.Success = false;
                    return rvm;
                }
                Management oldman = new Management();
                switch (data.IsCompleted ?? EnumComplete.AddedUnapproved)
                {
                    
                    case EnumComplete.Approved:
                        if (approvalEnabled)
                        {
                            oldman.Id = num_ber.ToString();
                            oldman.CreateUser = workUser.User.Id;
                            oldman.CreateTime = DateTime.Now;
                            oldman.ManagementId = Guid.NewGuid().ToString();
                            oldman.ManagementWord = data.ManagementWord;
                            oldman.OldManagementId = data.ManagementId;
                            oldman.IsCompleted = data.IsCompleted;
                            oldman.IsDeleted = 1;
                            _rep.Insert(oldman);
                            _rep.SaveChanges();
                            _rep.Database.BeginTransaction().Commit();
                            data.IsCompleted = EnumComplete.UpdatedUnapproved;
                            data.ManagementWord = manage.ManagementWord;
                            data.UpdateUser = workUser.User.ChineseName;
                            data.UpdateTime = DateTime.Now;
                            _rep.Update(data);
                            _rep.SaveChanges();
                            
                        }
                        else{
                            data.IsCompleted = EnumComplete.Approved;
                            data.ManagementWord = manage.ManagementWord;
                            data.UpdateUser = workUser.User.ChineseName;
                            data.UpdateTime = DateTime.Now;
                            _rep.Update(data);
                            _rep.SaveChanges();
                        }
                        break;
                    case EnumComplete.Reject:
                        //查询该数据是否存在备份数据存在则为编辑待审核拒绝不存在则为新增待审核拒绝
                        var IsCopyReject = _rep.FirstOrDefault<Management>(s=>s.OldManagementId==manage.ManagementId);
                        if (IsCopyReject == null)
                        {
                            data.IsCompleted = EnumComplete.AddedUnapproved;
                        }
                        else {
                            data.IsCompleted = EnumComplete.UpdatedUnapproved;
                        }
                        data.ManagementWord = manage.ManagementWord;
                        data.UpdateUser = workUser.User.ChineseName;
                        data.UpdateTime = DateTime.Now;
                        _rep.Update(data);
                        _rep.SaveChanges();
                        break;
                    case EnumComplete.AddedUnapproved:
                        data.IsCompleted = EnumComplete.AddedUnapproved;
                        data.ManagementWord = manage.ManagementWord;
                        data.UpdateTime = DateTime.Now;
                        data.UpdateUser = workUser.User.ChineseName;
                        _rep.Update(data);
                        _rep.SaveChanges();
                        break;
                    case EnumComplete.UpdatedUnapproved:
                        data.IsCompleted = EnumComplete.UpdatedUnapproved;
                        data.UpdateUser = workUser.User.ChineseName;
                        data.UpdateTime = DateTime.Now;
                        data.ManagementWord = manage.ManagementWord;
                        _rep.Update(data);
                        _rep.SaveChanges();
                        //查询是否存在备份数据
                        //var Copymanage = _rep.FirstOrDefault<Management>(s=>s.OldManagementId==manage.ManagementId).OldManagementId;
                        //data.IsCompleted = string.IsNullOrEmpty(Copymanage) ? EnumComplete.AddedUnapproved : EnumComplete.UpdatedUnapproved;
                        break;
                    default:
                        rvm.Msg = "This DataInfo can not be changed at this time.";
                        rvm.Success = false;
                        return rvm;
                }
            }
            if (data == null)
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        int num_ber;
                        var send1 = _rep.SqlQuery<Management>("select * from Management").OrderByDescending(s=>Convert.ToInt16(s.Id)).FirstOrDefault();
                        if (send1 == null) {
                             num_ber = 1;
                        }
                        else {
                            num_ber = Convert.ToInt32(send1.Id) + 1;
                        }
                        Management manages = new Management();
                        manages.Id = num_ber.ToString();
                        manages.ManagementId = Guid.NewGuid().ToString();
                        manages.ManagementWord = manage.ManagementWord;
                        manages.CreateTime = manage.CreateTime;
                        manages.CreateUser = manage.CreateUser;
                        manages.IsDeleted = 0;
                        manages.IsCompleted = manage.IsCompleted;
                        _rep.Insert<Management>(manages);
                        _rep.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        rvm.Msg = ex.Message;
                        rvm.Success = false;
                        return rvm;
                    }
                    tran.Commit();
                    tran.Dispose();
                }

            }
            else
            {
                using (var tran = _rep.Database.BeginTransaction())
                {
                    try
                    {
                        
                        //data.ClickVolume = productInfoView.dataInfo.ClickVolume;
                        data.ManagementWord = manage.ManagementWord;
                        data.UpdateUser = workUser.User.Id;
                        data.Remark = manage.Remark;
                        _rep.Update(data);
                        _rep.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        tran.Dispose();
                        rvm.Msg = "fail";
                        rvm.Success = false;
                        return rvm;
                    }
                    tran.Commit();
                    tran.Dispose();
                }
            }

            rvm.Msg = "success";
            rvm.Success = true;
            rvm.Result = data ?? manage;
            return rvm;
        }
        /// <summary>
        /// 获取所有短信模板
        /// </summary>
        /// <param name="manage">获取到的短信集合</param>
        /// <param name="workUser">当前登录用户</param>
        /// <returns></returns>
        public ReturnValueModel GetManagementList(RowNumModel<Management> manage,WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            var Users = _rep.SqlQuery<UserInfo>("select * from UserInfo where Id='"+workUser.User.Id+"'").FirstOrDefault();
            if (Users != null)
            {
                if (Users.EnglishName == "admin")
                {
                    var data = from a in _rep.SqlQuery<Management>("select * from Management where IsDeleted=0")
                               join b in _rep.SqlQuery<UserInfo>("select * from UserInfo where IsDeleted=2 ") on a.CreateUser equals b.Id
                               select new Management
                               {
                                   ManagementId = a.ManagementId,
                                   ManagementWord = a.ManagementWord,
                                   CreateUser = b.ChineseName,
                                   UpdateUser = b.EnglishName,
                                   IsCompleted = a.IsCompleted,
                                   Id = a.Id,
                                   Remark = a.Remark
                               };

                    if (data == null)
                    {
                        rvm.Success = false;
                        rvm.Msg = "No SMS Template";
                    }
                    else
                    {
                        if (manage.SearchParams != null)
                        {
                            if (!string.IsNullOrEmpty(manage.SearchParams.ManagementWord))
                            {
                                data = data.Where(s => s.ManagementWord.Contains(manage.SearchParams.ManagementWord));
                            }
                            if (!string.IsNullOrEmpty(manage.SearchParams.IsCompleted.ToString()))
                            {
                                data = data.Where(s => s.IsCompleted == manage.SearchParams.IsCompleted);
                            }
                            if (!string.IsNullOrEmpty(manage.SearchParams.CreateUser))
                            {
                                var SendUser = _rep.Where<UserInfo>(s => s.ChineseName == manage.SearchParams.CreateUser && s.IsDeleted != 1).FirstOrDefault();
                                data = data.Where(s => s.CreateUser.Contains(SendUser.ChineseName));
                            }
                            if (manage.SearchParams.IsCompletedList!=null) {
                                var data1 = data.Where(s => Convert.ToInt16(s.IsCompleted) == 2 && s.IsDeleted!=1);
                                var data2 = data.Where(s => Convert.ToInt16(s.IsCompleted) == 6 && s.IsDeleted != 1);
                                var data3 = data.Where(s => Convert.ToInt16(s.IsCompleted) == 8 && s.IsDeleted != 1);
                                data = (data1.Union(data2)).Union(data3);
                            }
                           
                        }

                        var total = data.Count();
                        //先按照Sort字段升序排列，再按照创建时间倒序排列
                        var rows = data.OrderByDescending(s => Convert.ToInt32(s.Id)).ToPaginationList(manage.PageIndex, manage.PageSize);
                        rvm.Success = true;
                        rvm.Msg = "Success";
                        rvm.Result = new
                        {
                            total = total,
                            rows = rows
                        };
                    }
                }
                else {
                    var data = from a in _rep.SqlQuery<Management>("select * from Management where CreateUser='" + workUser.User.Id + "' and IsDeleted=0")
                               join b in _rep.SqlQuery<UserInfo>("select * from UserInfo where IsDeleted=2 ") on a.CreateUser equals b.Id
                               select new Management
                               {
                                   ManagementId = a.ManagementId,
                                   ManagementWord = a.ManagementWord,
                                   CreateUser = b.ChineseName,
                                   IsCompleted = a.IsCompleted,
                                   UpdateUser=b.EnglishName,
                                   Id=a.Id,
                                   Remark=a.Remark
                               };
                    if (data == null)
                    {
                        rvm.Success = false;
                        rvm.Msg = "No SMS Template";
                    }
                    else
                    {
                        if (manage.SearchParams != null)
                        {
                            if (!string.IsNullOrEmpty(manage.SearchParams.ManagementWord))
                            {
                                data = data.Where(s => s.ManagementWord.Contains(manage.SearchParams.ManagementWord));
                            }
                            if (!string.IsNullOrEmpty(manage.SearchParams.IsCompleted.ToString()))
                            {
                                data = data.Where(s => s.IsCompleted == manage.SearchParams.IsCompleted);
                            }
                            if (!string.IsNullOrEmpty(manage.SearchParams.CreateUser))
                            {
                                var SendUser = _rep.Where<UserInfo>(s => s.ChineseName == manage.SearchParams.CreateUser && s.IsDeleted != 1).FirstOrDefault();
                                data = data.Where(s => s.CreateUser.Contains(SendUser.ChineseName));
                            }
                        }
                        
                        var total = data.Count();
                        //先按照Sort字段升序排列，再按照创建时间倒序排列
                        var rows = data.OrderByDescending(s => Convert.ToInt32(s.Id)).ToPaginationList(manage.PageIndex, manage.PageSize);
                        rvm.Success = true;
                        rvm.Msg = "Success";
                        rvm.Result = new
                        {
                            total = total,
                            rows = rows
                        };
                    }
                }
               
               

            }
            else {
                rvm.Success = false;
                rvm.Msg = "The user does not exist";
            }
            return rvm;
        }
        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="manage">需要删除的短信模板</param>
        /// <param name="workUser">操作用户</param>
        /// <returns></returns>
        public ReturnValueModel DeleteManagement(Management manage,WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            ///查询需要删除的数据
            var manageInfo = _rep.FirstOrDefault<Management>(s => s.ManagementId == manage.ManagementId);
            if (manageInfo== null)
            {
                rvm.Success = false;
                rvm.Msg = "Invalid Id";
                return rvm;
            }

            if (manageInfo.IsDeleted == 1)
            {
                rvm.Msg = "This management info has been deleted.";
                rvm.Success = true;
                return rvm;
            }
            var approvalEnabled = _systemService.AdminApprovalEnabled; //是否启用审核
            if (approvalEnabled)
            {
                switch (manageInfo.IsCompleted ?? EnumComplete.AddedUnapproved)
                {
                    case EnumComplete.Approved:
                        manageInfo.IsCompleted = EnumComplete.WillDelete;  //将要删除（等待审核）
                        _rep.Update(manageInfo);
                        _rep.SaveChanges();
                        break;
                    case EnumComplete.Reject:
                        manageInfo.IsCompleted = EnumComplete.Approved;
                        _rep.Update(manageInfo);
                        _rep.SaveChanges();
                        break;
                    case EnumComplete.AddedUnapproved:
                        DoManagementInfoDeletion(manageInfo, workUser);
                        break;
                    case EnumComplete.UpdatedUnapproved:
                        DoManagementInfoDeletion(manageInfo, workUser); //删除数据
                        break;
                    default:
                        rvm.Msg = "This meeting info can not be deleted at this time.";
                        rvm.Success = false;
                        return rvm;
                }

            }
            else {
                DoManagementInfoDeletion(manageInfo, workUser);
            }

            rvm.Msg = "success";
            rvm.Success = true;
            return rvm;
        }
        /// <summary>
        /// 删除短信模板
        /// </summary>
        /// <param name="manage"></param>
        /// <param name="workUser"></param>
        private void DoManagementInfoDeletion(Management manage, WorkUser workUser) {
            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {
                    var deleteManage = _rep.Where<Management>(s => s.ManagementId == manage.ManagementId && s.IsDeleted != 1).FirstOrDefault();

                    deleteManage.IsDeleted = 1;
                   deleteManage.UpdateTime = DateTime.Now;
                   deleteManage.UpdateUser = workUser.User.Id;
                    //files.ForEach(s => s.IsDeleted = 1);
                    //schules.ForEach(s => s.IsDeleted = 1);
                    //speakers.ForEach(s => s.IsDeleted = 1);
                    //meetRelation.ForEach(s => s.IsDeleted = 1);
                    //tags.ForEach(s => s.IsDeleted = 1);

                    if (!string.IsNullOrEmpty(deleteManage.ManagementId))
                    {
                       deleteManage.IsCompleted = EnumComplete.CanceledUpdate; //修改后的副本变为取消修改状态

                        //是否存在需要审核的其它记录
                        bool hasApprovingCopy = _rep.Table<Management>().Any(s => s.IsDeleted != 1 && s.ManagementId != manage.ManagementId);
                        if (!hasApprovingCopy)
                        {
                            //解锁原始数据
                            var manageOriginal = _rep.FirstOrDefault<Management>(s => s.IsDeleted != 1 && s.ManagementId == manage.ManagementId && s.IsCompleted == EnumComplete.Locked);
                            if (manageOriginal != null)
                            {
                                manageOriginal.IsCompleted = EnumComplete.Approved;
                                _rep.Update(manageOriginal);
                            }
                        }
                    }

                    _rep.Update(manage);
                    //_rep.UpdateList(files);
                    //_rep.UpdateList(schules);
                    //_rep.UpdateList(speakers);
                    //_rep.UpdateList(meetRelation);
                    //_rep.UpdateList(tags); //会议标识

                    _rep.SaveChanges();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                }
            }
        }
        /// <summary>
        /// 提交审核结果
        /// </summary>
        /// <param name="approvalResult"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel ManagementApproval(ApprovalResultViewModel approvalResult, WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            //var isAdmin = workUser.Roles.FirstOrDefault().RoleName.Contains("Admin");
            var isAdmin = _commonService.IsAdmin(workUser);
            if (!isAdmin)
            {
                rvm.Msg = "You have no administrator permission.";
                rvm.Success = false;
                return rvm;
            }

            if (approvalResult == null || !approvalResult.Approved.HasValue || string.IsNullOrEmpty(approvalResult.Id))
            {
                rvm.Msg = "Invalid parameter.";
                rvm.Success = false;
                return rvm;
            }
            Management modifiedDataInfo = null, originalDataInfo = null;
            modifiedDataInfo = _rep.FirstOrDefault<Management>(s => s.ManagementId == approvalResult.Id);
            if (modifiedDataInfo == null)
            {
                rvm.Msg = $"Invalid DataInfo Id: {approvalResult.Id}";
                rvm.Success = false;
                return rvm;
            }
            switch (modifiedDataInfo.IsCompleted ?? EnumComplete.AddedUnapproved)
            {
                case EnumComplete.AddedUnapproved:
                case EnumComplete.UpdatedUnapproved:
                case EnumComplete.Reject:
                case EnumComplete.WillDelete:
                    break;
                default:
                    rvm.Msg = $"This DataInfo is not unapproved.";
                    rvm.Success = false;
                    return rvm;
            }
            using (var tran = _rep.Database.BeginTransaction())
            {
                try
                {

                    switch (modifiedDataInfo.IsCompleted ?? EnumComplete.AddedUnapproved)
                    {
                        case EnumComplete.AddedUnapproved:
                            if (approvalResult.Approved ?? false)
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Approved;//审核通过
                                _rep.Update(modifiedDataInfo);
                                _rep.SaveChanges();

                            }
                            else
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Reject;  //审核拒绝
                                modifiedDataInfo.Remark = approvalResult.Note;
                                _rep.Update(modifiedDataInfo);
                                _rep.SaveChanges();

                            }
                            break;
                        case EnumComplete.UpdatedUnapproved:
                            if (approvalResult.Approved ?? false)
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Approved;//审核通过
                                _rep.Update(modifiedDataInfo);
                                _rep.SaveChanges();
                                //删除备份数据
                                var CopyDelete = _rep.FirstOrDefault<Management>(s => s.OldManagementId == modifiedDataInfo.ManagementId);
                                if (CopyDelete != null) {
                                    _rep.Delete(CopyDelete);
                                    _rep.SaveChanges();
                                }
                            }
                            else
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Reject;  //审核拒绝
                                modifiedDataInfo.Remark = approvalResult.Note;
                                _rep.Update(modifiedDataInfo);
                                _rep.SaveChanges();
                            }
                            break;
                        case EnumComplete.Reject:
                            if (approvalResult.Approved ?? false)
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Approved;//审核通过
                                _rep.Update(modifiedDataInfo);
                                _rep.SaveChanges();
                               
                            }
                            else
                            {
                                modifiedDataInfo.IsCompleted = EnumComplete.Reject;  //审核拒绝
                                modifiedDataInfo.Remark = approvalResult.Note;
                                _rep.Update(modifiedDataInfo);
                                _rep.SaveChanges();
                            }
                            break;
                        case EnumComplete.WillDelete:
                            if (originalDataInfo == null)
                            {
                                if (approvalResult.Approved ?? false)
                                {
                                    //同意删除
                                    modifiedDataInfo.IsDeleted = 1;
                                    modifiedDataInfo.IsCompleted = EnumComplete.Deleted;
                                    _rep.Update(modifiedDataInfo);
                                    _rep.SaveChanges();
                                }
                                else
                                {
                                    //拒绝删除
                                    modifiedDataInfo.IsCompleted = EnumComplete.Approved;
                                    _rep.Update(modifiedDataInfo);
                                    _rep.SaveChanges();
                                }
                                
                            }
                            break;
                    }

                    _rep.SaveChanges();

                    tran.Commit();
                    rvm.Msg = "success";
                    rvm.Success = true;
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    rvm.Msg = "fail";
                    rvm.Success = false;
                }
            }
            return rvm;
        }
        /// <summary>
        /// 撤销修改
        /// </summary>
        /// <param name="manAgement"></param>
        /// <param name="workUser"></param>
        /// <returns></returns>
        public ReturnValueModel RevokeUpdateManagement(Management manAgement,WorkUser workUser) {
            ReturnValueModel rvm = new ReturnValueModel();
            //首先查询需要撤销修改的数据
            var manAgementCount =_rep.FirstOrDefault< Management>(s => s.ManagementId == manAgement.ManagementId && s.IsDeleted != 1);
            /*_rep.SqlQuery<Management>("select * from Management Where ManagementId='"+manAgement.ManagementId+"' and IsDeleted=0").FirstOrDefault();*/
            //查询备份的数据
            var copyManagement = _rep.FirstOrDefault< Management>(s=>s.OldManagementId==manAgement.ManagementId);
            //将数据回退，并将备份数据删除
            if (manAgementCount != null)
            {
                //判读备份数据是否存在
                if (copyManagement != null)
                {
                    //Management RevokeUpdate = new Management();
                    //进行数据回滚
                    //manAgementCount.Id = copyManagement.Id;
                    //manAgementCount.ManagementId = copyManagement.ManagementId;
                    //manAgementCount.ManagementWord = copyManagement.ManagementWord;
                    //manAgementCount.IsCompleted = copyManagement.IsCompleted;
                    //manAgementCount.IsDeleted = 0;
                    manAgementCount.UpdateTime = DateTime.Now;
                    manAgementCount.UpdateUser = workUser.User.Id;
                    manAgementCount.ManagementWord = copyManagement.ManagementWord;
                    manAgementCount.IsCompleted = copyManagement.IsCompleted;
                    manAgementCount.IsDeleted = 0;
                    manAgementCount.OldManagementId = null;
                    //保存数据回滚内容
                    _rep.Update(manAgementCount);
                    _rep.SaveChanges();
                    //删除备份数据
                    _rep.Delete(copyManagement);
                    //保存修改内容
                    _rep.SaveChanges();
                    rvm.Success = true;
                    rvm.Msg = "success";
                }
                else {
                    rvm.Success = false;
                    rvm.Msg = "Backup data is lost and cannot be undone";
                }
            }
            else {
                rvm.Success = false;
                rvm.Msg = "No data on this item was queried";
            }
            return rvm;
        }
        
    }
}
