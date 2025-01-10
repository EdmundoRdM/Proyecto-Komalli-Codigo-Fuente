using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.DataBaseManagement.POCOs;
using Komalli.Util;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
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
        public ICollectionView ProductsCollectionView { get; }
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
            loadProducts();
            txtProductSearch.Text = "";
        }

        private void BtnRegisterProduct_Click(object sender, MouseButtonEventArgs e)
        {
            RegisterProductForm.Visibility = Visibility.Visible;
            TitleForm.Content = "Registrar producto";
            loadProductTypes();
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
            if (IsNotEmpty())
            {
               
                ProductPOCO newProduct = new ProductPOCO();
                newProduct.ProductName = txtProductName.Text;
                newProduct.ProductAvailableQuantity = int.Parse(txtProductAvailableQuantity.Text);
                newProduct.ProductStatus = true;
                newProduct.ProductDescription = txtProductDescription.Text;
                newProduct.ProductPrice = decimal.Parse(txtProductPrice.Text);
                newProduct.ProductTypeId = (int)cbProductType.SelectedValue;
                if (chBFromKitchen.IsChecked == true)
                {
                    newProduct.ProductFromKitchen = true;
                } else newProduct.ProductFromKitchen = false;
                ProductDAO productDAO = new ProductDAO();

                if (true)
                {
                    if (idProductSelected == 0)
                    {
                        try
                        {
                            bool register = productDAO.RegisterProduct(newProduct);
                            if (register == true)
                            {
                                MessageBox.Show("Product registrado con éxito", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClearFields();
                                RegisterProductForm.Visibility = Visibility.Collapsed;
                                loadProducts();
                            }
                        }
                        catch (SqlException)
                        {
                            MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else if (idProductSelected != 0)
                    {
                        try
                        {
                            bool update = productDAO.UpdateProduct(idProductSelected, newProduct);
                            if (update == true)
                            {
                                MessageBox.Show("Product actualizado con éxito", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClearFields();
                                RegisterProductForm.Visibility = Visibility.Collapsed;
                                loadProducts();
                            }
                        }
                        catch (SqlException)
                        {
                            MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                }
                else
                {
                    MessageBox.Show("La estructura de los datos es inválida. Ingrese datos correctos e intente de nuevo.", "Estructura de datos incorrecta", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            else
            {
                MessageBox.Show("La estructura de los datos es inválida. Ingrese datos correctos e intente de nuevo.", "Estructura de datos incorrecta", MessageBoxButton.OK, MessageBoxImage.Error);

            }
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

        private void loadProductTypes()
        {
            ProductTypeDAO typeDAO = new ProductTypeDAO();
            List<ProductTypePOCO> typeList = typeDAO.GetAllTypes();
            cbProductType.ItemsSource = typeList;

        }
        private bool IsValidDataStructure()
        {
            bool result = false;
            string name = txtProductName.Text;
            string description = txtProductDescription.Text;
            string price = txtProductPrice.Text;
            string available = txtProductAvailableQuantity.Text;

            if (Utilities.IsValidSpanishLettersAndSpaces(name) &&
                Utilities.IsValidSpanishLettersAndSpaces(description) &&
                Utilities.IsValidInteger(available) &&
                Utilities.IsValidDecimal(price))
            {
                result = true;
            }
            return result;
        }
        private bool IsNotEmpty()
        {
            bool result = false;

            if (txtProductName.Text != "" &&
                txtProductDescription.Text != "" &&
                txtProductPrice.Text != "" &&
                txtProductAvailableQuantity.Text != "" &&
                cbProductType.SelectedItem != null)
            {
                result = true;
            }
            return result;
        }
        private void ClearFields()
        {
            txtProductName.Text = "";
            txtProductDescription.Text = "";
            txtProductPrice.Text = "";
            txtProductAvailableQuantity.Text = "";
            cbProductType.SelectedItem = null;
        }
    }
}
