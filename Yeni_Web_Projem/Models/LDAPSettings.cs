namespace Yeni_Web_Projem.Models
{
    public class LDAPSettings
    {
        public string ApiUrl { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
        public int RetryCount { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 2;
        public bool EnableLogging { get; set; } = true;
        public bool MaskPasswordInLogs { get; set; } = true;
    }
}