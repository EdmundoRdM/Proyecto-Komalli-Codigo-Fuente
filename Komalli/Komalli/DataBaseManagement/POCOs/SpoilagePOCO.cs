using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.POCOs
{
    public class SpoilagePOCO
    {
        public int SpoilageId { get; set; }
        public string SpoilageConcept { get; set; }
        public DateTime Date { get; set; }
        public string StaffName { get; set; }
        public decimal Total { get; set; }
        public List<SpoilageDetailPOCO> Details { get; set; } = new List<SpoilageDetailPOCO>();
    }

    public class SpoilageDetailPOCO
    {
        public int SpoilageId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total { get; set; }
    }
}
