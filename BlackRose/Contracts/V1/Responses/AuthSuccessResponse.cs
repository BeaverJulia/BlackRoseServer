﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string UserName { get; set; }
    }
}
