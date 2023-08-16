using ECX.Website.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Page
{
    public class PageFormDto : BaseDtos
    {
        public string LangId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CatagoryId { get; set; }
        public IFormFile ImgFile{get;set;}
    }
}
