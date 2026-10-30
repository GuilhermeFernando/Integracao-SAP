using Robo_InserirXMLSAP.Services;
using Sap.Data.Hana;

namespace Robo_InserirXMLSAP.Repository
{
    public class HanaRepository
    {
        private readonly HanaServices _hanaServices;

        public HanaRepository(HanaServices hanaServices)
        {
            _hanaServices = hanaServices;            
        }

        public async Task<List<Dictionary<string,object?>> ExecutarConsultaAsync(string sql, Dictionary<string, object?>? parametros=null)
        {
            var resultado = new List<Dictionary<string, object?>>();
            using HanaConnection conexao = _hanaServices.CriarConexao();
            await conexao.OpenAsync();

            using HanaCommand command = new(sql, conexao);

            if (parametros != null)
            {
                foreach(var parametro in parametros)
                {
                    command.Parameters.AddWithValue(parametro.Key, parametro.Value ?? DBNull.Value);
                }
            }

            using HanaDataReader reader = (HanaDataReader)await command.ExecuteReaderAsync();
            
            while (await reader.ReadAsync())
            {
                var registro = new Dictionary<string, object?>();

                for(int i = 0; i< reader.FieldCount; i++)
                {
                    registro[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                resultado.Add(registro);
            }
            return resultado;


        }
    }
}
