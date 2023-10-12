using ECX.Website.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.DTOs.Language
{
    public class LanguageFormDto : BaseDtos
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
    }
}
