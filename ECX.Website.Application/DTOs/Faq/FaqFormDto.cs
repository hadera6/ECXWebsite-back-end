using ECX.Website.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Faq
{
    public class FaqFormDto : BaseDtos
    {
        public string LangId { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
