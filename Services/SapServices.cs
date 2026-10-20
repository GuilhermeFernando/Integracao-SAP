using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
public class SapServices : IDisposable
{
    private readonly SapConfiguration _sapConfiguration;
    private readonly HttpClient _httpClient;

    private HttpClient CriarHttpClient()
    {
        var cookieContainer = new CookieContainer();
        const string certificadoEsperado = "0F5BFD9B56C861DC9060E63A788F37956EB8A08A9E98DCDC3185B75FAD7C57CF";
        var handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true,

            ServerCertificateCustomValidationCallback =
        (_, certificate, _, _) =>
            certificate is not null &&
            string.Equals(
                certificate.GetCertHashString(
                    System.Security.Cryptography.HashAlgorithmName.SHA256),
                certificadoEsperado,
                StringComparison.OrdinalIgnoreCase)
        };

        return new HttpClient(handler);
    }

    public SapServices(SapConfiguration sapConfiguration)
    {
        _sapConfiguration = sapConfiguration;
        _httpClient = CriarHttpClient();
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

        string jsonEnviado = System.Text.Json.JsonSerializer.Serialize(loginData);

        Console.WriteLine($"URL: {loginUrl}");
        Console.WriteLine($"JSON enviado: {jsonEnviado}");

        using var response = await _httpClient.PostAsJsonAsync(loginUrl, loginData);    

        string respondeBody = await response.Content.ReadAsStringAsync();   


        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Falha no login do SAP. HTTP {(int)response.StatusCode}: {respondeBody}");
        }

    }
    public void Dispose()
    {
        _httpClient.Dispose();
    }

}