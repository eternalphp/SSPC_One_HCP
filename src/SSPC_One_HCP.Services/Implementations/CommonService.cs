using Newtonsoft.Json;
using NPOI.SS.UserModel;
using SSPC_One_HCP.Core.Data;
using SSPC_One_HCP.Core.Domain.CommonModels;
using SSPC_One_HCP.Core.Domain.Enums;
using SSPC_One_HCP.Core.Domain.Models.DataModels;
using SSPC_One_HCP.Core.Domain.Models.IdentityEntities;
using SSPC_One_HCP.Core.Domain.SspcModels;
using SSPC_One_HCP.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SSPC_One_HCP.Services.Utils;

namespace SSPC_One_HCP.Services.Implementations
{
    public class CommonService : ICommonService
    {
        private readonly IEfRepository _rep;
        private readonly string _DownloadUrl = ConfigurationManager.AppSettings["DownloadUrl"];

        public CommonService(IEfRepository rep)
        {
            _rep = rep;
        }
        /// <summary>
        /// 获取下拉框选项
        /// </summary>
        /// <param name="selectType">下拉框类型</param>
        /// <returns></returns>
        public ReturnValueModel GetSelectList(string selectType)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = _rep.Where<DropDownConfig>(s => s.DropDownType == selectType).OrderBy(s => s.DropDownValue);
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                list = list
            };
            return rvm;
        }
        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel GetCompanyList()
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var list = _rep.Table<CompanyInfo>();
            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                list = list
            };
            return rvm;
        }

        /// <summary>
        /// 审批状态获取
        /// </summary>
        /// <param name="status">审批状态key</param>
        /// <param name="assetsMainId">单据主键</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public string GetStatusStr(int? status, string assetsMainId, WorkUser workUser)
        {
            var statusStr = "";
            switch (status)
            {
                case (int)EnumApproval.Draft:
                    statusStr = EnumApproval.Draft.ToString();
                    break;
                case (int)EnumApproval.Pending:
                    var ar = _rep.Where<ApprovalRecord>(s => s.AssetsMainId == assetsMainId);
                    var submiter = ar.FirstOrDefault(s => s.OperationAction == "8021")?.OperationUser ?? "";
                    statusStr = ar.Any(s => s.OperationUser.Contains(workUser.User.Code)) && ar.OrderByDescending(o => o.OperationDate).FirstOrDefault().OperationAction == "8202" && !submiter.Contains(workUser.User.Code) ? EnumApproval.Approved.ToString() : EnumApproval.Pending.ToString();
                    break;
                case (int)EnumApproval.Reject:
                    statusStr = EnumApproval.Reject.ToString();
                    break;
                case (int)EnumApproval.CompletionApproval:
                    statusStr = EnumApproval.CompletionApproval.ToString();
                    break;
                case (int)EnumApproval.Receiving:
                    statusStr = EnumApproval.Receiving.ToString();
                    break;
                case (int)EnumApproval.Complete:
                    statusStr = EnumApproval.Complete.ToString();
                    break;
                case (int)EnumApproval.Obsolete:
                    statusStr = EnumApproval.Obsolete.ToString();
                    break;
                case (int)EnumApproval.PsSendError:
                    statusStr = EnumApproval.PsSendError.ToString();
                    break;
                default:
                    statusStr = "";
                    break;
            }
            return statusStr;
        }
        
        /// <summary>
        /// 根据AD域账户获取公司
        /// </summary>
        /// <param name="accountName">AD域账户</param>
        /// <returns></returns>
        public ReturnValueModel GetCompanyCodeByAccount(string accountName)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            var companyCode = _rep.FirstOrDefault<UserModel>(s => s.ADAccount == accountName)?.CompanyCode;

            rvm.Success = true;
            rvm.Msg = "";
            rvm.Result = new
            {
                CompanyCode = companyCode
            };
            return rvm;
        }
        /// <summary>
        /// 导入多语言
        /// </summary>
        /// <param name="httpRequest">请求内容</param>
        /// <returns></returns>
        public ReturnValueModel UploadLanguageConfig(HttpRequest httpRequest)
        {
            ReturnValueModel rvm = new ReturnValueModel();
            HttpFileCollection files = httpRequest.Files;
            if (files.Count != 1)//文件数只能是1个
            {
                rvm.Success = false;
                rvm.Msg = "";
                rvm.Result = new
                {
                    fileCount = files.Count
                };
            }
            else
            {
                var lanFile = files[0];
                IWorkbook workbook = WorkbookFactory.Create(lanFile.InputStream);
                ISheet sheet = workbook.GetSheetAt(0);
                IRow firstRow = sheet.GetRow(0);
                int columnNum = firstRow.PhysicalNumberOfCells;//总列数
                int rowNum = sheet.LastRowNum;//总行数
                var all = _rep.Table<LanguageConfig>();
                _rep.DeleteList(all);
                _rep.SaveChanges();
                List<LanguageConfig> lcList = new List<LanguageConfig>();
                for (int i = 1; i < columnNum; i++)
                {
                    for (int j = 1; j < rowNum; j++)
                    {
                        if (sheet?.GetRow(j)?.GetCell(i) == null)
                        {
                            continue;
                        }
                        LanguageConfig lc = new LanguageConfig
                        {
                            LanKey = sheet?.GetRow(j).GetCell(0).StringCellValue,//获取第0列的数据
                            LanType = sheet?.GetRow(0).GetCell(i).StringCellValue,//获取第0行的数据
                            LanValue = sheet?.GetRow(j)?.GetCell(i)?.StringCellValue,
                            CreateTime = DateTime.Now
                        };
                        lcList.Add(lc);
                        _rep.Insert(lc);
                    }
                }


                //_rep.InsertList(lcList);
                _rep.SaveChanges();
                rvm.Success = true;
                rvm.Msg = "";
                rvm.Result = new
                {
                    lcList
                };
            }

            return rvm;
        }
        /// <summary>
        /// 导出语言包
        /// </summary>
        /// <returns></returns>
        public ReturnValueModel ExportLanguageConfig()
        {
            ReturnValueModel rvm = new ReturnValueModel();

            var lan = _rep.All<LanguageConfig>().OrderBy(s => s.LanKey).ToList();
            var typeList = lan.Select(s => s.LanType).Distinct();
            var path = AppDomain.CurrentDomain.BaseDirectory + "/i18n/languages/";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (var type in typeList)
            {
                var lanJson = path + type + ".json";
                if (File.Exists(lanJson))
                {
                    File.Delete(lanJson);
                }
                FileStream fs = new FileStream(lanJson, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                using (StreamWriter file = new StreamWriter(fs, Encoding.UTF8))
                {
                    file.WriteLine("{");
                    var json = lan.Where(s => s.LanType == type)
                        .Select(s => "      \"" + s.LanKey + "\": \"" + s.LanValue + "\"")
                        .Aggregate((s, a) => s + ",\n" + a + "");
                    file.WriteLine($"{json}");
                    file.WriteLine("}");
                }
            }
            rvm.Success = true;
            rvm.Msg = "success";
            rvm.Result = new
            {
                lan
            };

            return rvm;
        }
        /// <summary>
        /// 获取IT权限
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public string GetItRoleId(string companyCode)
        {
            string roleId = string.Empty;
            switch (companyCode)
            {
                case "4031":
                    roleId = "F1198028-50B0-4599-96CE-02197E5BFCFF";
                    break;
                case "4036":
                    roleId = "A1A56277-A0CE-47B7-BC9D-A91FF189DAF5";
                    break;
                case "403I":
                    roleId = "A8BEF397-765A-418F-8316-0794EE469045";
                    break;
                case "4033":
                    roleId = "84506F59-F415-46AD-A089-DE4B998969CB";
                    break;
                case "-1":
                    roleId = "108A0B56-78DF-403D-B894-8B16F3C4A3E7";//superadmin
                    break;
                default:
                    roleId = "108A0B56-78DF-403D-B894-8B16F3C4A3E7";
                    break;
            }

            return roleId;
        }
        /// <summary>
        /// 获取admin权限
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public string GetAdminRoleId(string companyCode)
        {
            string roleId = "";
            switch (companyCode)
            {
                default:
                    roleId = "002222B5-C4D4-4DD7-9FEE-53201BD2BA11";
                    break;
            }

            return roleId;
        }
        /// <summary>
        /// 判断是否为管理员
        /// </summary>
        /// <param name="companyCode">公司编号</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public bool IsAdmin(WorkUser workUser)
        {
            if (workUser.Roles == null) return false;

            var roles = workUser.Roles.Select(s => s.Id).ToList();
            var adminRole = GetAdminRoleId(workUser.User?.CompanyCode);
            var isAdmin = roles.Contains(adminRole);
            return isAdmin;
        }
        /// <summary>
        /// 获取BU admin权限
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public string GetBUAdminRoleId(string companyCode)
        {
            string roleId = "";
            switch (companyCode)
            {
                default:
                    roleId = "002222B5-C4D4-4DD7-9FEE-53201BD2BA22";
                    break;
            }

            return roleId;
        }
        /// <summary>
        /// 判断是否为BU管理员
        /// </summary>
        /// <param name="companyCode">公司编号</param>
        /// <param name="workUser">当前操作人</param>
        /// <returns></returns>
        public bool IsBuAdmin(WorkUser workUser)
        {
            if (workUser.Roles == null) return false;

            var roles = workUser.Roles.Select(s => s.Id).ToList();
            var buAdminRole = GetBUAdminRoleId(workUser.User?.CompanyCode);
            var isBUAdmin = roles.Contains(buAdminRole);
            return isBUAdmin;
        }

        public void WriteLog(string Content)
        {
            LoggerHelper.WriteLogInfo(Content);
        }
    }
}
