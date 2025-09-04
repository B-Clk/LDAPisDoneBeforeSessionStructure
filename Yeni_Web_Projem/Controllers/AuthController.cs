namespace Yeni_Web_Projem.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Yeni_Web_Projem.Models;
    using System.Threading.Tasks;
    using Yeni_Web_Projem.Services;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly LDAPAuthenticationService _ldapService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(
            LDAPAuthenticationService ldapService,
            ILogger<AuthController> logger)
        {
            _ldapService = ldapService;
            _logger = logger;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { message = "E-posta ve şifre gereklidir." });
                }

                bool isAuthenticated = await _ldapService.AuthenticateViaLDAPAPIAsync(
                    request.Email, request.Password);


                if (isAuthenticated)
                {
                    _logger.LogInformation("Kullanıcı başarıyla giriş yaptı: {Email}", request.Email);

                    return Ok(new
                    {
                        success = true,
                        message = "Giriş başarılı",
                        user = new { email = request.Email }
                    });
                }
                else
                {
                    _logger.LogWarning("Başarısız giriş denemesi: {Email}", request.Email);

                    return Unauthorized(new
                    {
                        success = false,
                        message = "Geçersiz e-posta veya şifre"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Giriş işlemi sırasında hata oluştu: {Email}", request.Email);

                return StatusCode(500, new
                {
                    success = false,
                    message = "Sunucu hatası oluştu"
                });
            }
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }



}