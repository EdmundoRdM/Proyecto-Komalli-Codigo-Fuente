using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.Util;
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

    public partial class ReportsModule : Page
    {
        public ReportsModule()
        {
            InitializeComponent();
        }

        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            if (RBDailReport.IsChecked == true)
            {
                try
                {
                    var sales = SaleDAO.GetTodaySalesForReport();
                    string filePath = GetSaveFilePathSales();

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        ReportGenerator.GenerateDailySalesReport(filePath, sales);
                        System.Diagnostics.Process.Start("explorer", filePath);
                        MessageBox.Show("Reporte creado exitosamente", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (RBInvetoryReport.IsChecked == true) 
            {
                try
                {
                    var products = ProductDAO.GetProductsForReport();
                    string filePath = GetSaveFilePathInventory();

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        ReportGenerator.GenerateDailyInventoryReport(filePath, products);
                        System.Diagnostics.Process.Start("explorer", filePath);
                        MessageBox.Show("Reporte creado exitosamente", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }           

        }

        private string GetSaveFilePathSales()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Guardar reporte",
                Filter = "Documentos de Word (*.docx)|*.docx",
                FileName = $"ReporteDiarioVentas_{DateTime.Now:yyyyMMdd}.docx",
                DefaultExt = ".docx"
            };

            bool? result = saveFileDialog.ShowDialog();
            return result == true ? saveFileDialog.FileName : null;
        }

        private string GetSaveFilePathInventory()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Guardar reporte",
                Filter = "Documentos de Word (*.docx)|*.docx",
                FileName = $"ReporteInventario_{DateTime.Now:yyyyMMdd}.docx",
                DefaultExt = ".docx"
            };

            bool? result = saveFileDialog.ShowDialog();
            return result == true ? saveFileDialog.FileName : null;
        }

    }
}
