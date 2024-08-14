using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace ShiftsRazorClassLibrary.Areas.Shifts.Pages
{
    public class IndexModel : PageModel
    {

        public List<Shift> Shifts { get; set; } = new List<Shift>();

        public async Task<IActionResult> OnGetAsync()
        {
            SqlConnection connection = new SqlConnection("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=AdventureWorks2019;Integrated Security=True");
            var sqlCommand = new SqlCommand(@"SELECT [ShiftID]
      ,[Name]
      ,[StartTime]
      ,[EndTime]
      ,[ModifiedDate]
  FROM [AdventureWorks2019].[HumanResources].[Shift]", connection);
            await connection.OpenAsync();
            var reader  = await sqlCommand.ExecuteReaderAsync();
            
            while(await reader.ReadAsync())
            {
                Shifts.Add(new Shift(short.Parse(reader["ShiftID"].ToString())
                    , reader["Name"].ToString()
                    , TimeOnly.Parse(reader["StartTime"].ToString())
                    , TimeOnly.Parse(reader["EndTime"].ToString())));
            }

            return Page();
        }
    }

    public record Shift(short ShiftID,  string ShiftName, TimeOnly StartTime, TimeOnly EndTime);
}


