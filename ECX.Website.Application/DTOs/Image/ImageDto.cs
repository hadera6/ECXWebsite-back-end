using ECX.Website.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Image
{
    public class ImageDto : BaseDtos
    {
        public string LangId { get; set; }
        public string Title { get; set; }
        public string Caption { get; set; }
        public bool   IsCarousel { get; set; }
        public string ImgName { get; set; }
    }
}
