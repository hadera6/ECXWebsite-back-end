using ECX.Website.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.FeedBack
{
    public class FeedBackDto : BaseDtos
    {
        public string LangId { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
    }
}
