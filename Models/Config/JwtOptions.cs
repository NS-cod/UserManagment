using System.ComponentModel.DataAnnotations;

namespace API.Config
{
    public class JwtOptions
    {
        [Required] public string SecretKey { get; set; } = string.Empty;
        [Required] public string Issuer { get; set; } = string.Empty;
        [Required] public string Audience { get; set; } = string.Empty;
        [Range(1, int.MaxValue)] public int ExpirationMinutes { get; set; }
    }
}
