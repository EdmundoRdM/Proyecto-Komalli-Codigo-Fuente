using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.POCOs
{
    internal class StaffPOCO
    {
        public int StaffId { get; set; }
        public string EmployeeNumber { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int Role { get; set; }
        public bool Status { get; set; }
    }
}
