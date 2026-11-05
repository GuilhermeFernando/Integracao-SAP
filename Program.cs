
using Robo_InserirXMLSAP.Configuration;
using Robo_InserirXMLSAP.Models;
using Robo_InserirXMLSAP.Repository;
using Robo_InserirXMLSAP.Services;
using System.Xml.Linq;


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
    XmlServices xmlServices = new();
    
    HanaConfiguration hanaConfiguration = configuracao.GetHanaConfiguration();
    HanaServices hanaServices = new
        (
         hanaConfiguration.Server,
         hanaConfiguration.Database,
         hanaConfiguration.DBUser,
         hanaConfiguration.DBPassword
        );

    HanaRepository hanaRepository = new(hanaServices);

    CteServices cteServices = new(hanaRepository);


    List<string> arquivoXml = xmlServices.LocalizarArquivoXml(settings.SourceDirectory);

    Console.WriteLine($"Arquivo encontrado: {arquivoXml.Count}");

    foreach (string arquivo in arquivoXml)
    {
        Console.WriteLine($"Processando: {Path.GetFileName(arquivo)}");

        XDocument documento = xmlServices.CarregarXml(arquivo);
        CteModel cte = xmlServices.CteDados(documento);

        Console.WriteLine($"CNPJ do emitente: {cte.CNPJ}");

        string? cardCode = await cteServices.BuscarCardCodeCNPJ(cte);

        if(string.IsNullOrWhiteSpace(cardCode) )
        {
            Console.WriteLine("Parceiro não encontrado no SAP");
        }
        else
        {
            Console.WriteLine($"CardCode encontrado: {cardCode}");
        }
    }


}
catch (Exception ex)
{
    Console.WriteLine("Ocorreu um erro durante a execução");
    Console.WriteLine(ex.ToString());
}
finally
{
    Console.WriteLine();
    Console.WriteLine("Pressione qualquer tecla para fechar...");
    Console.ReadKey();

}
