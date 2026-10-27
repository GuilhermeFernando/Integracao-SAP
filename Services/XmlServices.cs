using System.IO;
using System.Xml.Linq;
using Robo_InserirXMLSAP.Models;

namespace Robo_InserirXMLSAP.Services
{
    public class XmlServices
    {
       public CteModel CteDados(XDocument documento)
        {
            XNamespace ns = "http://www.portalfiscal.inf.br/cte";

            string? cnpj = documento
                .Root?
                .Element(ns + "CTe")?
                .Element(ns + "infCte")?
                .Element(ns + "emit")?
                .Element(ns + "CNPJ")?
                .Value;

            if (string.IsNullOrWhiteSpace(cnpj))
            {
                throw new InvalidDataException("CNPJ do emitente não encontrado no CT-e.");
            }

            return new CteModel
            {
                CNPJ = cnpj
            };
        }
    }
}
