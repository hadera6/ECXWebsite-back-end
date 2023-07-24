using ECX.Website.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Commodity
{
    public class CommodityDto : BaseDtos
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Img { get; set; }
    }
}
