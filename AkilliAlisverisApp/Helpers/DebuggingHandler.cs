using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AkilliAlisverisApp.Helpers
{
    public class DebuggingHandler : DelegatingHandler
    {
        public DebuggingHandler() : this(new HttpClientHandler())
        {
        }

        public DebuggingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Debug.WriteLine($"--> Giden İstek: {request.Method} {request.RequestUri}");
            if (request.Headers.Authorization != null)
            {
                Debug.WriteLine($"--> Authorization Header: {request.Headers.Authorization.Scheme} {request.Headers.Authorization.Parameter}");
            }
            else
            {
                Debug.WriteLine("--> Authorization Header: YOK!");
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                // EĞER İSTEK BAŞARISIZ OLURSA, CEVABIN İÇERİĞİNİ LOGLA
                Debug.WriteLine($"--> API HATA KODU: {response.StatusCode}");
                string errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"--> API CEVAP İÇERİĞİ: {errorContent}");
            }

            return response;
        }
    }
}