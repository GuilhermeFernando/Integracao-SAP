public class BuscarXML
{
    public string BuscarDiretorioOrigem()
    {
        Console.WriteLine("Digite o diretório para buscar XML:");
        string diretorio = Console.ReadLine();
        
        if(string.IsNullOrWhiteSpace(diretorio))
        {
            throw new ArgumentException("o diretoio não foi encontrado");
        }

        if(!Directory.Exists(diretorio))
        {
            throw new DirectoryNotFoundException($"o diretoio {diretorio} não encontrado");
        }
        return diretorio;
    }
}