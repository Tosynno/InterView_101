using Azure;
using Microsoft.EntityFrameworkCore;
using SubscriptionManagement.Application.Interfaces;
using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Models.Response;
using SubscriptionManagement.Application.Repositories;
using SubscriptionManagement.Application.Utilities;
using SubscriptionManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Services
{
    public class SubscriberService(ISubscriberRepo subscriberRepo, JwtHandler jwtHandler, IServiceRepo repo) : ISubscriber
    {
        private ISubscriberRepo _subscriberRepo = subscriberRepo;
        private IServiceRepo _repo = repo;
        private JwtHandler _jwtHandler = jwtHandler;

        public async Task<ApiResponseBase<object>> CheckStatus(string Token, SubscribeRequest request)
        {
            var response = new ApiResponseBase<object>();
            var serviceid = _jwtHandler.DecodeJwtToken(Token).FirstOrDefault(c => c.Type == "Id")?.Value;
            var res = await _repo.GetServicesAsync(serviceid);

            if (res.ServiceUsername != request.ServiceId)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Invalid ServiceId Provided";
                return response;
            }
            var sub = await _subscriberRepo.GetSubscribersAsync();
            var result = await sub.FirstOrDefaultAsync(c => c.ServiceId == res.ServiceUsername && c.PhoneNumber == request.PhoneNumber);
            if (result == null)
            {
                response.ResponseCode = "01";
                response.ResponseDescription = "Subscribed not found";
                return response;
            }
            response.Data= result;
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }

        public async Task<ApiResponseBase<object>> Subscribe(string Token, SubscribeRequest request)
        {
            var response = new ApiResponseBase<object>();
            var serviceid = _jwtHandler.DecodeJwtToken(Token).FirstOrDefault(c => c.Type == "Id")?.Value;
            var res = await _repo.GetServicesAsync(serviceid);

            if (res.ServiceUsername != request.ServiceId)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Invalid ServiceId Provided";
                return response;
            }
            var sub = await _subscriberRepo.GetSubscribersAsync();
            var result = await sub.FirstOrDefaultAsync(c => c.ServiceId == res.ServiceUsername && c.PhoneNumber == request.PhoneNumber);
            if (result != null)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "User already subscribed";
                return response;
            }
            var subscription = new Subscriber
            {
                ServiceId = res.ServiceUsername,
                PhoneNumber = request.PhoneNumber,
                SubscriptionStatus = "Subscribed",
                SubscriptionDate = DateTime.UtcNow
            };

            await _subscriberRepo.AddSubscriberAsync(subscription);
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }

        public async Task<ApiResponseBase<object>> Unsubscribe(string Token, SubscribeRequest request)
        {
            var response = new ApiResponseBase<object>();
            var serviceid = _jwtHandler.DecodeJwtToken(Token).FirstOrDefault(c => c.Type == "Id")?.Value;
            var res = await _repo.GetServicesAsync(serviceid);

            if (res.ServiceUsername != request.ServiceId)
            {
                response.ResponseCode = "99";
                response.ResponseDescription = "Invalid ServiceId Provided";
                return response;
            }
            var sub = await _subscriberRepo.GetSubscribersAsync();
            var result = await sub.FirstOrDefaultAsync(c => c.ServiceId == res.ServiceUsername && c.PhoneNumber == request.PhoneNumber);
            if (result == null || result.SubscriptionStatus != "Subscribed")
            {
                response.ResponseCode = "01";
                response.ResponseDescription = "User is not subscribed";
                return response;
            }
            result.SubscriptionStatus = "Unsubscribed";
            result.UnsubscriptionDate = DateTime.UtcNow;
            await _subscriberRepo.UpdateSubscriberAsync(result);
            response.ResponseCode = "00";
            response.ResponseDescription = "SUCCESSFUL";
            return response;
        }
    }
}
