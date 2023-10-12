using ECX.Website.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Applicant
{
    public class ApplicantDto : BaseDtos
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string EduStatus { get; set; }
        public string FileName { get; set; }
    }
}
