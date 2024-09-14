using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SubscriptionManagement.Application.Interfaces;
using SubscriptionManagement.Application.Models.Request;
using SubscriptionManagement.Application.Models.Response;

namespace SubscriptionManagement.API.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        protected IService _service;
        public AuthenticationController(IService service)
        {
            _service = service;
        }

        [HttpPost("account/login")]
        public async Task<ActionResult<ApiResponseNoData>> Login(LoginRequest request)
        {
            var res = await _service.Login(request);
            if (res.ResponseCode == "01") {
                return NotFound(res);
            }
            else if(res.ResponseCode == "02") {
                return Unauthorized(res);
            }
            else if (res.ResponseCode == "03")
            {
                return StatusCode(StatusCodes.Status403Forbidden, res);
            }
            else if (res.ResponseCode == "99")
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }

        [HttpPost("account/enableOrDisableService")]
        public async Task<ActionResult<ApiResponseNoData>> EnableOrDisableService()
        {
            string[] auth = HttpContext.Request.Headers["Authorization"].ToString().Split(':');
            var res = await _service.EnableOrDisableService(auth[0]);
            if (res.ResponseCode == "01")
            {
                return NotFound(res);
            }
            else if (res.ResponseCode == "02")
            {
                return Unauthorized(res);
            }
            else if (res.ResponseCode == "03")
            {
                return StatusCode(StatusCodes.Status403Forbidden, res);
            }
            else if (res.ResponseCode == "99")
            {
                return BadRequest(res);
            }
            else
            {
                return Ok(res);
            }
        }
    }
}
