using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Core.Utils
{
    /// <summary>
    /// 基本配置
    /// </summary>
    public class BaseConfig : IConfig
    {
        public BaseConfig()
        {
        }


        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置），请妥善保管，避免密钥泄露
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置），请妥善保管，避免密钥泄露
        */

        public string GetAppID()
        {
            return ConfigurationManager.AppSettings["AppId"];
        }

        public string GetCommercialAppId()
        {
            return ConfigurationManager.AppSettings["CommercialAppId"];
        }
        public string GetMchID()
        {
            return ConfigurationManager.AppSettings["MCHID"];
        }
        public string GetKey()
        {
            return ConfigurationManager.AppSettings["MCHKEY"];
        }
        public string GetAppSecret()
        {
            return ConfigurationManager.AppSettings["AppSecret"];
        }



        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
         * 1.证书文件不能放在web服务器虚拟目录，应放在有访问权限控制的目录中，防止被他人下载；
         * 2.建议将证书文件名改为复杂且不容易猜测的文件
         * 3.商户服务器要做好病毒和木马防护工作，不被非法侵入者窃取证书文件。
        */
        public string GetSSlCertPath()
        {
            return ConfigurationManager.AppSettings["CertPath"];
        }
        public string GetSSlCertPassword()
        {
            return ConfigurationManager.AppSettings["CertPassword"];
        }



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public string GetNotifyUrl()
        {
            return ConfigurationManager.AppSettings[""];
        }

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public string GetIp()
        {
            return ConfigurationManager.AppSettings["macIp"];
        }


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public string GetProxyUrl()
        {
            return ConfigurationManager.AppSettings[""];
        }


        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public int GetReportLevel()
        {
            return 1;
        }


        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public int GetLogLevel()
        {
            return 1;
        }
        //================【HCP小程序】===========================================
        /// <summary>
        /// 获取HCP小程序appid
        /// </summary>
        /// <returns></returns>
        public string GetAppIdHcp()
        {
            return ConfigurationManager.AppSettings["xAppId"];
        }
        /// <summary>
        /// 获取HCP小程序appsecret
        /// </summary>
        /// <returns></returns>
        public string GetAppSecretHcp()
        {
            return ConfigurationManager.AppSettings["xAppSecret"];
        }

        //================【对接费卡文库】===========================================
        /// <summary>
        /// 获取费卡文库appid
        /// </summary>
        /// <returns></returns>
        public string GetFkLibAppId()
        {
            return ConfigurationManager.AppSettings["FkLibAppId"];
        }

        //================【多福助手公众号】===========================================
        /// <summary>
        /// 多福助手公众号 appid
        /// </summary>
        /// <returns></returns>
        public string GetAppIdDF()
        {
            return ConfigurationManager.AppSettings["dfAppId"];
        }
        /// <summary>
        /// 多福助手公众号 appsecret
        /// </summary>
        /// <returns></returns>
        public string GetAppSecretDF()
        {
            return ConfigurationManager.AppSettings["dfAppSecret"];
        }
    }
}
