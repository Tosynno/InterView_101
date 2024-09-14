using SubscriptionManagement.Domain;
using SubscriptionManagement.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Interfaces
{
    public interface IServiceRepo : IRepository<SubscriptionManagementDataContext, Service>
    {
        Task<IQueryable<Service>> GetServicesAsync();
        Task<Service> GetServicesAsync(string Username);
        Task UpdateServiceAsync(Service service);
        Task<Service> AddServiceAsync(Service service);
    }
}
