﻿using SSPC_One_HCP.Core.Domain.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSPC_One_HCP.Data.Mappings.DataMappings
{
    public class RongCloudChatRoomHandleMap : BaseEntityTypeConfiguration<RongCloudChatRoomHandle>
    {
        public RongCloudChatRoomHandleMap()
        {
            this.ToTable("RongCloudChatRoomHandle");

        }
    }
}
