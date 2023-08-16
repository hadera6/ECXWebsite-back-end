using ECX.Website.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.PageCatagory
{
    public class PageCatagoryDto : BaseDtos
    {
        public string LangId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImgName { get; set; }
    }
}
