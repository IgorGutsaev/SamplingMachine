using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
using MPT.Vending.Domains.SharedContext;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace API.Controllers
{
    public class IdentityController : ControllerBase
    {
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(3);

        public IdentityController(IIdentityService identityService, IConfiguration configuration) {
            _identityService = identityService;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInRequest request) {
            User user = _identityService.Get(x => x.Email == request.Email.ToLower() && x.Password == request.Password.Trim()).FirstOrDefault()!;

            if (user == null || (!user.Admin && !user.Claims.Any()))
                return Unauthorized();

            var tokenGenRequest = new TokenGenerationRequest {
                Email = user.Email,
                UserId = user.UID.ToString(),
                CustomClaims = new Dictionary<string, object>()
            };

            if (user.Admin)
                tokenGenRequest.CustomClaims.Add("admin", user.Admin);

            if (user.Claims.Any(x=> string.Equals(x, "kiosk", StringComparison.InvariantCultureIgnoreCase)))
                tokenGenRequest.CustomClaims.Add("kiosk", new MailAddress(user.Email).User);

            return Ok(GenerateToken(tokenGenRequest));
        }

        [AllowAnonymous]
        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignInRequest request) {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool exist = _identityService.Get(x => x.Email == request.Email.ToLower()).Any();

            if (exist)
                return Conflict();

            _identityService.Create(request.Email, request.Password);

            return SignIn(request);
        }

        [HttpGet("user")]
        public IActionResult GetUser() {
            JwtSecurityToken token = FetchToken();
            if (token == null)
                return Unauthorized();

            string userId = token.Claims.FirstOrDefault(x => x.Type == "userid")?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized();

            return Ok(_identityService.Get(x=>x.UID == Guid.Parse(userId)).FirstOrDefault());
        }

        public string GenerateToken(TokenGenerationRequest request) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(AzureKeyVaultReader.GetSecret("ogmento-jwt-key"));
            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, request.Email),
                new(JwtRegisteredClaimNames.Email, request.Email),
                new("userid", request.UserId.ToString()),
            };

            foreach (var claimPair in request.CustomClaims) {
                string valueType;

                if (claimPair.Value is bool)
                    valueType = ClaimValueTypes.Boolean;
                else if (claimPair.Value is double || claimPair.Value is decimal)
                    valueType = ClaimValueTypes.Double;
                else valueType = ClaimValueTypes.String;

                if (!string.IsNullOrWhiteSpace(valueType)) {
                    var claim = new Claim(claimPair.Key, claimPair.Value.ToString()!, valueType);
                    claims.Add(claim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        private JwtSecurityToken FetchToken() {
            JwtSecurityToken token = null;

            if (Request.Headers.Keys.Contains("Authorization")) {
                StringValues values;

                if (Request.Headers.TryGetValue("Authorization", out values)) {
                    var jwt = values.ToString();

                    if (jwt.Contains("Bearer")) {
                        jwt = jwt.Replace("Bearer", "").Trim();
                    }

                    var handler = new JwtSecurityTokenHandler();

                    token = handler.ReadJwtToken(jwt);
                }
            }

            return token;
        }

        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
    }
}
