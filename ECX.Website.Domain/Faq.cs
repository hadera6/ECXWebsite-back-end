using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Faq : BaseDomainEntity
    {

        public string LangId { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

    }
}
