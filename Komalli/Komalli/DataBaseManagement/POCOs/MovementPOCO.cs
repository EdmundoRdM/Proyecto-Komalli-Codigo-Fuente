using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.POCOs
{
    internal class MovementPOCO
    {
            public int MovementId { get; set; }
            public decimal Amount { get; set; } 
            public string Description { get; set; } 
            public DateTime Date { get; set; } 
            public int MovementType { get; set; }
            public String MovementTypeName { get; set; }
            public int StaffID { get; set; }
    }
}
