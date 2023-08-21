﻿using ECX.Website.Application.DTOs.WareHouse;
using ECX.Website.Application.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECX.Website.Application.CQRS.WareHouse_.Request.Command
{
    public class CreateWareHouseCommand : IRequest<BaseCommonResponse>
    {
        public WareHouseFormDto WareHouseFormDto { get; set; }
    }
}
