using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Komalli.GUIs
{
    /// <summary>
    /// Lógica de interacción para CashierLanding.xaml
    /// </summary>
    public partial class CashierLanding : Page
    {
        ProductDAO productDAO = new ProductDAO();
        public List<ProductPOCO> Products { get; set; }
        public int productTypeSelected = 1;
        private int TempSaleID; 
        public CashierLanding()
        {
            InitializeComponent();
            InitializeShoppingCart();
        }
        private void InitializeShoppingCart()
        {
            try
            {
                ShoppingCart.Instance.CreateTempSale();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al inicializar el carrito: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ShowMenuForm()
        {
            HideAllGrids();
            TodaysMealsGrid.Visibility = Visibility.Visible;
            List<ProductPOCO> todaysMeals = productDAO.GetMealsForToday(DateTime.Now);
            UpdateMenuLabels(todaysMeals, DateTime.Now);
        }


        public void UpdateMenuLabels(List<ProductPOCO> mealsForToday, DateTime todayDate)
        {
            try
            {
                if (mealsForToday == null || !mealsForToday.Any())
                {
                    throw new InvalidOperationException("No se encontraron productos para la fecha de hoy.");
                }

                lblMenuDate.Content = $"Menú del día {todayDate:dd/MM/yyyy}";

                var breakfast = mealsForToday.FirstOrDefault(p => p.ProductTypeId == 1);
                var lunch = mealsForToday.FirstOrDefault(p => p.ProductTypeId == 2);

                if (breakfast != null)
                {
                    lblBreakfast.Content = FormatDescription(breakfast.ProductDescription);
                }
                else
                {
                    lblBreakfast.Content = "No disponible.";
                }

                if (lunch != null)
                {
                    lblMeal.Content = FormatDescription(lunch.ProductDescription);
                }
                else
                {
                    lblMeal.Content = "No disponible.";
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Información no disponible", MessageBoxButton.OK, MessageBoxImage.Warning);

                lblMenuDate.Content = $"Menú del día {todayDate:dd/MM/yyyy}";
                lblBreakfast.Content = "No disponible.";
                lblMeal.Content = "No disponible.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string FormatDescription(string description)
        {
            return description.Replace(",", "\n");
        }

        private void btnGeneralMenu_Click(object sender, RoutedEventArgs e)
        {
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: De la carta";
            TodaysMealsGrid.Visibility = Visibility.Hidden;
            GeneralMenuGrid.Visibility = Visibility.Visible;
            productTypeSelected = 4;
            Products = productDAO.GetProductsByType(3);
            this.GeneralMenuList.ItemsSource = Products;
        }

        private void ShoppingCart_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ProductPOCO product)
            {
                try
                {
                    ShoppingCart.Instance.AddProduct(product, 1);
                    MessageBox.Show($"Producto agregado al carrito: {product.ProductName}", "Carrito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al agregar producto al carrito: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void InfoIcon_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ProductPOCO product)
            {
                HideAllGrids();
                ProductDetailsGrid.Visibility = Visibility.Visible;
                lblProductName.Content = $"{ product.ProductName}";
                txtBProductDescription.Text = $"{product.ProductDescription}";
                productTypeSelected = product.ProductTypeId;
            }
        }


        private void btnTodaysMenu_Click(object sender, RoutedEventArgs e)
        {
            HideAllGrids();
            TodaysMealsGrid.Visibility = Visibility.Visible;
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: Menu del Día";
            productTypeSelected = 1;
            Products = productDAO.GetProductsByType(1);
            this.GeneralMenuList.ItemsSource = Products;

        }
        private void btnDrinks_Click(object sender, RoutedEventArgs e)
        {
            HideAllGrids();
            GeneralMenuGrid.Visibility = Visibility.Visible;
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: Bebidas";
            productTypeSelected = 4;
            Products = productDAO.GetProductsByType(4);
            this.GeneralMenuList.ItemsSource = Products;

        }

        private void btnDesserts_Click(object sender, RoutedEventArgs e)
        {
            productTypeSelected = 0;
            HideAllGrids();
            GeneralMenuGrid.Visibility = Visibility.Visible;
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: Postres";
            productTypeSelected = 5;
            Products = productDAO.GetProductsByType(5);
            this.GeneralMenuList.ItemsSource = Products;
        }
        private void HideAllGrids()
        {
            MenuForm.Visibility = Visibility.Visible;
            DefaultBackground.Visibility = Visibility.Hidden;
            TodaysMealsGrid.Visibility = Visibility.Hidden;
            GeneralMenuGrid.Visibility = Visibility.Hidden;
            ProductDetailsGrid.Visibility = Visibility.Hidden;
            ShoppingCartGrid.Visibility = Visibility.Hidden;
        }

        private void GetBackToGrid()
        {
            if (productTypeSelected > 0)
            {

                switch (productTypeSelected)
                {
                    case 1:
                    case 2:
                        TodaysMealsGrid.Visibility = Visibility.Visible;
                        List<ProductPOCO> todaysMeals = productDAO.GetMealsForToday(DateTime.Now);
                        UpdateMenuLabels(todaysMeals, DateTime.Now);
                        break;

                    case 3:
                        GeneralMenuGrid.Visibility = Visibility.Visible;
                        lblGeneralMenu.Content = "Menú Cafetería FEIGE: De la carta";
                        Products = productDAO.GetProductsByType(3);
                        GeneralMenuList.ItemsSource = Products;
                        break;

                    case 4:
                        GeneralMenuGrid.Visibility = Visibility.Visible;
                        lblGeneralMenu.Content = "Menú Cafetería FEIGE: Bebidas";
                        Products = productDAO.GetProductsByType(4);
                        GeneralMenuList.ItemsSource = Products;
                        break;

                    case 5:
                        GeneralMenuGrid.Visibility = Visibility.Visible;
                        lblGeneralMenu.Content = "Menú Cafetería FEIGE: Postres";
                        Products = productDAO.GetProductsByType(5);
                        GeneralMenuList.ItemsSource = Products;
                        break;

                    default:
                        MessageBox.Show("Tipo de producto no reconocido.");
                        break;
                }
            }
            else
            {
                MessageBox.Show("No se pudo identificar el producto asociado al botón.");
            }
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            HideAllGrids();
            GetBackToGrid();
        }

        private void btnShoppingCart_Click(object sender, RoutedEventArgs e)
        {
            var productsInCart = ShoppingCart.Instance.GetProducts();

            if (productsInCart.Any()) 
            {
                ShoppingCartDetails.ItemsSource = productsInCart;

                HideAllGrids();
                UpdateShoppingCart();
                ShoppingCartGrid.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Aún no hay productos en el carrito.", "Carrito vacío", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnCloseShoppingCart_Click(object sender, RoutedEventArgs e)
        {
            UpdateShoppingCart();
            HideAllGrids();
            GetBackToGrid();
        }

        private void btnEndOrder_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxTCustumerName.Text))
            {
                MessageBox.Show("Por favor, ingrese el nombre del cliente antes de terminar la venta.",
                                "Validación",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            else
            {
                ShoppingCart.Instance.FinalizeSale(TxTCustumerName.Text, TxtCustumerPrefferences.Text);
                ShowMenuForm();

            }
        }

        private void EmptyCart_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de que desea vaciar el carrito?",
                                 "Confirmación",
                                 MessageBoxButton.YesNo,
                                 MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    ShoppingCart.Instance.ClearCart();

                    ShoppingCartDetails.ItemsSource = null;

                    MessageBox.Show("El carrito ha sido vaciado.",
                                    "Carrito Vacío",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al vaciar el carrito: {ex.Message}",
                                    "Error",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                }
            }

        }


        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProductPOCO product)
            {
                if (product.Quantity < product.ProductAvailableQuantity)
                {
                    product.Quantity++;
                    UpdateShoppingCart();
                }
                else
                {
                    MessageBox.Show("No puedes agregar más de la cantidad disponible.", "Límite alcanzado", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is ProductPOCO product)
            {
                if (product.Quantity > 1)
                {
                    ShoppingCart.Instance.ReduceProductQuantity(product.ProductId);
                    UpdateShoppingCart();
                }
                else
                {
                    MessageBox.Show("La cantidad no puede ser menor a 1.", "Cantidad inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private void UpdateShoppingCart()
        {
            ShoppingCartDetails.Items.Refresh();
        }

        private void DeleteProduct_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ProductPOCO product)
            {
                var result = MessageBox.Show($"¿Deseas eliminar {product.ProductName} del carrito?",
                                             "Eliminar Producto",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        ShoppingCart.Instance.RemoveProduct(product.ProductId);

                        ShoppingCartDetails.ItemsSource = null;
                        ShoppingCartDetails.ItemsSource = ShoppingCart.Instance.GetProducts();

                        MessageBox.Show($"{product.ProductName} ha sido eliminado del carrito.",
                                        "Producto Eliminado",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ocurrió un error al eliminar el producto: {ex.Message}",
                                        "Error",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                }
            }
        }

    }
}
