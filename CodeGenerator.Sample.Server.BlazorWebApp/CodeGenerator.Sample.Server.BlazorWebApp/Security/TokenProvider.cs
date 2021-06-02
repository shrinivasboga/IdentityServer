namespace CodeGenerator.Sample.Server.BlazorWebApp.Security
{
    public class TokenProvider
    {
        public string XsrfToken { get; set; }
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}