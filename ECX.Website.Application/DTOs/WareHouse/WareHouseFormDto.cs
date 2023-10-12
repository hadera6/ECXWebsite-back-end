using ECX.Website.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.WareHouse
{
    public class WareHouseFormDto : BaseDtos
    {
        
        public string LangId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Capacity { get; set; }
        public IFormFile ImgFile{get;set;}

    }
}
