using ECX.Website.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Subscription
{
    public class SubscriptionDto : BaseDtos
    {
        public string Email { get; set; }
        public string SubscriberName { get; set; }
    }
}
