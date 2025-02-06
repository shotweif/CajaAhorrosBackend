using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CajaAhorrosBackend.Services
{
    public class TokenService(byte[] key)
    {
        private readonly byte[] _key = key;

        public string GenerateToken(string userId, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = "CajaAhorrosBackend",
                Audience = "CajaAhorrosBackend",
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        // public void ValidateToken(string token)
        // {
        //     var validationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true,
        //         ValidIssuer = "CajaAhorrosBackend",
        //         ValidAudience = "CajaAhorrosBackend",
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ClaveSuper$ecreta123456789_ABCDEFGHIJKLMN"))
        //     };

        //     var tokenHandler = new JwtSecurityTokenHandler();
        //     SecurityToken validatedToken;
        //     var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

        //     Console.WriteLine("Token validado correctamente.");
        // }
    }


}
