using ECX.Website.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;

namespace ECX.Website.Application.DTOs.PageCatagory
{
    public class PageCatagoryFormDto : BaseDtos
    {
        public string LangId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public IFormFile ImgFile { get; set; }
    }
}
