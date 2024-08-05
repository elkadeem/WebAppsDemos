using DIDemosConsoleApp;
using Moq;

namespace DIDemoPrject.Testing
{
    public class DepartmentsServiceUnitTest
    {
        [Fact]
        public void Initiate_Object_Wit_Null_Repository_Will_Throw_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new DepartmentsService(null));
        }


        [Fact]
        public void GetDepartments_Will_Return_NonEmpty_List()
        {
            // AAA
            // Arrange
            DepartmentsService departmentsService =
                new DepartmentsService(new FakeDepartmentRepistory());

            // Act
            var items = departmentsService.GetDepartments();
        
            // Assert
            Assert.NotEmpty(items);
        }

        [Fact]
        public void GetDepartments_WithMock_Will_Return_NonEmpty_List()
        {
            // AAA
            // Arrange
            Mock<IDepartmentRepository> mock = new Mock<IDepartmentRepository>();

            mock.Setup(c => c.GetDepartments()).Returns(new List<Department>() {
               new Department{ Id = 1, Name = "Department" }
            });

            DepartmentsService departmentsService =
                new DepartmentsService(mock.Object);

            // Act
            var items = departmentsService.GetDepartments();

            // Assert            
            Assert.NotEmpty(items);
        }

    }

    public class FakeDepartmentRepistory : IDepartmentRepository
    {
        public List<Department> GetDepartments()
        {
            return new List<Department>() { 
             new Department{ Id = 1, Name = "Department" }
            };
        }
    }
}