using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Message : BaseDomainEntity
    {

        public string LangId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        
    }
}
