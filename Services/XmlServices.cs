using System.Xml.Linq;

namespace Robo_InserirXMLSAP.Services
{
    public class XmlServices
    {
        public XDocument CarregarXml(string caminhoArquivo)
        {
            if (!File.Exists(caminhoArquivo))
            {
                throw new FileNotFoundException($"Arquivo não encontrado no caminho: {caminhoArquivo}");
            }
            return XDocument.Load(caminhoArquivo);
        }
    }
}
