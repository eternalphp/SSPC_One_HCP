using System.Web;
using System.Web.Optimization;

namespace SSPC_One_HCP.WebApi
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BundleConfig'
    public class BundleConfig
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BundleConfig'
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member 'BundleConfig.RegisterBundles(BundleCollection)'
        public static void RegisterBundles(BundleCollection bundles)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member 'BundleConfig.RegisterBundles(BundleCollection)'
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
