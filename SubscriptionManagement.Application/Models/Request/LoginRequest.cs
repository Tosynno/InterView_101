﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Models.Request
{
    public class LoginRequest
    {
        public string ServiceId { get; set; }
        public string Password { get; set; }
    }
}
