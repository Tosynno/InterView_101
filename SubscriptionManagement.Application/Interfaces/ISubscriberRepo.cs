using SubscriptionManagement.Domain;
using SubscriptionManagement.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Interfaces
{
    public interface ISubscriberRepo : IRepository<SubscriptionManagementDataContext, Subscriber>
    {
        Task<IQueryable<Subscriber>> GetSubscribersAsync();
        Task<Subscriber> GetSubscribersAsync(string Username);
        Task UpdateSubscriberAsync(Subscriber service);
        Task<Subscriber> AddSubscriberAsync(Subscriber service);
    }
}
