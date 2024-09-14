using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SubscriptionManagement.Application.Models.Response;
using SubscriptionManagement.Domain;
using SubscriptionManagement.Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SubscriptionManagement.Application.Utilities
{
    public class JwtHandler
    {
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private readonly SubscriptionManagementDataContext _DbContext;
        private SecurityKey _issuerSigningKey { get; set; }
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtHeader _jwtHeader;
        private IHttpContextAccessor _accessor;

        public JwtHandler(IOptions<JwtOptions> options, IHttpContextAccessor accessor, SubscriptionManagementDataContext dbContext)
        {
            _options = options.Value;
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidIssuer = _options.Issuer,
                IssuerSigningKey = _issuerSigningKey
            };
            _accessor = accessor;
            _DbContext = dbContext;
        }

        public async Task<JsonWebTokenResponse> Create(Service user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Id", user.Id.ToString()),
                new Claim("exp",  DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes).ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.ServiceName),
               // new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
             }),
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                Issuer = _options.Issuer,
                Audience = _options.Issuer,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (_options.SecretKey)),
                SecurityAlgorithms.HmacSha256)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var stringToken = tokenHandler.WriteToken(token);
            TimeSpan currentDateTimeOffset;

            var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();

            ActiveServiceLog sa = new ActiveServiceLog();
            sa.ServiceId = user.Id.ToString();
            sa.IPAddress = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            sa.IsLogin = true;
            sa.AddedOn = DateTime.UtcNow;
            sa.ExpiryOn = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);
            _DbContext.ActiveServiceLogs.Add(sa);
            await _DbContext.SaveChangesAsync();
            return new JsonWebTokenResponse
            {
                Token = stringToken,
                Expires = (long)(new TimeSpan(DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes).Ticks - centuryBegin.Ticks).TotalSeconds),
                // LastActivity = currentDateTimeOffset.Humanize(),
                // LastActivity = currentDateTimeOffset,
            };
        }
        public IEnumerable<Claim> DecodeJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                foreach (var claim in jsonToken.Claims)
                {
                    Console.WriteLine($"{claim.Type}: {claim.Value}");
                }

                return jsonToken.Claims;
            }
            else
            {
                Console.WriteLine("Invalid token");
                return Enumerable.Empty<Claim>();
            }
        }
        public static long GetTokenExpirationTime(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var tokenExp = jwtSecurityToken.Claims.First(claim => claim.Type.Equals("exp")).Value;
            var ticks = long.Parse(tokenExp);
            return ticks;
        }

        public static bool CheckTokenIsValid(string token)
        {
            var tokenTicks = GetTokenExpirationTime(token);
            var tokenDate = DateTimeOffset.FromUnixTimeSeconds(tokenTicks).UtcDateTime;

            var now = DateTime.Now.ToUniversalTime();

            var valid = tokenDate >= now;

            return valid;
        }
        //public async Task<JsonWebTokenResponse> RefreshToken(string token)
        //{
        //    var GetActiveUser = _DbContext.ActiveUserLogs.Where(x => x.Token == token).FirstOrDefault();
        //    if (GetActiveUser != null)
        //    {
        //        var user = _DbContext.UserProfiles.Where(x => x.Id == Guid.Parse(GetActiveUser.UserId)).FirstOrDefault();
        //        if (user == null)
        //        {
        //            return null;
        //        }
        //        if (GetActiveUser.IsLogin == true)
        //        {
        //            GetActiveUser.IsLogin = false;
        //            _DbContext.ActiveUserLogs.Update(GetActiveUser);
        //            await _DbContext.SaveChangesAsync();
        //        }

        //        var tokenDescriptor = new SecurityTokenDescriptor
        //        {
        //            Subject = new ClaimsIdentity(new[]
        //            {
        //                new Claim("Id", Guid.NewGuid().ToString()),
        //                new Claim("exp",  DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes).ToString()),
        //                new Claim(JwtRegisteredClaimNames.Sub, user.FullName),
        //                new Claim(JwtRegisteredClaimNames.Email, user.Email),
        //                new Claim(JwtRegisteredClaimNames.Jti,
        //                Guid.NewGuid().ToString())
        //                }),
        //            Expires = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
        //            Issuer = _options.Issuer,
        //            Audience = _options.Issuer,
        //            SigningCredentials = new SigningCredentials
        //                (new SymmetricSecurityKey(Encoding.ASCII.GetBytes
        //            (_options.SecretKey)),
        //                SecurityAlgorithms.HmacSha512Signature)
        //        };
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var tokenx = tokenHandler.CreateToken(tokenDescriptor);
        //        var jwtToken = tokenHandler.WriteToken(tokenx);
        //        var stringToken = tokenHandler.WriteToken(tokenx);


        //        //ActiveUserLog sa = new ActiveUserLog();
        //        //sa.UserId = user.Id.ToString();
        //        //sa.Token = stringToken;
        //        //sa.IsLogin = true;
        //        //sa.ExpiryOn = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);
        //        //_DbContext.ActiveUserLogs.Add(sa);
        //        //await _DbContext.SaveChangesAsync();
        //        var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
        //        return new JsonWebTokenResponse
        //        {
        //            Token = stringToken,
        //            Expires = (long)(new TimeSpan(DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes).Ticks - centuryBegin.Ticks).TotalSeconds),
        //        };
        //    }

        //    return new JsonWebTokenResponse
        //    {
        //        Token = "",
        //        Expires = 0,
        //    };

        //}

        //public async Task<bool> SignOff(string token)
        //{
        //    var GetActiveUser = _DbContext.ActiveUserLogs.Where(x => x.Token == token && x.IsLogin == true).FirstOrDefault();
        //    if (GetActiveUser != null)
        //    {
        //        GetActiveUser.IsLogin = false;
        //        _DbContext.ActiveUserLogs.Update(GetActiveUser);
        //        await _DbContext.SaveChangesAsync();
        //        return true;
        //    }
        //    return false;
        //}
    }
}
