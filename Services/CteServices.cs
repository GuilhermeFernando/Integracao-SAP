
using Robo_InserirXMLSAP.Models;
using Robo_InserirXMLSAP.Repository;

namespace Robo_InserirXMLSAP.Services
{
    public class CteServices
    {
        private readonly HanaRepository _hanaRepository;

        public CteServices(HanaRepository hanaRepository)
        {
            _hanaRepository = hanaRepository;
        }

        public async Task<string?> BuscarCardCodeCNPJ(CteModel cte)
        {
            var resultado = await _hanaRepository.ExecutarConsultaAsync("BuscarCardCodeCNPJ.sql", cte.CNPJ);

            if(resultado.Count == 0)
            {
                return null;
            }

            return resultado[0]["CardCode"]?.ToString();
        }
    }
}
