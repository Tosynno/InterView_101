using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Models.Response
{
    public interface IApiResponse
    {
        bool Successful { get; }
    }
}
