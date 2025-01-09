using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
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
    /// Lógica de interacción para OrdersCashier.xaml
    /// </summary>
    public partial class OrdersCashier : Page
    {
        private SaleDAO saleDAO = new SaleDAO();
        public OrdersCashier()
        {
            InitializeComponent();
            LoadSalesWithStatus(2);
        }

        private void LoadSalesWithStatus(int status)
        {
            try
            {
                List<SalePOCO> sales = saleDAO.GetSalesByStatus(status);

                OrdersDataGrid.ItemsSource = sales;
                OrdersBackground.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las ventas con estado {status}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void OrdenInfo_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is SalePOCO selectedSale)
            {
                try
                {
                    ProductDAO productDao = new ProductDAO();
                    List<ProductPOCO> products = productDao.GetProductsBySaleId(selectedSale.SaleId);

                    ShoppingCartDetails.ItemsSource = products;

                    lblCustomerName.Content = selectedSale.CustomerName;
                    Total.Content = $"Total: {selectedSale.TotalSale:C}";
                    TxtCustumerPrefferences.Text = selectedSale.AdditionalRequest; 

                    OrdersBackground.Visibility = Visibility.Hidden;
                    OrdersDetails.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los detalles de la orden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una orden válida.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }




        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is SalePOCO selectedSale)
            {
                try
                {
                    SaleDAO saleDao = new SaleDAO();

                    saleDao.UpdateSaleStatus(selectedSale.SaleId, 4);

                    MessageBox.Show("La orden ha sido cancelada con exito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadSalesWithStatus(2);

                    OrdersDetails.Visibility = Visibility.Hidden;
                    OrdersBackground.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cancelar la orden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una orden válida.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

        }
        private void EndOrder_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is SalePOCO selectedSale)
            {
                try
                {
                    SaleDAO saleDao = new SaleDAO();

                    saleDao.UpdateSaleStatus(selectedSale.SaleId, 3);

                    MessageBox.Show("La orden ha sido entregada exitosamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadSalesWithStatus(2);

                    OrdersDetails.Visibility = Visibility.Hidden;
                    OrdersBackground.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al entregar la orden: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una orden válida.", "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BackToList_Click(object sender, RoutedEventArgs e)
        {
            OrdersBackground.Visibility = Visibility.Visible;
            OrdersDetails.Visibility = Visibility.Hidden;

        }

    }
}
