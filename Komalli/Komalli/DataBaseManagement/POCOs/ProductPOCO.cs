using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.POCOs
{
    public class ProductPOCO
    {
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductAvailableQuantity { get; set; }
        public String ProductDescription { get; set; }
        public String ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public String ProductType { get; set; }
        public Boolean ProductStatus { get; set; }
        public DateTime ProductSellingDate { get; set; }
        public Boolean ProductFromKitchen { get; set; }

    }
}
