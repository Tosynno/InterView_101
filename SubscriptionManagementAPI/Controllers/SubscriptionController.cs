using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManagement.Application.Interfaces;
using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Models.Response;
using SubscriptionManagement.Domain;

namespace SubscriptionManagement.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController(ISubscriber subscriber) : ControllerBase
    {
        protected ISubscriber _subscriber = subscriber;
        [HttpPost("Subscribe")]
        public async Task<ActionResult<ApiResponseNoData>> Subscribe(SubscribeRequest request)
        {
            string[] auth = HttpContext.Request.Headers["Authorization"].ToString().Split(':');
            var res = await _subscriber.Subscribe(auth[0], request);
           
            if (res.ResponseCode == "99")
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }

        [HttpPost("Unsubscribe")]
        public async Task<ActionResult<ApiResponseNoData>> Unsubscribe(SubscribeRequest request)
        {
            string[] auth = HttpContext.Request.Headers["Authorization"].ToString().Split(':');
            var res = await _subscriber.Unsubscribe(auth[0], request);

            if (res.ResponseCode == "99")
            {
                return BadRequest(res);
            }
            else if (res.ResponseCode == "01")
            {
                return NotFound(res);
            }
            else
            {
                return Ok(res);
            }
        }

        [HttpPost("CheckStatus")]
        public async Task<ActionResult<ApiResponseNoData>> CheckStatus(SubscribeRequest request)
        {
            string[] auth = HttpContext.Request.Headers["Authorization"].ToString().Split(':');
            var res = await _subscriber.CheckStatus(auth[0], request);

            if (res.ResponseCode == "99")
            {
                return BadRequest(res);
            }
            else if (res.ResponseCode == "01")
            {
                return NotFound(res);
            }
            else
            {
                return Ok(res);
            }
        }
    }
}
