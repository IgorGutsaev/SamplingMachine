using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MPT.Vending.API.Dto;
using MPT.Vending.Domains.Identity.Abstractions;
using System.IdentityModel.Tokens.Jwt;
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

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] SignInRequest request) {
            User user = _identityService.Get(x => x.Email == request.Email.ToLower() && x.Password == request.Password.Trim()).FirstOrDefault()!;

            if (user == null)
                return Unauthorized();

            return Ok(GenerateToken(new TokenGenerationRequest {
                Email = user.Email,
                UserId = user.UID.ToString(),
                CustomClaims = new Dictionary<string, object> {
                    { "admin", user.Admin }
                }
            }));
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

        private readonly IIdentityService _identityService;
        private readonly IConfiguration _configuration;
    }
}
