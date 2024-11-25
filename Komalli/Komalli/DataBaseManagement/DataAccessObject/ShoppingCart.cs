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
    internal class ShoppingCart
    {
        SaleDAO saleDAO = new SaleDAO();
        private static ShoppingCart _instance;
        private readonly List<ProductPOCO> _products;
        public int TempSaleId;
        public decimal TotalSale;

        public static ShoppingCart Instance => _instance ?? (_instance = new ShoppingCart());

        private ShoppingCart()
        {
            _products = new List<ProductPOCO>();
        }

        public void AddProduct(ProductPOCO product, int quantityToAdd)
        {
            if (product != null)
            {
                var existingProduct = _products.FirstOrDefault(p => p.ProductId == product.ProductId);

                if (existingProduct != null)
                {
                    existingProduct.Quantity += quantityToAdd;
                }
                else
                {
                    product.Quantity = quantityToAdd;
                    _products.Add(product);
                }
                CalculateTotal();
            }
        }

        public void ReduceProductQuantity(int productId)
        {
            try
            {
                var productToReduce = _products.FirstOrDefault(p => p.ProductId == productId);

                if (productToReduce != null)
                {
                    if (productToReduce.Quantity > 1)
                    {
                        productToReduce.Quantity -= 1;
                        CalculateTotal();
                    }
                    else
                    {
                        throw new InvalidOperationException("La cantidad a reducir es mayor o igual a la cantidad actual del producto.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Producto no encontrado en el carrito.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al reducir la cantidad del producto: {ex.Message}", ex);
            }
        }

        public void RemoveProduct(int productId)
        {
            try
            {
                var productToRemove = _products.FirstOrDefault(p => p.ProductId == productId);

                if (productToRemove != null)
                {
                    _products.Remove(productToRemove);
                    CalculateTotal();
                }
                else
                {
                    throw new InvalidOperationException("Producto no encontrado en el carrito.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar completamente el producto: {ex.Message}", ex);
            }
        }


        public List<ProductPOCO> GetProducts()
        {
            return _products;
        }

        public bool HasProducts()
        {
            return _products.Any();
        }

        public void ClearCart()
        {
            _products.Clear();
            TempSaleId = 0; 
        }

        public void CreateTempSale()
        {
            if (TempSaleId == 0) 
            {
                TempSaleId = saleDAO.CreateTempSale(); 
            }
        }

        public void FinalizeSale(string CustomerName, String CustomerRequest)
        {
            if (TempSaleId == 0)
            {
                throw new InvalidOperationException("No se ha creado una venta temporal.");
            }

            if (!HasProducts())
            {
                throw new InvalidOperationException("El carrito está vacío. No se puede procesar la venta.");
            }

            try
            {
                saleDAO.FinalizeSale(TempSaleId,CustomerName, CustomerRequest, TotalSale, _products);
                ClearCart();
                CreateTempSale();
                MessageBox.Show("La venta se registró con éxito.", "Venta Completada", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al finalizar la venta: {ex.Message}", ex);
            }
        }

        public decimal CalculateTotal()
        {
            TotalSale = _products.Sum(p => p.ProductPrice * p.Quantity);
            return TotalSale;
        }

    }
}
