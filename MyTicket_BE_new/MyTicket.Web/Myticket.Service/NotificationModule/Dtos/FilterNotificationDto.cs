﻿using MYTICKET.BASE.SERVICE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYTICKET.WEB.SERVICE.NotificationModule.Dtos
{
    public class FilterNotificationDto : PagingRequestBaseDto
    {
        public bool? IsSeen { get; set; }
    }
}
