using SubscriptionManagement.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.API.IntegrationTest
{
    [Binding]
    internal class Startup
    {
        [BeforeTestRun]
        public static void Setup()
        {
            SubscriptionManagementDataContext.Connectionstring = "Server=.;Database=SubscriptionManagementdb;User Id=admin;Password=sWRW8bdS3pyNwxA;TrustServerCertificate=True";
        }
    }
}
