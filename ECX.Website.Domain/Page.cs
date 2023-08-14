using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Page : BaseDomainEntity
    {

        public string LangId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CatagoryId { get; set; }
        public string ImgName { get; set; }

    }
}
