using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class SaleDAO
    {
        public List<Sale> GetTodaySales()
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var today = DateTime.Today;
                    var tomorrow = today.AddDays(1);
                    var todaySales = context.Sale
                        .Where(s => s.SaleDate >= today && s.SaleDate < tomorrow)
                        .ToList();

                    return todaySales;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las ventas del día actual.", ex);
            }
        }

        public SalePOCO GetSaleById(int saleId)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var sale = (from s in context.Sale
                                where s.SaleId == saleId
                                select new SalePOCO
                                {
                                    SaleId = s.SaleId,
                                    SaleDate = s.SaleDate,
                                    AdditionalRequest = s.AdditionalRequest,
                                    TotalSale = s.TotalSale,
                                    CustomerName = s.CustomerName,
                                    StaffID = s.StaffID
                                }).FirstOrDefault();

                    if (sale == null)
                    {
                        throw new Exception($"No se encontró una venta con el ID {saleId}.");
                    }
                    return sale;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la información de la venta.", ex);
            }
        }

        public List<OrderPOCO> GetOrdersBySaleById(int saleId)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var result = (from b in context.Bill
                                  join p in context.Product on b.Product equals p.ProductId
                                  where b.Sale == saleId
                                  select new OrderPOCO
                                  {
                                      SaleId = b.Sale,
                                      ProductId = b.Product,
                                      ProductName = p.Name,
                                      Quantity = b.Quantity,
                                      UnitPrice = b.UnitPrice,
                                      Total = b.Total
                                  }).ToList();

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al recuperar los pedidos para la venta especificada.", ex);
            }
        }
    }

    
}
