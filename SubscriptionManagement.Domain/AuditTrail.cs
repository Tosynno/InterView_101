using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Domain
{
    public class AuditTrail
    {
        public Guid Id { get; set; }
        public string Action { get; set; }
        public string ClientName { get; set; }
        public string IPAddress { get; set; }
        public DateTime DateGenerated { get; set; } = DateTime.Now;
        public string Request { get; set; }
        public string Response { get; set; }
    }
}
