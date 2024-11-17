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


    }
}
