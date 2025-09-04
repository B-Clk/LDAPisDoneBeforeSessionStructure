using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;
using Yeni_Web_Projem.Models;

namespace Yeni_Web_Projem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            // Eðer kullanýcý giriþ yapmýþsa SorumluMailler sayfasýna yönlendir
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserToken")))
            {
                return RedirectToAction("SorumluMailler");
            }

            // Giriþ yapmamýþsa Login sayfasýna yönlendir
            return RedirectToAction("Login");
        }


        public IActionResult Login()
        {
            // Eðer kullanýcý zaten giriþ yapmýþsa doðrudan SorumluMailler sayfasýna yönlendir
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserToken")))
            {
                return RedirectToAction("SorumluMailler");
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var authRequest = new
                {
                    email = model.Email,
                    password = model.Password
                };

                var json = JsonSerializer.Serialize(authRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");


                // HttpClient oluþtur (SSL doðrulamasýný devre dýþý býrak)
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
                };



                /*
                // SSL doðrulamasýný devre dýþý býrak  Eski hali üstte
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                */


                using var client = new HttpClient(handler);
                // client.Timeout = TimeSpan.FromSeconds(30); // dün eklendi

                // AuthController'a istek gönder
                var response = await client.PostAsync("https://api.sinop.edu.tr:7000/AuthAPI/auth/dualauth", content);

                

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseContent,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (authResponse != null && authResponse.isSuccess)
                    {
                        // Baþarýlý giriþ - session oluþtur
                        HttpContext.Session.SetString("UserToken", "authenticated");
                        HttpContext.Session.SetString("UserEmail", model.Email);

                        return RedirectToAction("SorumluMailler");
                    }
                    else
                    {
                        // JSON deserialization başarısız oldu
                        // Örnek: JSON formatı AuthResponse sınıfıyla uyuşmuyor
                        Console.WriteLine($"Response Content: {responseContent}");
                        Console.WriteLine($"AuthResponse is null: {authResponse == null}");

                        // API başarılı yanıt verdi ama işlem başarısız
                        // Örnek: { "success": false, "message": "Invalid credentials" }
                        Console.WriteLine($"AuthResponse.Success: {authResponse?.isSuccess}");


                    }

                        // Önce response durumunu kontrol edin
                        Console.WriteLine($"Status Code: {(int)response.StatusCode} - {response.StatusCode}");
                    Console.WriteLine($"IsSuccessStatusCode: {response.IsSuccessStatusCode}");

                }else{

                    Console.WriteLine($"Status Code: {(int)response.StatusCode} - {response.StatusCode}");
                    Console.WriteLine($"IsSuccessStatusCode: {response.IsSuccessStatusCode}");
                    // Hata içeriğini okuyun
                    var errorContent = await response.Content.ReadAsStringAsync();

                    try
                    {
                        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent);
                        ModelState.AddModelError(string.Empty, errorResponse?.Message ?? "Bir hata oluştu");
                    }
                    catch
                    {
                        // JSON parse edilemezse direkt içeriği göster
                        ModelState.AddModelError(string.Empty, $"Sunucu hatası: {errorContent}");
                    }

                    Console.WriteLine($"Error Response: {errorContent}");

                    // Headers'ları kontrol edin
                    foreach (var header in response.Headers)
                    {
                        Console.WriteLine($"{header.Key}: {string.Join(",", header.Value)}");
                    }
                }


                ModelState.AddModelError(string.Empty, "Geçersiz email veya şifre");

                ModelState.AddApiError("Kimlik doğrulama başarısız");

                return View(model);
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Ağ hatası: {ex.Message}");
                return View(model);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Login işlemi sırasında hata oluştu");
                ModelState.AddModelError(string.Empty, "Giriþ işlemi sýrasýnda bir hata oluþtu. Lütfen daha sonra tekrar deneyin.");
                return View(model);
            }
        }



        public IActionResult MailTalepSayfasi()
        {
            return View();
        }
        public IActionResult MailTalepFormu()
        {

            return View();
        }


        public IActionResult SorumluMailler()
        {

            // Kullanýcýnýn giriþ yapýp yapmadýðýný kontrol et
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UserToken")))
            {
                return RedirectToAction("Login");
            }

            // Burada kullanýcýnýn sorumlu olduðu mailleri getirebilirsiniz
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




    }
    public static class ModelStateExtensions
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, Dictionary<string, string> errors)
        {
            foreach (var error in errors)
            {
                modelState.AddModelError(error.Key, error.Value);
            }
        }

        public static void AddApiError(this ModelStateDictionary modelState, string errorMessage)
        {
            modelState.AddModelError(string.Empty, $"API Hatası: {errorMessage}");
        }
    }
}