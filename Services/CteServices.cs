
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

        public async Task<string?> BuscarCardCodeCNPJ(string cnpj)
        {
            var resultado = await _hanaRepository.ExecutarConsultaAsync("BuscarCardCodeCNPJ.sql", cnpj);

            if(resultado.Count == 0)
            {
                return null;
            }

            return resultado[0]["CardCode"]?.ToString();
        }
    }
}
