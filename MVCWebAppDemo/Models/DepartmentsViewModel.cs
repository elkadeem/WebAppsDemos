using X.PagedList;

namespace MVCWebAppDemo.Models
{
    public class DepartmentsViewModel
    {
        public string Keyword {  get; set; }

        public int CurrentPage { get; set; }

        public IPagedList<DepartmentDto> Departments { get; set; }
    }
}
