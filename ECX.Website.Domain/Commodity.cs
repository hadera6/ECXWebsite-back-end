using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Commodity : BaseDomainEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }

    }
}
