Console.WriteLine("===========================================");
Console.WriteLine("       ROBO DE INTEGRACAO SAP B1");
Console.WriteLine("===========================================");

Console.WriteLine("Iniciando aplicacao...");

XmlConfiguration configuracao = new XmlConfiguration();

XmlConfiguration settings = configuracao.GetRoboSettings();

Console.WriteLine($"Origem: {settings.SourceDirectory}");
Console.WriteLine($"Sucesso: {settings.DestinationDirectorySuccess}");
Console.WriteLine($"Erro: {settings.DestinationDirectoryError}");