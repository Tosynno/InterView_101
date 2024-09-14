using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Models.Request
{
    public class SubscribeRequest
    {
        public string ServiceId { get; set; }
        public string PhoneNumber { get; set; }
        public string CountryCode { get; set; }
    }
}
