using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SSPC_One_HCP.Services.Utils
{
    /// <summary>
    /// LdapAuthentication 的摘要说明
    /// </summary>
    public class LdapAuthentication
    {
        /// <summary>
        /// 空构造
        /// </summary>
        public LdapAuthentication()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        private string _path;
        private string _filterAttribute;
        /// <summary>
        /// 带参数构造
        /// </summary>
        /// <param name="path"></param>
        public LdapAuthentication(string path)
        {
            _path = path;
        }

        /// <summary>
        /// 判断是否域用户
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public bool IsAuthenticated(string domain, string username, string pwd)
        {

            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(_path, domainAndUsername, pwd);

            try
            {
                //Bind to the native AdsObject to force authentication.
                object obj = entry.NativeObject;

                DirectorySearcher search = new DirectorySearcher(entry)
                {
                    Filter = "(SAMAccountName=" + username + ")"
                };

                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                //Update the new path to the user in the directory.
                _path = result.Path;
                _filterAttribute = (string)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                throw;
                //return false;
                //throw new Exception("Error authenticating user. " + ex.Message);
            }
            return true;
        }

        /// <summary>
        /// 根据用户名获取所属组名
        /// </summary>
        /// <returns></returns>
        public string GetGroupByUser()
        {
            DirectorySearcher search = new DirectorySearcher(_path);
            search.Filter = "(cn=" + _filterAttribute + ")";
            search.PropertiesToLoad.Add("memberOf");
            StringBuilder groupNames = new StringBuilder();

            try
            {
                SearchResult result = search.FindOne();
                if (result != null)
                {
                    int propertyCount = result.Properties["memberOf"].Count;
                    for (int propertyCounter = 0; propertyCounter < propertyCount; propertyCounter++)
                    {
                        var dn = (string)result.Properties["memberOf"][propertyCounter];
                        var equalsIndex = dn.IndexOf("=", 1, StringComparison.Ordinal);
                        var commaIndex = dn.IndexOf(",", 1, StringComparison.Ordinal);
                        if (-1 == equalsIndex)
                        {
                            return null;
                        }
                        groupNames.Append(dn.Substring((equalsIndex + 1), (commaIndex - equalsIndex) - 1));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error obtaining group names. " + ex.Message);
            }
            return groupNames.ToString();
        }

        /// <summary>
        /// 获取组用户
        /// </summary>
        /// <param name="Groupname">组名</param>
        /// <returns></returns>
        public string[] GetUsersForGroup(string Groupname)
        {
            DirectorySearcher ds = new DirectorySearcher(_path)
            {
                Filter = "(&(objectClass=group)(cn=" + Groupname + "))"
            };
            ds.PropertiesToLoad.Add("member");
            SearchResult r = ds.FindOne();

            if (r.Properties["member"] == null)
            {
                return (null);
            }

            string[] results = new string[r.Properties["member"].Count];
            for (int i = 0; i < r.Properties["member"].Count; i++)
            {
                string theGroupPath = r.Properties["member"][i].ToString();
                results[i] = theGroupPath.Substring(3, theGroupPath.IndexOf(",") - 3);
            }
            return (results);
        }

        /// <summary>
        /// 获取用户所属组
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public string[] GetGroupsForUser(string username)
        {
            DirectorySearcher ds = new DirectorySearcher(_path);
            ds.Filter = "(&(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("memberof");
            SearchResult r = ds.FindOne();

            if (r.Properties["memberof"].Count == 0)
            {
                return (null);
            }

            string[] results = new string[r.Properties["memberof"].Count];
            for (int i = 0; i < r.Properties["memberof"].Count; i++)
            {
                string theGroupPath = r.Properties["memberof"][i].ToString();
                results[i] = theGroupPath.Substring(3, theGroupPath.IndexOf(",") - 3);
            }
            return (results);
        }

        public string[] GetAllGroupsForUser(string username)
        {
            DirectorySearcher ds = new DirectorySearcher(_path);
            ds.Filter = "(&(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("memberof");
            SearchResult r = ds.FindOne();
            if (r.Properties["memberof"] == null)
            {
                return (null);
            }
            string[] results = new string[r.Properties["memberof"].Count + 1];
            for (int i = 0; i < r.Properties["memberof"].Count; i++)
            {
                string theGroupPath = r.Properties["memberof"][i].ToString();
                results[i] = theGroupPath.Substring(3, theGroupPath.IndexOf(",") - 3);
            }
            results[r.Properties["memberof"].Count] = "All";//All组属于任何人,在AD之外定义了一个组，以便分配用户权限
            return (results);
        }

        /// <summary>
        /// 获取组用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>

        public string GetUserDisplayName(string username)
        {
            string results;
            DirectorySearcher ds = new DirectorySearcher(_path);
            ds.Filter = "(&(objectClass=user)(sAMAccountName=" + username + "))";
            ds.PropertiesToLoad.Add("DisplayName");
            SearchResult r = ds.FindOne();
            results = r.GetDirectoryEntry().InvokeGet("DisplayName").ToString();
            return (results);

        }



        public string GetAdGroupDescription(string prefix)//根据CN获取组description
        {
            string results;
            DirectorySearcher groupsDS = new DirectorySearcher(_path);
            groupsDS.Filter = "(&(objectClass=group)(CN=" + prefix + "*))";
            groupsDS.PropertiesToLoad.Add("cn");
            SearchResult sr = groupsDS.FindOne();
            results = sr.GetDirectoryEntry().InvokeGet("description").ToString();
            return (results);
        }

        public DataTable GetAdGroupInfo()//根据CN获取组信息
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("URL", typeof(System.String));
            dt.Columns.Add("cn", typeof(System.String));
            dt.Columns.Add("Description", typeof(System.String));

            DirectorySearcher searcher = new DirectorySearcher(_path);

            searcher.Filter = "(&(objectClass=group))";
            //searcher.SearchScope = SearchScope.Subtree;
            //searcher.Sort = new SortOption("description", System.DirectoryServices.SortDirection.Ascending);
            searcher.PropertiesToLoad.AddRange(new string[] { "cn", "description" });
            SearchResultCollection results = searcher.FindAll();
            if (results.Count == 0)
            {
                return (null);
            }
            else
            {
                foreach (SearchResult result in results)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = result.Path.ToString();
                    dr[1] = result.GetDirectoryEntry().InvokeGet("cn").ToString();
                    if (result.GetDirectoryEntry().InvokeGet("Description") != null)
                        dr[2] = result.GetDirectoryEntry().InvokeGet("Description").ToString();
                    else
                        dr[2] = result.GetDirectoryEntry().InvokeGet("cn").ToString();
                    dt.Rows.Add(dr);
                }
                dt.DefaultView.Sort = "description ASC";
                return dt;
            }

        }

        public string getAccountName(string cn) //根据CN获取登陆名
        {
            foreach (string path in _path.Split(','))
            {
                DirectorySearcher ds = new DirectorySearcher(path);
                ds.Filter = "(&(objectClass=user)(cn=*" + cn + "*))";
                ds.PropertiesToLoad.Add("sAMAccountName");
                SearchResult r = ds.FindOne();
                if (r != null)
                    return r.GetDirectoryEntry().InvokeGet("sAMAccountName").ToString();
            }
            return null;
        }

        public DataTable adUserlist(string groupname)   //生成用户数据表
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("cn", typeof(System.String));
            dt.Columns.Add("sAMAccountName", typeof(System.String));
            string[] groupmember = GetUsersForGroup(groupname);
            if (groupmember.Length == 0)
            {
                return null;
            }
            else
            {
                foreach (string member in groupmember)
                {
                    if (IsAccountActive(getAccountControl(getAccountName(member))))
                    {
                        DataRow dr = dt.NewRow();
                        dr[0] = member.ToString();
                        dr[1] = getAccountName(member);
                        dt.Rows.Add(dr);
                    }
                }
                return dt;

            }
        }



        public DataTable adUserlist()   //生成指定的用户信息数据表
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("memberof", typeof(System.String));
            dt.Columns.Add("cn", typeof(System.String));
            dt.Columns.Add("Description", typeof(System.String));
            dt.Columns.Add("name", typeof(System.String));
            dt.Columns.Add("Mail", typeof(System.String));
            dt.Columns.Add("samaccountname", typeof(System.String));
            dt.Columns.Add("whencreated", typeof(System.String));
            dt.Columns.Add("title", typeof(System.String));
            dt.Columns.Add("department", typeof(System.String));
            DirectorySearcher searcher = new DirectorySearcher(_path);
            //searcher.Filter = "(description=ADPJ*)";
            searcher.Filter = "(description=ADPL*)";
            searcher.PropertiesToLoad.AddRange(new string[] { "memberof", "cn", "description", "name", "Mail", "samaccountname", "whencreated", "title", "department" });
            SearchResultCollection results = searcher.FindAll();

            if (results.Count == 0)
            {
                return (null);
            }
            else
            {
                foreach (SearchResult result in results)
                {

                    DataRow dr = dt.NewRow();
                    //dr[0] = result.Path.ToString();
                    if (result.GetDirectoryEntry().InvokeGet("memberof") != null)
                        dr[0] = result.GetDirectoryEntry().InvokeGet("memberof").ToString();
                    else
                        dr[0] = "";
                    if (result.GetDirectoryEntry().InvokeGet("cn") != null)
                        dr[1] = result.GetDirectoryEntry().InvokeGet("cn").ToString();
                    else
                        dr[1] = "";

                    if (result.GetDirectoryEntry().InvokeGet("Description") != null)
                        dr[2] = result.GetDirectoryEntry().InvokeGet("Description").ToString();
                    else
                        dr[2] = result.GetDirectoryEntry().InvokeGet("cn").ToString();
                    if (result.GetDirectoryEntry().InvokeGet("name") != null)
                        dr[3] = result.GetDirectoryEntry().InvokeGet("name").ToString();
                    else
                        dr[3] = "";
                    if (result.GetDirectoryEntry().InvokeGet("Mail") != null)
                        dr[4] = result.GetDirectoryEntry().InvokeGet("Mail").ToString();
                    else
                        dr[4] = "";
                    if (result.GetDirectoryEntry().InvokeGet("samaccountname") != null)
                        dr[5] = result.GetDirectoryEntry().Properties["samaccountname"].Value.ToString();
                    else
                        dr[5] = "";
                    if (result.GetDirectoryEntry().InvokeGet("whencreated") != null)
                        dr[6] = result.GetDirectoryEntry().Properties["whencreated"].Value.ToString();
                    else
                        dr[6] = "";

                    if (result.GetDirectoryEntry().InvokeGet("title") != null)
                        dr[7] = result.GetDirectoryEntry().Properties["title"].Value.ToString();
                    else
                        dr[7] = "";
                    if (result.GetDirectoryEntry().InvokeGet("department") != null)
                        dr[8] = result.GetDirectoryEntry().Properties["department"].Value.ToString();
                    else
                        dr[8] = "";

                    dt.Rows.Add(dr);
                }
                dt.DefaultView.Sort = "description ASC";
                return dt;
            }
        }

        public void adUserlistbox(ListBox results, string groupName)  //生成USER
        {
            results.Items.Clear();
            DataTable dt = adUserlist(groupName);
            if (dt != null)
            {
                results.DataSource = dt;
                results.DataTextField = dt.Columns[0].Caption;
                results.DataValueField = dt.Columns[1].Caption;
                results.DataBind();
            }
        }

        public void adGrouplistbox(ListBox results)
        {
            results.Items.Clear();
            DataTable dt = GetAdGroupInfo();
            DataRow dr = dt.NewRow();
            dr[1] = "All";
            dr[2] = "All";
            dt.Rows.Add(dr);
            results.DataSource = dt;
            results.DataTextField = dt.Columns[2].Caption;
            results.DataValueField = dt.Columns[1].Caption;
            results.DataBind();
        }
        
        public void aduserGrouplist(DropDownList results)
        {
            results.Items.Clear();
            DataTable dt = GetAdGroupInfo();
            results.DataSource = dt;
            results.DataTextField = dt.Columns[2].Caption;
            results.DataValueField = dt.Columns[1].Caption;
            results.DataBind();
        }

        public int getAccountControl(string accountName)//获取权限码
        {
            int results;
            DirectorySearcher ds = new DirectorySearcher(_path);
            ds.Filter = "(&(objectClass=user)(sAMAccountName=" + accountName + "))";
            ds.PropertiesToLoad.Add("userAccountControl");
            try
            {
                SearchResult r = ds.FindOne();
                results = Convert.ToInt32(r.GetDirectoryEntry().InvokeGet("userAccountControl"));
                return results;
            }
            catch
            {
                return 0;
            }

        }

        public bool IsAccountActive(int userAccountControl)//判断是否有效
        {
            int ADS_UF_ACCOUNTDISABLE = 0X0002;
            int userAccountControl_Disabled = Convert.ToInt32(ADS_UF_ACCOUNTDISABLE);
            int flagExists = userAccountControl & userAccountControl_Disabled;
            if (flagExists > 0)
                return false;
            else
                return true;
        }

        public DirectoryEntry GetDirectoryEntryByAccount(string sAMAccountName)
        {
            DirectorySearcher deSearch = new DirectorySearcher(_path);
            deSearch.Filter = "(&(objectCategory=person)(objectClass=user)(sAMAccountName=" + sAMAccountName + "))";
            // deSearch.SearchScope = SearchScope.Subtree;

            try
            {
                SearchResult result = deSearch.FindOne();
                if (result == null)
                { return null; }
                DirectoryEntry de = new DirectoryEntry(_path);
                return de;
            }
            catch
            {
                //throw;
                return null;
            }
        }

    }
}
