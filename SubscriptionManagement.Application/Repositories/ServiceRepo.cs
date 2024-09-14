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
    public class ServiceRepo : BaseRepository<SubscriptionManagementDataContext, Service>, IServiceRepo
    {
        public ServiceRepo(SubscriptionManagementDataContext dbContext) : base(dbContext)
        {
        }

        public async Task<Service> AddServiceAsync(Service service)
        {
            return await AddAsync(service);
        }

        public async Task<IQueryable<Service>> GetServicesAsync()
        {
            var res = await GetAllAsync();
            return res.AsQueryable();
        }

        public async Task<Service> GetServicesAsync(string Username)
        {
            var res = await GetAllAsync();
            return await Task.FromResult(res.Distinct().FirstOrDefault(c => c.ServiceUsername.ToLower() == Username.ToLower()));
        }

        public async Task UpdateServiceAsync(Service service)
        {
            await UpdateAsync(service);
        }
    }
}
