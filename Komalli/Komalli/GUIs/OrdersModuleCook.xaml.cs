using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.DataBaseManagement.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Lógica de interacción para OrdersModuleCook.xaml
    /// </summary>
    public partial class OrdersModuleCook : Page
    {
        private Timer timer;
        SaleDAO saleDAO = new SaleDAO();

        public OrdersModuleCook()
        {
            InitializeComponent();
            LoadSales();
            InitializeTimer();
        }
        private void InitializeTimer()
        {
            timer = new Timer(10000);
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                LoadSales();
            });
        }

        private void LoadSales()
        {           
            var sales = saleDAO.GetTodaySales();
            SalesDataGrid.ItemsSource = sales;
        }

        private void LoadOrderDetailProducts(int orderId) 
        {
            var orderDetail = saleDAO.GetOrdersBySaleById(orderId);
            OrderDetailDataGrid.ItemsSource = orderDetail;
        }

        private void BtnShowOrderDetail_Click(object sender, RoutedEventArgs e)
        {
            int saleId = (int)((Button)sender).CommandParameter;
            OrderDetail.Visibility = Visibility.Visible;
            var saleSelected = saleDAO.GetSaleById(saleId);
            txtOrderId.Text = saleSelected.SaleId.ToString();
            txtAditionalRequest.Text = saleSelected.AdditionalRequest.ToString();
            LoadOrderDetailProducts(saleId);

        }

        private void BtnUpdateStatusOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            OrderDetail.Visibility = Visibility.Collapsed;
            clearFields();
        }

        private void clearFields() 
        {
            txtAditionalRequest.Text = string.Empty;
            txtOrderId.Text = string.Empty;
        }
    }
}
