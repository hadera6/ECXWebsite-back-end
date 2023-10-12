using ECX.Website.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Applicant
{
    public class ApplicantFormDto : BaseDtos
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string EduStatus { get; set; }
        public IFormFile File { get; set; }
    }
}
