using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

public class SapServices : IDisposable
{
    private readonly SapConfiguration _sapConfiguration;
    private readonly HttpClient _httpClient;

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
        var cookieContainer = new CookieContainer();

        var handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true,
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var httpClient = new HttpClient(handler);

        // Simula os cabeçalhos utilizados pelo Postman.
        httpClient.DefaultRequestHeaders.Accept.Clear();       

        return httpClient;
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
            throw new InvalidOperationException(
                $"Falha no login do SAP. HTTP {(int)response.StatusCode}: {responseBody}");
        }       
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}