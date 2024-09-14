using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Domain
{
    public class Service
    {
        public long Id { get; set; }
        public string ServiceUsername { get; set; }
        public string ServiceName { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        public DateTime CreatedDate { get; set; }

       
    }
}
