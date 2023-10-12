using ECX.Website.Application.DTOs.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.ContractFile
{
    public class ContractFileDto : BaseDtos
    {
        public string LangId { get; set; }
        public string CommodityId { get; set;}
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
    }
}
