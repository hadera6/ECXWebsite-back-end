using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class Applicant : BaseDomainEntity
    {

        public string FName { get; set; }
        public string LName { get; set; }
        public string EduStatus { get; set; }
        public string FileName { get; set; }
        
    }
}
