
try
{
    Console.WriteLine("===========================================");
    Console.WriteLine("       ROBO DE INTEGRACAO SAP B1");
    Console.WriteLine("===========================================");

    Console.WriteLine("Iniciando aplicacao...");

    XmlConfiguration configuracao = new XmlConfiguration();
    XmlConfiguration settings = configuracao.GetRoboSettings();


    SapConfiguration sapConfiguration = configuracao.GetSapConfiguration();

    using SapServices sapServices = new(sapConfiguration);

    await sapServices.LoginAsync();
    Console.WriteLine("Login SAP realizado com sucesso");
}
catch (Exception ex)
{
    Console.WriteLine("Ocorreu um erro d urante a execução");
    Console.WriteLine(ex.ToString());
}
finally
{
    Console.WriteLine();
    Console.WriteLine("Pressione qualquer tecla para fechar...");
    Console.ReadKey();

}