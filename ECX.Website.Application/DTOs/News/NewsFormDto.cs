using ECX.Website.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.News
{
    public class NewsFormDto : BaseDtos
    {
        public string LangId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public DateTime ExpDate { get; set; }
        public IFormFile ImgFile{get;set;}
    }
}
