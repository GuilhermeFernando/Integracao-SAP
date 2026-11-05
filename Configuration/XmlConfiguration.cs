using Microsoft.Extensions.Configuration;
using Robo_InserirXMLSAP.Configuration;
public class XmlConfiguration
{
    private readonly IConfiguration _configuration;
    public string SourceDirectory {get;set;} = string.Empty;
    public string DestinationDirectorySuccess {get;set;} = string.Empty;
    public string DestinationDirectoryError {get;set;} = string.Empty;


    public XmlConfiguration()
    {
        _configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json",optional: false, reloadOnChange: false)
        .Build();
    }

    public XmlConfiguration GetRoboSettings()
    {
        return _configuration 
        .GetSection("Robo")
        .Get<XmlConfiguration>()
        ?? throw new InvalidOperationException ("A configuracao do Robo nao foi encontrada");
    }

    public SapConfiguration GetSapConfiguration()
    {
        return _configuration
        .GetSection("SapConnection")
        .Get<SapConfiguration>()
        ?? throw new InvalidOperationException("A configuracao da SL nao foi encontrada");
    }


    public HanaConfiguration GetHanaConfiguration()
    {
        return _configuration
            .GetSection("HanaConnection")
            .Get<HanaConfiguration>()
            ?? throw new InvalidOperationException("A configuração do HANA nao foi encontrada");
    }
    
}