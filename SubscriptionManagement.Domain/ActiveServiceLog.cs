using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Domain
{
    public class ActiveServiceLog
    {
        public long Id { get; set; }
        public string? ServiceId { get; set; }
        public string? IPAddress { get; set; } = null!;
        public bool IsLogin { get; set; }
        public DateTime? ExpiryOn { get; set; }
        public DateTime? AddedOn { get; set; }
    }
}
