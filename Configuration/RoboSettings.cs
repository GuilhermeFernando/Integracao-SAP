using Microsoft.Extensions.Configuration;
public class RoboSettings
{
    private readonly IConfiguration _configuration;
    public string SourceDirectory = string.Empty;
    public string DestinationDirectorySucess = string.Empty;
    public string DestinationDirectoryError = string.Empty;


    public RoboSettings()
    {
        _configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json",optional: false, reloadOnChange: false)
        .Build();
    }

    public RoboSettings GetRoboSettings()
    {
        return _configuration 
        .GetSection("Robo")
        .Get<RoboSettings>()
        ?? throw new InvalidOperationException ("A configuracao do Robo nao foi encontrada");
    }

    
}