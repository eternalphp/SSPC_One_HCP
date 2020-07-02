using Newtonsoft.Json;
using SSPC_One_HCP.RongCloud.models;
using System;

namespace SSPC_One_HCP.RongCloud.response
{
    public class ChatroomKeepaliveResult : Result

    {
        [JsonProperty(PropertyName = "chatrooms")]
        private String[] chatrooms;

        public ChatroomKeepaliveResult(int code, String msg, String[] chatrooms) : base(code, msg)
        {
            this.chatrooms = chatrooms;
        }
        [JsonIgnore]
        public string[] Chatrooms { get => chatrooms; set => chatrooms = value; }


        override
        public String ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
