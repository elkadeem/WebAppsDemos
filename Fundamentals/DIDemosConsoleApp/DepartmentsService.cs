namespace DIDemosConsoleApp
{
    public class DepartmentsService
    {
        IDepartmentRepository _departmentRespository;
        public DepartmentsService(IDepartmentRepository departmentRespository)
        {
            _departmentRespository = departmentRespository ?? throw new ArgumentNullException(nameof(departmentRespository));
        }

        public List<Department> GetDepartments()
        {
            return _departmentRespository.GetDepartments();
        }
    }
}
