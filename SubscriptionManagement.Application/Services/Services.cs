using Azure;
using Microsoft.EntityFrameworkCore;
using SubscriptionManagement.Application.Interfaces;
using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Models.Response;
using SubscriptionManagement.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Services
{
    public class Services : IService
    {
        private IServiceRepo _repo;
        private JwtHandler _jwtHandler;

        public Services(IServiceRepo repo, JwtHandler jwtHandler)
        {
            _repo = repo;
            _jwtHandler = jwtHandler;
        }

        public async Task<ApiResponseBase<object>> EnableOrDisableService(string Token)
        {
            var response = new ApiResponseBase<object>();
            var serviceid = _jwtHandler.DecodeJwtToken(Token).FirstOrDefault(c => c.Type == "Id")?.Value;
           var res = await _repo.GetServicesAsync(serviceid);
            if (res == null)
            {
                response.ResponseCode = "01";
                response.ResponseDescription = "Services not found";
                return response;
            }
            if (res.IsActive) { 
                res.IsActive = false;
                await _repo.UpdateServiceAsync(res);
                response.ResponseCode = "00";
                response.ResponseDescription = "Services has been deactivated";
                return response;
            }
            res.IsActive = true;
            await _repo.UpdateServiceAsync(res);
            response.ResponseCode = "00";
            response.ResponseDescription = "Services has been activated";
            return response;
        }

        public async Task<ApiResponseBase<object>> Login(LoginRequest request)
        {
            var respone = new ApiResponseBase<object>();
            var service = await _repo.GetServicesAsync(request.ServiceId);
            if (service == null)
            {
                respone.ResponseCode = "01";
                respone.ResponseDescription = "Services not found";
                return respone;
            }

            //note: password must be encrypted
            var res = await _repo.GetServicesAsync();
            var result = await res.FirstOrDefaultAsync(c => c.Id == service.Id && c.Password == request.Password);
            if (service == null)
            {
                respone.ResponseCode = "02";
                respone.ResponseDescription = "Invalid credentials";
                return respone;
            }
            if (service.IsActive)
            {
                var token = await _jwtHandler.Create(service);
                if (token == null)
                {
                    respone.ResponseCode = "99";
                    respone.ResponseDescription = "Something when wrong at this point, please contact administrator";
                    return respone;
                }
                respone.ResponseCode = "00";
                respone.ResponseDescription = "SUCCESSFUL";
                return respone;
            }
            respone.ResponseCode = "03";
            respone.ResponseDescription = "Service not Active";
            return respone;
        }
    }
}
