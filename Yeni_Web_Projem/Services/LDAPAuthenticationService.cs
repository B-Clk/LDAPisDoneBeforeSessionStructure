using System.Net;
using Yeni_Web_Projem.Controllers;

namespace Yeni_Web_Projem.Services
{
    public class LDAPAuthenticationService
    {
        private readonly ILogger<AuthController> _logger;
        public async Task<bool> AuthenticateViaLDAPAPIAsync(string email, string password)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Kullanıcı adını e-posta'dan ayıkla
                    string username = email.Contains("@") ? email.Split('@')[0] : email;
                    _logger.LogInformation("LDAP API doğrulama başlatılıyor - Username: {Username}, Email: {Email}", username, email);

                    // LDAP API endpoint
                    string apiUrl = "https://api.sinop.edu.tr:7000/AuthAPI/auth/dualauth";

                    // JSON request body oluştur
                    string jsonRequest = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";

                    _logger.LogInformation("LDAP API İsteği Gönderiliyor - URL: {ApiUrl} - Request: {Request}",
                        apiUrl, jsonRequest.Replace(password, "*"));

                    using (var client = new TimedWebClient(30000))
                    {
                        client.Headers[HttpRequestHeader.ContentType] = "application/json";

                        // API'ye POST isteği gönder
                        string response = client.UploadString(apiUrl, jsonRequest);

                        _logger.LogInformation("LDAP API Yanıt Alındı - Response: {Response}", response);

                        // JSON response'u parse et
                        if (response.Contains("\"isSuccess\":true"))
                        {
                            _logger.LogInformation("LDAP API Doğrulama Başarılı - Kullanıcı: {Username}", username);
                            return true;
                        }
                        else
                        {
                            _logger.LogWarning("LDAP API Doğrulama Başarısız - Kullanıcı: {Username} - Response: {Response}",
                                username, response);
                            return false;
                        }
                    }
                }
                catch (WebException webEx)
                {
                    // Web bağlantı hatası
                    _logger.LogError("LDAP API Web Hatası - Kullanıcı: {Username} - Hata: {Message} - Status: {Status}",
                        email, webEx.Message, webEx.Status);
                    return false;
                }
                catch (System.Net.Sockets.SocketException socketEx)
                {
                    // Socket bağlantı hatası
                    _logger.LogError("LDAP API Socket Hatası - Kullanıcı: {Username} - Hata: {Message} - ErrorCode: {ErrorCode}",
                        email, socketEx.Message, socketEx.ErrorCode);
                    return false;
                }
                catch (Exception ex)
                {
                    // Genel hata
                    _logger.LogError("LDAP API Genel Hatası - Kullanıcı: {Username} - Hata: {Message} - StackTrace: {StackTrace}",
                        email, ex.Message, ex.StackTrace);
                    return false;
                }
            });
        }

        private class TimedWebClient : WebClient
        {
            private readonly int _timeoutMs;
            public TimedWebClient(int timeoutMs)
            {
                _timeoutMs = timeoutMs;
            }
            protected override WebRequest GetWebRequest(Uri address)
            {
                var request = base.GetWebRequest(address);
                if (request != null)
                {
                    request.Timeout = _timeoutMs;
                    var http = request as HttpWebRequest;
                    if (http != null)
                    {
                        http.ReadWriteTimeout = _timeoutMs;
                        http.ContinueTimeout = _timeoutMs;
                    }
                }
                return request;
            }
        }
    }
}