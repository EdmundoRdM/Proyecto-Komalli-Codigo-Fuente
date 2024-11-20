using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class ProductTypeDAO
    {
        public List<ProductTypePOCO> GetAllTypes()
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var typeList = context.ProductType.ToList();
                    var productTypePOCOList = typeList.Select(type => new ProductTypePOCO
                    {
                        ProductTypeId = type.ProductTypeId,
                        ProductTypeName =type.TypeName
                    }).ToList();

                    return productTypePOCOList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el producto.", ex);
            }
        }
    }
}
