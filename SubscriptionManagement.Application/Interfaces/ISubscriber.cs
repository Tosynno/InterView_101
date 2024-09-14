using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Interfaces
{
    public interface ISubscriber
    {
        Task<ApiResponseBase<object>> Subscribe(string Token, SubscribeRequest request);
        Task<ApiResponseBase<object>> Unsubscribe(string Token, SubscribeRequest request);
        Task<ApiResponseBase<object>> CheckStatus(string Token, SubscribeRequest request);
    }
}
