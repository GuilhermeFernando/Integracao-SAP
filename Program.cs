Console.WriteLine("===========================================");
Console.WriteLine("       ROBO DE INTEGRACAO SAP B1");
Console.WriteLine("===========================================");

Console.WriteLine("Iniciando aplicacao...");

string pastaXML = Path.Combine(Directory.GetCurrentDirectory(),"XML");

if(Directory.Exists(pastaXML))
{
    Console.WriteLine("A pasta XML foi encontrada");
}
else
{
    Console.WriteLine("A pasta XML nao foi encontrada");
}




Console.ReadKey();