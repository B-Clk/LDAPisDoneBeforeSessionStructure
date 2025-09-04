namespace Yeni_Web_Projem.Models
{
    public class LDAPConfig
    {
        public string Server { get; set; } = string.Empty;
        public int Port { get; set; } = 389;
        public string BindDN { get; set; } = string.Empty;
        public string BindPassword { get; set; } = string.Empty;
        public string SearchBase { get; set; } = string.Empty;
    }
}