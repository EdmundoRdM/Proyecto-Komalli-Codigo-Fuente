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
    /// Lógica de interacción para SpoilageLanding.xaml
    /// </summary>
    public partial class SpoilageLanding : Page
    {
        private List<SpoilageDetailPOCO> originalDetails; 
        private List<SpoilageDetailPOCO> editedDetails;

        public bool IsEditing { get; set; } = false;
        public List<SpoilagePOCO> Spoilages { get; set; }
        public SpoilageLanding()
        {
            InitializeComponent();
            LoadSpoilages();
        }

        private void LoadSpoilages()
        {
            try
            {
                SpoilageDAO spoilageDAO = new SpoilageDAO();
                Spoilages = spoilageDAO.GetAllSpoilages();

                SpoilageListBox.ItemsSource = Spoilages; 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las mermas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateTotalPriceLabel()
        {
            var details = (List<SpoilageDetailPOCO>)OrderDetailDataGrid.ItemsSource;

            if (details != null)
            {
                decimal totalPrice = details.Sum(d => d.Total);

                lblTotalPrice.Content = $"Precio total: {totalPrice:C}";
            }
            else
            {
                lblTotalPrice.Content = "Precio total: $0.00";
            }
        }


        private void btnViewSpoilageDetails_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedSpoilage = button?.Tag as SpoilagePOCO;

            if (selectedSpoilage != null)
            {
                try
                {
                    
                    SpoilageDetailsBackground.Visibility = Visibility.Visible;
                    SpoilageContainer.Visibility = Visibility.Hidden;

                    
                    foreach (var detail in selectedSpoilage.Details)
                    {
                        detail.SpoilageId = selectedSpoilage.SpoilageId; 
                    }

                    originalDetails = selectedSpoilage.Details.Select(d => new SpoilageDetailPOCO
                    {
                        SpoilageId = d.SpoilageId, 
                        ProductId = d.ProductId,
                        ProductName = d.ProductName,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = d.Total
                    }).ToList();

                    editedDetails = originalDetails.Select(d => new SpoilageDetailPOCO
                    {
                        SpoilageId = d.SpoilageId, 
                        ProductId = d.ProductId,
                        ProductName = d.ProductName,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = d.Total
                    }).ToList();

                    OrderDetailDataGrid.ItemsSource = editedDetails;

                    UpdateTotalPriceLabel();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al cargar los detalles de la merma: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("No se pudo identificar la merma seleccionada.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }





        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var context = button?.Tag as SpoilageDetailPOCO;

            if (context != null)
            {
                context.Quantity++;
                context.Total = context.Quantity * context.UnitPrice;

                OrderDetailDataGrid.Items.Refresh();
                UpdateTotalPriceLabel();
            }
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var context = button?.Tag as SpoilageDetailPOCO;

            if (context != null && context.Quantity > 0)
            {
                context.Quantity--;
                context.Total = context.Quantity * context.UnitPrice;

                OrderDetailDataGrid.Items.Refresh();
                UpdateTotalPriceLabel();
            }
        }




        private void btnBackToSpoilageList_Click(object sender, RoutedEventArgs e)
        {
            if (HasUnsavedChanges())
            {
                var result = MessageBox.Show(
                    "¿Desea guardar los cambios antes de regresar?",
                    "Cambios pendientes",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    btnSaveChanges_Click(sender, e);
                }
                else if (result == MessageBoxResult.No)
                {
                    DiscardChanges(); 
                }
                else
                {
                    return;
                }
            }

            SpoilageDetailsBackground.Visibility = Visibility.Hidden;
            SpoilageContainer.Visibility = Visibility.Visible;
        }


        private bool HasUnsavedChanges()
        {
            return !originalDetails.SequenceEqual(editedDetails, new SpoilageDetailComparer());
        }

        public class SpoilageDetailComparer : IEqualityComparer<SpoilageDetailPOCO>
        {
            public bool Equals(SpoilageDetailPOCO x, SpoilageDetailPOCO y)
            {
                return x.ProductId == y.ProductId &&
                       x.Quantity == y.Quantity &&
                       x.Total == y.Total;
            }

            public int GetHashCode(SpoilageDetailPOCO obj)
            {
                return obj.ProductId.GetHashCode() ^ obj.Quantity.GetHashCode() ^ obj.Total.GetHashCode();
            }
        }

        private void DiscardChanges()
        {
            editedDetails = originalDetails.Select(d => new SpoilageDetailPOCO
            {
                ProductId = d.ProductId,
                ProductName = d.ProductName,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice,
                Total = d.Total
            }).ToList();

            OrderDetailDataGrid.ItemsSource = editedDetails;
            OrderDetailDataGrid.Items.Refresh();
            UpdateTotalPriceLabel();
        }


        private void SetEditingMode(bool isEditing)
        {
            IsEditing = isEditing;

            OrderDetailDataGrid.Items.Refresh();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnSaveChanges.Visibility = Visibility.Visible;

            SetEditingMode(true);

            Console.WriteLine("Modo edición habilitado.");
        }


        private void btnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SpoilageDAO spoilageDAO = new SpoilageDAO();
                bool allSaved = true;

                foreach (var detail in editedDetails)
                {
                    var originalDetail = originalDetails.FirstOrDefault(d => d.ProductId == detail.ProductId);

                    if (originalDetail != null && originalDetail.Quantity != detail.Quantity)
                    {
                        int quantityDifference = detail.Quantity - originalDetail.Quantity;

                        if (quantityDifference > 0)
                        {
                            int availableQuantity = spoilageDAO.GetProductAvailableQuantity(detail.ProductId);

                            if (availableQuantity >= quantityDifference)
                            {
                                if (detail.SpoilageId == 0)
                                {
                                    detail.SpoilageId = originalDetail.SpoilageId;
                                }

                                spoilageDAO.UpdateSpoilageDetail(detail);

                                spoilageDAO.UpdateProductInventory(detail.ProductId, -quantityDifference);
                            }
                            else
                            {
                                MessageBox.Show($"No hay suficiente inventario disponible para el producto {detail.ProductName}.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                                allSaved = false;
                                break;
                            }
                        }
                        else if (quantityDifference < 0)
                        {
                            if (detail.SpoilageId == 0)
                            {
                                detail.SpoilageId = originalDetail.SpoilageId;
                            }

                            spoilageDAO.UpdateSpoilageDetail(detail);
                            spoilageDAO.UpdateProductInventory(detail.ProductId, -quantityDifference);
                        }
                    }
                }

                if (allSaved)
                {
                    MessageBox.Show("Cambios guardados correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    originalDetails = editedDetails.Select(d => new SpoilageDetailPOCO
                    {
                        SpoilageId = d.SpoilageId,
                        ProductId = d.ProductId,
                        ProductName = d.ProductName,
                        Quantity = d.Quantity,
                        UnitPrice = d.UnitPrice,
                        Total = d.Total
                    }).ToList();

                    SetEditingMode(false);
                    btnSaveChanges.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar cambios: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }






    }
}
