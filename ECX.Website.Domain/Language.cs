using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Language : BaseDomainEntity
    {

        public string Name { get; set; }
        public string ShortName { get; set; }
      
    }
}
