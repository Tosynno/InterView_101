using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Domain
{
    public class Subscriber
    {
        public long Id { get; set; }
        public string ServiceId { get; set; }
        public string PhoneNumber { get; set; }
        public string SubscriptionStatus { get; set; }
        public DateTime SubscriptionDate { get; set; }
        public DateTime UnsubscriptionDate { get; set; }

       
    }
}
