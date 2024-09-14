using SubscriptionManagement.Application.Interfaces;
using SubscriptionManagement.Domain;
using SubscriptionManagement.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Repositories
{
    public class SubscriberRepo : BaseRepository<SubscriptionManagementDataContext, Subscriber>, ISubscriberRepo
    {
        public SubscriberRepo(SubscriptionManagementDataContext dbContext) : base(dbContext)
        {
        }

        public async Task<Subscriber> AddSubscriberAsync(Subscriber service)
        {
            return await AddAsync(service);
        }

        public async Task<IQueryable<Subscriber>> GetSubscribersAsync()
        {
            var res = await GetAllAsync();
            return res.AsQueryable();
        }

        public async Task<Subscriber> GetSubscribersAsync(string Username)
        {
            var res = await GetAllAsync();
            return await Task.FromResult(res.Distinct().FirstOrDefault(c => c.ServiceId.ToLower() == Username.ToLower()));
        }

        public async Task UpdateSubscriberAsync(Subscriber service)
        {
            await UpdateAsync(service);
        }
    }
}
