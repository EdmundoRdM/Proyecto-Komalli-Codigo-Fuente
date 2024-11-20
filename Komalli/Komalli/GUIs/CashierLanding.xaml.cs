using Komalli.DataBaseManagement.DataAccessObject;
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
        public CashierLanding()
        {
            InitializeComponent();
        }

        public void ShowMenuForm()
        {
            MenuForm.Visibility = Visibility.Visible;
            DefaultBackground.Visibility = Visibility.Hidden;
            TodaysMealsGrid.Visibility = Visibility.Visible; 
            GeneralMenuGrid.Visibility = Visibility.Hidden;
            List<ProductPOCO> todaysMeals = productDAO.GetMealsForToday(DateTime.Now);
            UpdateMenuLabels(todaysMeals, DateTime.Now);
        }


        public void UpdateMenuLabels(List<ProductPOCO> mealsForToday, DateTime todayDate)
        {
            lblMenuDate.Content = $"Menú del día {todayDate:dd/MM/yyyy}";

            // Filtrar el desayuno y la comida
            var breakfast = mealsForToday.FirstOrDefault(p => p.ProductTypeId == 1); 
            Console.Write(breakfast.ProductDescription);
            var lunch = mealsForToday.FirstOrDefault(p => p.ProductTypeId == 2);
            Console.Write(lunch.ProductDescription);

            if (breakfast != null)
            {
                lblBreakfast.Content = FormatDescription(breakfast.ProductDescription);
            }

            if (lunch != null)
            {
                lblMeal.Content = FormatDescription(lunch.ProductDescription);
            }
        }

        // Método para separar la descripción por saltos de línea
        private string FormatDescription(string description)
        {
            return description.Replace(",", "\n");
        }

        private void btnGeneralMenu_Click(object sender, RoutedEventArgs e)
        {
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: De la carta";
            TodaysMealsGrid.Visibility = Visibility.Hidden;
            GeneralMenuGrid.Visibility = Visibility.Visible;
            Products = productDAO.GetProductsByType(3);
            this.GeneralMenuList.ItemsSource = Products;
        }

        private void ShoppingCart_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ProductPOCO product)
            {
                // Lógica para el carrito
                MessageBox.Show($"Carrito clickeado para: {product.ProductName}, {product.ProductId}");
            }
        }

        private void InfoIcon_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image image && image.DataContext is ProductPOCO product)
            {
                // Lógica para la información
                MessageBox.Show($"Información clickeada para: {product.ProductName}, {product.ProductId}");
            }
        }


        private void btnTodaysMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuForm.Visibility = Visibility.Visible;
            DefaultBackground.Visibility = Visibility.Hidden;
            TodaysMealsGrid.Visibility = Visibility.Visible;
            GeneralMenuGrid.Visibility = Visibility.Hidden;
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: Menu del Día";
            Products = productDAO.GetProductsByType(4);
            this.GeneralMenuList.ItemsSource = Products;

        }
        private void btnDrinks_Click(object sender, RoutedEventArgs e)
        {
            MenuForm.Visibility = Visibility.Visible;
            DefaultBackground.Visibility = Visibility.Hidden;
            TodaysMealsGrid.Visibility = Visibility.Hidden;
            GeneralMenuGrid.Visibility = Visibility.Visible;
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: Bebidas";
            Products = productDAO.GetProductsByType(4);
            this.GeneralMenuList.ItemsSource = Products;

        }

        private void btnDesserts_Click(object sender, RoutedEventArgs e)
        {
            MenuForm.Visibility = Visibility.Visible;
            DefaultBackground.Visibility = Visibility.Hidden;
            TodaysMealsGrid.Visibility = Visibility.Hidden;
            GeneralMenuGrid.Visibility = Visibility.Visible;
            lblGeneralMenu.Content = "Menú Cafetería FEIGE: Postres";
            Products = productDAO.GetProductsByType(5);
            this.GeneralMenuList.ItemsSource = Products;
        }


    }
}
