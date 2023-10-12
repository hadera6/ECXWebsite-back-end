using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Subscription : BaseDomainEntity
    {

        public string Email { get; set; }
        public string SubscriberName { get; set; }
    }
}
