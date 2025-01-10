using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    public class ProductDAO
    {
        public int RegisterProduct(ProductPOCO product)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el empleado.", ex);
            }
        }

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

        public List<ProductPOCO> GetProductsByType(int productTypeId)
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    return context.Product
                        .Where(p => p.Type == productTypeId && p.AvailableQuantity > 0 && p.Status)
                        .Select(p => new ProductPOCO
                        {
                            ProductId = p.ProductId,
                            ProductPrice = p.Price,
                            ProductAvailableQuantity = p.AvailableQuantity,
                            ProductDescription = p.Description,
                            ProductName = p.Name,
                            ProductTypeId = p.Type,
                            ProductType = p.ProductType.TypeName,
                            ProductStatus = p.Status,
                            ProductSellingDate = p.SellingDate ?? DateTime.MinValue,
                            ProductFromKitchen = p.FromKitchen
                        }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener productos por tipo.", ex);
                }
            }
        }

        public List<ProductPOCO> GetMealsForToday(DateTime todayDate)
        {
            List<ProductPOCO> meals = new List<ProductPOCO>();

            try
            {
                using (var context = new KomalliDBEntities())
                {
                    int breakfastTypeId = 1; // Asegúrate de que estos IDs sean correctos
                    int lunchTypeId = 2;

                    var query = context.Product
                        .Where(p => (p.Type == breakfastTypeId || p.Type == lunchTypeId) &&
                                    DbFunctions.TruncateTime(p.SellingDate) == DbFunctions.TruncateTime(todayDate) &&
                                    p.Status == true &&
                                    p.AvailableQuantity > 0)
                        .Select(p => new ProductPOCO
                        {
                            ProductId = p.ProductId,
                            ProductPrice = p.Price,
                            ProductAvailableQuantity = p.AvailableQuantity,
                            ProductDescription = p.Description,
                            ProductName = p.Name,
                            ProductTypeId = p.Type,
                            ProductType = p.ProductType.TypeName,
                            ProductStatus = p.Status,
                            ProductSellingDate = p.SellingDate ?? DateTime.MinValue,
                            ProductFromKitchen = p.FromKitchen
                        });

                    meals = query.ToList();
                }
            }
            catch (EntityException ex)
            {
                throw new Exception("Error de conexión con la base de datos.", ex);
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception("Operación no válida durante la ejecución de la consulta.", ex);
            }
            catch (TimeoutException ex)
            {
                throw new Exception("La consulta excedió el tiempo de espera.", ex);
            }

            return meals;
        }


        public ProductPOCO GetProductById(int productId)
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    var product = context.Product
                        .Where(p => p.ProductId == productId)
                        .Select(p => new ProductPOCO
                        {
                            ProductId = p.ProductId,
                            ProductPrice = p.Price,
                            ProductAvailableQuantity = p.AvailableQuantity,
                            ProductDescription = p.Description,
                            ProductName = p.Name,
                            ProductTypeId = p.Type,
                            ProductType = p.ProductType.TypeName,
                            ProductStatus = p.Status,
                            ProductSellingDate = p.SellingDate ?? DateTime.MinValue,
                            ProductFromKitchen = p.FromKitchen
                        }).FirstOrDefault();

                    return product ?? throw new Exception("Producto no encontrado.");
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener el producto por ID.", ex);
                }
            }
        }

        public List<ProductPOCO> GetAllProducts()
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    return context.Product
                        .Where(p => p.Status == true)
                        .Select(p => new ProductPOCO
                        {
                            ProductId = p.ProductId,
                            ProductPrice = p.Price,
                            ProductAvailableQuantity = p.AvailableQuantity,
                            ProductDescription = p.Description,
                            ProductName = p.Name,
                            ProductTypeId = p.Type,
                            ProductType = p.ProductType.TypeName,
                            ProductStatus = p.Status,
                            ProductSellingDate = p.SellingDate ?? DateTime.MinValue,
                            ProductFromKitchen = p.FromKitchen
                        }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener todos los productos.", ex);
                }
            }
        }

        public bool UpdateProduct(int productId, ProductPOCO updatedProduct)
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    var product = context.Product.FirstOrDefault(p => p.ProductId == productId);
                    if (product == null)
                        throw new Exception("Producto no encontrado.");

                    product.Price = updatedProduct.ProductPrice;
                    product.AvailableQuantity = updatedProduct.ProductAvailableQuantity;
                    product.Description = updatedProduct.ProductDescription;
                    product.Name = updatedProduct.ProductName;
                    product.Type = updatedProduct.ProductTypeId;
                    product.Status = updatedProduct.ProductStatus;
                    product.SellingDate = updatedProduct.ProductSellingDate;
                    product.FromKitchen = updatedProduct.ProductFromKitchen;

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al modificar el producto.", ex);
                }
            }
        }

        public List<ProductPOCO> GetProductsBySaleId(int saleId)
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    var products = (from bill in context.Bill
                                    join product in context.Product on bill.Product equals product.ProductId
                                    where bill.Sale == saleId
                                    select new ProductPOCO
                                    {
                                        ProductId = product.ProductId,
                                        ProductPrice = product.Price,
                                        ProductAvailableQuantity = product.AvailableQuantity,
                                        ProductDescription = product.Description,
                                        ProductName = product.Name,
                                        ProductTypeId = product.Type,
                                        ProductType = product.ProductType.TypeName,
                                        ProductStatus = product.Status,
                                        ProductSellingDate = product.SellingDate ?? DateTime.MinValue,
                                        ProductFromKitchen = product.FromKitchen,
                                        Quantity = bill.Quantity,
                                        TotalInDB = bill.UnitPrice * bill.Quantity
                                    }).ToList();

                    return products;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener los productos asociados a la venta.", ex);
                }
            }
        }


        public List<ProductPOCO> GetProductsByIds(List<int> productIds)
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    return context.Product
                        .Where(p => productIds.Contains(p.ProductId))
                        .Select(p => new ProductPOCO
                        {
                            ProductId = p.ProductId,
                            ProductPrice = p.Price,
                            ProductAvailableQuantity = p.AvailableQuantity,
                            ProductDescription = p.Description,
                            ProductName = p.Name,
                            ProductTypeId = p.Type,
                            ProductType = p.ProductType.TypeName,
                            ProductStatus = p.Status,
                            ProductSellingDate = p.SellingDate ?? DateTime.MinValue,
                            ProductFromKitchen = p.FromKitchen
                        }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener productos por IDs.", ex);
                }
            }
        }

        public bool ReduceProductQuantity(int productId, int quantityToReduce)
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    var product = context.Product.FirstOrDefault(p => p.ProductId == productId);
                    if (product == null)
                    {
                        throw new Exception("Producto no encontrado.");
                    }

                    if (quantityToReduce > product.AvailableQuantity)
                    {
                        throw new Exception("La cantidad a reducir es mayor que la cantidad disponible.");
                    }

                    product.AvailableQuantity -= quantityToReduce;

                    context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al reducir la cantidad del producto.", ex);
                }
            }
        }

        public static List<ProductPOCO> GetProductsForReport()
        {
            using (var context = new KomalliDBEntities())
            {
                try
                {
                    return context.Product
                        .Where(p => p.Status == true)
                        .Select(p => new ProductPOCO
                        {
                            ProductId = p.ProductId,
                            ProductPrice = p.Price,
                            ProductAvailableQuantity = p.AvailableQuantity,
                            ProductName = p.Name,
                            ProductTypeId = p.Type,
                        }).ToList();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener todos los productos.", ex);
                }
            }
        }

    }
}
