using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class SpoilageDAO
    {
        public List<SpoilagePOCO> GetAllSpoilages()
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var spoilages = context.Spoilage
                        .OrderByDescending(s => s.Date)
                        .Select(s => new SpoilagePOCO
                        {
                            SpoilageId = s.SpoilageId,
                            SpoilageConcept = s.SpoilageConcept,
                            Date = s.Date,
                            StaffName = s.Staff.FirstName + " " + s.Staff.LastName,
                            Total = s.SpoilageDetail.Sum(sd => sd.Total), // Calcula el total de los detalles
                            Details = s.SpoilageDetail.Select(sd => new SpoilageDetailPOCO
                            {
                                SpoilageId = sd.SpoilageId,
                                ProductId = sd.ProductId,
                                ProductName = sd.Product.Name,
                                Quantity = sd.Quantity,
                                UnitPrice = sd.UnitPrice, // Asegúrate de que este campo está correctamente mapeado
                                Total = sd.Quantity * sd.UnitPrice // Calcula el total para este producto
                            }).ToList()
                        }).ToList();

                    return spoilages;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar las mermas: " + ex.Message, ex);
            }
        }


        public void UpdateSpoilageDetail(SpoilageDetailPOCO detail)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var spoilageDetail = context.SpoilageDetail
                        .FirstOrDefault(sd => sd.SpoilageId == detail.SpoilageId && sd.ProductId == detail.ProductId);

                    if (spoilageDetail != null)
                    {
                        spoilageDetail.Quantity = detail.Quantity;
                        spoilageDetail.Total = detail.Quantity * detail.UnitPrice; // Asegura que el cálculo del total sea correcto
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception($"Detalle de merma no encontrado. SpoilageId: {detail.SpoilageId}, ProductId: {detail.ProductId}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el detalle de la merma: " + ex.Message, ex);
            }
        }


        public void UpdateProductInventory(int productId, int quantityChange)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var product = context.Product.FirstOrDefault(p => p.ProductId == productId);

                    if (product != null)
                    {
                        product.AvailableQuantity += quantityChange;
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new Exception("Producto no encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el inventario del producto: " + ex.Message, ex);
            }
        }

        public int GetProductAvailableQuantity(int productId)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var product = context.Product.FirstOrDefault(p => p.ProductId == productId);
                    return product?.AvailableQuantity ?? 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la cantidad disponible del producto: " + ex.Message, ex);
            }
        }
    }
}
