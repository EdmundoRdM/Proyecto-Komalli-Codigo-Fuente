using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.POCOs
{
    internal class SalePOCO
    {
        public int SaleId { get; set; }
        public DateTime SaleDate { get; set; }
        public string AdditionalRequest { get; set; }
        public decimal TotalSale { get; set; }
        public string CustomerName { get; set; }
        public int StaffID { get; set; }
    }
}
