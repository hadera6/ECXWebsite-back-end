using ECX.Website.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECX.Website.Domain
{
    public class WareHouse : BaseDomainEntity
    {

        public string LangId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public string Capacity { get; set; }
        public string ImgName { get; set; }

    }
}
