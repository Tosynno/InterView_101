using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Models.Response;
using SubscriptionManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Interfaces
{
    public interface IService
    {
        Task<ApiResponseBase<object>> Login(LoginRequest request);
        Task<ApiResponseBase<object>> EnableOrDisableService(string ServiceId);
      
    }
}
