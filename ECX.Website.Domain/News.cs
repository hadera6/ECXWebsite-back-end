using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class News : BaseDomainEntity
    {

        public string LangId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public DateTime ExpDate { get; set; }
        public string ImgName { get; set; }

    }
}
