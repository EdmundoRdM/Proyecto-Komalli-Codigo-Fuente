using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                        .Where(s => s.SaleDate >= today && s.SaleDate < tomorrow && s.SaleStatus == 1)
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
                                    StaffID = s.StaffID,
                                    SaleStatus = s.SaleStatus
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
                                  where b.Sale == saleId && p.FromKitchen == true
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

        public int CreateTempSale()
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    var tempSale = new Sale
                    {
                        SaleDate = DateTime.Now,
                        TotalSale = 0,
                        CustomerName = "Temporal",
                        StaffID = 1,
                        SaleStatus = 1
                    };

                    context.Sale.Add(tempSale);
                    context.SaveChanges();
                    return tempSale.SaleId;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al crear la venta temporal.", ex);
                }
            }
        }

        public void FinalizeSale(int tempSaleId, string customerName, string customerRequest, decimal totalSale, List<ProductPOCO> products)
        {
            if (products == null || !products.Any())
            {
                throw new InvalidOperationException("La lista de productos está vacía. No se puede procesar la venta.");
            }

            using (var context = new KomalliDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var sale = context.Sale.FirstOrDefault(s => s.SaleId == tempSaleId);
                        if (sale == null)
                        {
                            throw new InvalidOperationException("No se encontró la venta temporal.");
                        }

                        sale.CustomerName = customerName;
                        sale.AdditionalRequest = customerRequest;
                        sale.TotalSale = totalSale;
                        sale.SaleDate = DateTime.Now;
                        sale.SaleStatus = 1; 

                        foreach (var product in products)
                        {
                            var dbProduct = context.Product.FirstOrDefault(p => p.ProductId == product.ProductId);
                            if (dbProduct == null || dbProduct.AvailableQuantity < product.Quantity)
                            {
                                throw new Exception($"El producto {product.ProductName} no tiene suficiente cantidad disponible.");
                            }

                            dbProduct.AvailableQuantity -= product.Quantity;

                            var bill = new Bill
                            {
                                Sale = sale.SaleId,
                                Product = product.ProductId,
                                Quantity = product.Quantity,
                                UnitPrice = product.ProductPrice,
                                Total = product.ProductPrice * product.Quantity
                            };

                            context.Bill.Add(bill);
                        }

                        context.SaveChanges();
                        transaction.Commit(); 
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception($"Error al finalizar la venta: {ex.Message}", ex);
                    }
                }
            }
        }

        public void UpdateSaleStatus(int saleId, int newStatus)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var sale = context.Sale.FirstOrDefault(s => s.SaleId == saleId);
                    if (sale == null)
                    {
                        throw new Exception($"No se encontro una venta con el ID {saleId}.");
                    }
                    sale.SaleStatus = newStatus;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado de la venta: " + ex.Message);
            }
        }

        public void DeleteTempSale(int tempSaleId)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var tempSale = context.Sale.FirstOrDefault(s => s.SaleId == tempSaleId);

                    if (tempSale != null)
                    {
                        context.Sale.Remove(tempSale);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar la venta temporal: {ex.Message}", ex);
            }
        }

    }


}
