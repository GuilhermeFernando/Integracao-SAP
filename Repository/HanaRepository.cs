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

        public async Task<List<Dictionary<string, object?>>> ExecutarConsultaAsync(string nomeArquivoSql, params object?[] parametros)
        {
           string caminhoSql = Path.Combine(AppContext.BaseDirectory,"Queries", nomeArquivoSql);

           if (!File.Exists(caminhoSql))
            {
                throw new FileNotFoundException($"Arquivo SQL não encontrado: {caminhoSql}");
            }

           string sql = await File.ReadAllTextAsync(caminhoSql);

           var resultado = new List<Dictionary<string, object?>>();

           using HanaConnection conexao = _hanaServices.CriarConexao();
           await conexao.OpenAsync();

           using HanaCommand comando = new(sql, conexao);

           foreach(var parametro in parametros)
            {
                comando.Parameters.AddWithValue(string.Empty, parametro ?? DBNull.Value);
            }
           using HanaDataReader leitor = (HanaDataReader)await comando.ExecuteReaderAsync();

            while (await leitor.ReadAsync())
            {
                var registro = new Dictionary<string, object?>();
                for(int i = 0; i < leitor.FieldCount; i++)
                {
                    registro[leitor.GetName(i)] = leitor.IsDBNull(i) ? null : leitor.GetValue(i);
                }
                resultado.Add(registro);
            }
            return resultado;
        }
    }
}
