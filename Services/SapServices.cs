using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public class SapServices : IDisposable
{
    private readonly SapConfiguration _sapConfiguration;
    private readonly HttpClient _httpClient;
    private readonly CookieContainer _cookieContainer = new CookieContainer();

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = null
    };

    public SapServices(SapConfiguration sapConfiguration)
    {
        _sapConfiguration = sapConfiguration;
        _httpClient = CriarHttpClient();
    }


    private HttpClient CriarHttpClient()
    {

        var handler = new HttpClientHandler
        {
            CookieContainer = _cookieContainer,
            UseCookies = true,
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var httpClient = new HttpClient(handler);

        // Simula os cabeçalhos utilizados pelo Postman.
        httpClient.DefaultRequestHeaders.Accept.Clear();       

        return httpClient;
    }

    private bool ValidarSessao()
    {
        string url = _sapConfiguration.ServiceLayerUrl.TrimEnd('/');

        Uri uri = new Uri(url);

        CookieCollection cookies = _cookieContainer.GetCookies(uri);

        Cookie? sessao = cookies["B1SESSION"];

        if (sessao == null)
        {
            return false;
        }

        if (sessao.Expired)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> ValidarSessaoAsync()
    {
        if (!ValidarSessao())
        {
            return false;
        }

        string url = $"{_sapConfiguration.ServiceLayerUrl.TrimEnd('/')}/b1s/v1/Users";

        using var response = await _httpClient.GetAsync(url);

        return response.IsSuccessStatusCode;
    }


    public async Task GarantirSessaoAsync()
    {
        bool sessaoValida = await ValidarSessaoAsync();

        if (!sessaoValida)
        {
            await LoginAsync();
        }
    }

    public async Task LoginAsync()
    {
        string loginUrl = $"{_sapConfiguration.ServiceLayerUrl.TrimEnd('/')}/b1s/v1/Login";

        var loginData = new
        {
            CompanyDB = _sapConfiguration.CompanyDB,
            UserName = _sapConfiguration.UserName,
            Password = _sapConfiguration.Password
        };

        using var response = await _httpClient.PostAsJsonAsync(
            loginUrl,
            loginData,
            JsonOptions
            );

        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Falha no login do SAP. HTTP {(int)response.StatusCode}: {responseBody}");
        }       
    }


    public async Task<HttpResponseMessage> RenovarSessao(Func<Task<HttpResponseMessage>> requisicao)
    {
        var response = await requisicao();
        
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
        }
        response.Dispose();

        Console.WriteLine("Sessão expirada. Renovando sessão no SAP...");

        await LoginAsync();

        response = await requisicao();

        return response;
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}