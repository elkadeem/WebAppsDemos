using System.ComponentModel.DataAnnotations;

namespace MVCWebAppDemo.Models
{
    public class DepartmentDto
    {
        public short DepartmentId { get; set; }

        [Display(Name = "Department Name")]
        public string Name { get; set; } = null!;


        [Display(Name = "Group Name")]
        public string GroupName { get; set; } = null!;

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }
    }
}
