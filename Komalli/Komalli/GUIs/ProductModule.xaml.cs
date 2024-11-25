using Komalli.DataBaseManagement.DataAccessObject;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Lógica de interacción para ProductModule.xaml
    /// </summary>
    public partial class ProductModule : Page
    {
        int idProductSelected = 0;
        
        public ProductModule()
        {
            InitializeComponent();
            loadProducts();
        }

        private void BtnSearchProduct_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnReloadProduct_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnRegisterProduct_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnUpdateProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveProduct_Click(object sender, RoutedEventArgs e)
        {

        }

        private void loadProducts()
        {
            try
            {
                ProductDAO productDAO = new ProductDAO();
                var productList = productDAO.GetAllProducts();
                ProductListView.ItemsSource = productList;
            }
            catch (SqlException)
            {
                MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
