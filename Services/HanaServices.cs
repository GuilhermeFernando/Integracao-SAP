using Sap.Data.Hana;

namespace Robo_InserirXMLSAP.Services
{
    public class HanaServices
    {
        private readonly string _connectionString;
        public HanaServices(string server, string database, string user, string password)
        {
            _connectionString = $"Server={server};"
                   + $"CurrentSchema={database};"
                   + $"UserID={user};"
                   + $"Password={password};";
                        
        }

        public HanaConnection CriarConexao()
        {
            return new HanaConnection(_connectionString);
        }
    }
}
