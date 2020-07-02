using Newtonsoft.Json;
using SSPC_One_HCP.RongCloud.models;
using System;
using System.Collections.Generic;

namespace SSPC_One_HCP.RongCloud.response
{
    public class ChatroomQueryResult : Result
    {
        public List<ChatroomQuery> ChatRooms { get; set; }



    }

    public class ChatroomQuery
    {
        public string ChrmId { get; set; }
        public string Name { get; set; }
        public string Time { get; set; }

    }

}
