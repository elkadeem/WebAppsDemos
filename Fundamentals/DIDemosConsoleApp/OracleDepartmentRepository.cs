using Oracle.ManagedDataAccess.Client;

namespace DIDemosConsoleApp
{
    public class OracleDepartmentRepository : IDepartmentRepository
    {
        OracleConnection _connection;

        public OracleDepartmentRepository(string connectionString)
        {
            _connection = new OracleConnection(connectionString);
        }

        public List<Department> GetDepartments()
        {
            throw new NotImplementedException();
        }
    }
}
