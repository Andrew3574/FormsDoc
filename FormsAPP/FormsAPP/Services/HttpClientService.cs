using System.Net;
using System.Text;

namespace FormsAPP.Services
{
    public class HttpClientService
    {
        private HttpClient? _httpClient;
        private CookieContainer? _cookieContainer;
        private readonly string _baseUrl;
        public HttpClientService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _baseUrl = configuration["FormsAPI:apiUrl"]!;
            InitiateHttpClient();
            SetJwtCookie(httpContextAccessor);
        }
        public HttpClient? GetClient()
        {
            return _httpClient;
        }

        private void SetJwtCookie(IHttpContextAccessor httpContextAccessor)
        {
            var request = httpContextAccessor.HttpContext!.Request;
            if (request != null && request.Cookies.TryGetValue("jwt", out var token))
                {
                    var cookie = new Cookie("jwt", token)
                    {
                        Path = "/",
                        HttpOnly = true
                    };
                    _cookieContainer!.Add(new Uri(_baseUrl), cookie);
                }
        }

        private void InitiateHttpClient()
        {
            _cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer,
                UseCookies = true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }
    }
}
