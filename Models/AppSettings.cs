namespace WebApplication1.Models
{
    public class AppSettings
    {
        public Dictionary<string, string> apikeyvalues { get; set; }
        public AppSettings(IConfiguration configuration)
        {
            try
            {
                apikeyvalues = configuration.GetSection("ApiKeyDictionary").Get<Dictionary<string, string>>();
            }
            catch { }
        }
    }
}
