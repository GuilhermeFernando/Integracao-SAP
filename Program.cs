
using Robo_InserirXMLSAP.Services;

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

    await sapServices.GarantirSessaoAsync();
    Console.WriteLine("Garantia de sessão concluída.");


    string caminhoXml = @"C:\Users\wevy_partner\Documents\Guilherme\35261048722915000218570010000123771133571705.xml";

    var xmlServices = new XmlServices();

    var documento = xmlServices.CarregarXml(caminhoXml);

    Console.WriteLine($"Elemento raiz: {documento.Root?.Name}");
    Console.WriteLine($"Namespace: {documento.Root?.GetDefaultNamespace()}");


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
