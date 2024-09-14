using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Models.Response
{
    public class JsonWebTokenResponse
    {
        public string Token { get; set; }
        public long Expires { get; set; }
        public TimeSpan LastActivity { get; set; }
    }
}
