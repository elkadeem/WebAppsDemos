using Microsoft.Data.SqlClient;

namespace DIDemosConsoleApp
{
    public class DepartmentRespository : IDepartmentRepository
    {
        SqlConnection _connection;
        public DepartmentRespository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public List<Department> GetDepartments()
        {
            throw new NotImplementedException();
        }
    }
}
