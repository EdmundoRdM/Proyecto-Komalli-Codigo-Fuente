using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class ProductDAO
    {
        public List<ProductPOCO> GetProductByName(string partialName)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var productList = (from product in context.Product
                                    where product.Status == true &&
                                           product.Name.ToLower().Contains(partialName.ToLower())
                                     orderby product.Name descending
                                     select new ProductPOCO
                                     {
                                         ProductId = product.ProductId,
                                         ProductAvailableQuantity = product.AvailableQuantity,
                                         ProductPrice = product.Price,
                                         ProductDescription = product.Description,
                                         ProductName = product.Name,
                                         ProductFromKitchen = product.FromKitchen,
                                         ProductSellingDate = (DateTime)product.SellingDate,
                                         ProductStatus = product.Status,
                                         ProductTypeId = product.Type
                                     })
                             .ToList();
                    return productList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el personal por nombre parcial.", ex);
            }
        }
    }
}
