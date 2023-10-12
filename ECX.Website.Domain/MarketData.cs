using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class MarketData : BaseDomainEntity
    {

        public string LangId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImgName { get; set; }

    }
}
