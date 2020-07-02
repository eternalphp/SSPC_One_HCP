using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServices
{
    public class WxPackage
    {
        public class Token
        {
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }

        public class Send
        {
            /// <summary>
            /// 接口调用凭证
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 	接收者（用户）的 openid
            /// </summary>
            public string touser { get; set; }
            /// <summary>
            /// 	所需下发的模板消息的id
            /// </summary>
            public string template_id { get; set; }

            /// <summary>
            /// 表单提交场景下，为 submit 事件带上的 formId；支付场景下，为本次支付的 prepay_id
            /// </summary>
            public string form_id { get; set; }
            /// <summary>
            /// 	点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
            /// </summary>
            public string page { get; set; }
            /// <summary>
            /// 模板内容，不填则下发空模板。具体格式请参考示例。
            /// </summary>
            public SendData data { get; set; }
        }

        public class SendData {
            public SendValue keyword1 { get; set; }
            public SendValue keyword2 { get; set; }
        }

        public class SendValue
        {
            public string value { get; set; }
        }
    }
}
