using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Account : BaseDomainEntity
    {
    
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string ImgName { get; set; }

    }
}
