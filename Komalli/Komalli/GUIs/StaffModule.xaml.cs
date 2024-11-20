using System;
using System.Collections.Generic;
using System.Data;
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
using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using Komalli.Util;
using Microsoft.Win32;

namespace Komalli.GUIs
{
    /// <summary>
    /// Lógica de interacción para StaffModule.xaml
    /// </summary>
    public partial class StaffModule : Page
    {
        int idStaffSelected = 0;
        int EmployeeNumberSelected = 0;

        public StaffModule()
        {
            InitializeComponent();
            LoadStaffs();   
        }

        private void BtnSaveStaff_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotEmpty())
            {
                StaffPOCO newStaff = new StaffPOCO();
                newStaff.FirstName = txtStaffName.Text;
                newStaff.LastName = txtStaffLastName.Text;
                newStaff.MiddleName = txtStaffSecondLastName.Text;
                newStaff.Password = txtPassword.Text;
                newStaff.Role = (int)cbStaffRol.SelectedValue;
                StaffDAO staffDAO = new StaffDAO();

                if (IsValidateDataStructure())
                {
                    if (EmployeeNumberSelected == 0)
                    {
                        try
                        {
                            int register = staffDAO.RegisterStaff(newStaff);
                            if (register == 1)
                            {
                                MessageBox.Show("Personal registrado con éxito", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                                CleanFields();
                                RegisterStaffForm.Visibility = Visibility.Collapsed;
                                LoadStaffs();
                            }
                        }
                        catch (SqlException)
                        {
                            MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else if (EmployeeNumberSelected != 0)
                    {
                        try
                        {
                            int update = staffDAO.UpdateStaff(newStaff, EmployeeNumberSelected);
                            if (update == 1)
                            {
                                MessageBox.Show("Personal actualizado con éxito", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                                CleanFields();
                                RegisterStaffForm.Visibility = Visibility.Collapsed;
                                LoadStaffs();
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CleanFields();
            RegisterStaffForm.Visibility = Visibility.Collapsed;
            LoadStaffs();
        }

        private void BtnRegisterStaff_Click(object sender, MouseButtonEventArgs e)
        {         
            RegisterStaffForm.Visibility = Visibility.Visible;
            TitleForm.Content = "Registrar personal";
            LoadRoles();

        }

        private void LoadStaffs() 
        {
            try
            {
                StaffDAO staffDAO = new StaffDAO();
                var staffList = staffDAO.GetLastTenStaffRecords();
                StaffGrid.ItemsSource = staffList;  
            }
            catch (SqlException)
            {
                MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRoles()
        {
            StaffDAO staffDAO = new StaffDAO();
            List<RolePOCO> roleList = staffDAO.GetAllRoles();
            cbStaffRol.ItemsSource = roleList;
        }
        private void BtnUpdateStaff_Click(object sender, RoutedEventArgs e)
        {
            int staffId = (int)((Button)sender).CommandParameter;
            idStaffSelected = staffId;
            RegisterStaffForm.Visibility = Visibility.Visible;
            LoadRoles();
            TitleForm.Content = "Editar personal";
            StaffDAO staffDAO = new StaffDAO();
            var staffSelected = staffDAO.GetStaffById(idStaffSelected);
            txtStaffName.Text = staffSelected.FirstName;
            txtStaffLastName.Text = staffSelected.LastName;
            txtStaffSecondLastName.Text = staffSelected.MiddleName;
            txtPassword.Text = staffSelected.Password;
            cbStaffRol.SelectedValue = staffSelected.Role;
            EmployeeNumberSelected = idStaffSelected;
        }

        private void BtnDeleteStaff_Click(object sender, RoutedEventArgs e)
        {
            int staffId = (int)((Button)sender).CommandParameter;
            idStaffSelected = staffId;
            MessageBoxResult resultMessage = MessageBox.Show("¿Estás seguro de eliminar al personal? Esta acción no es reversible", "Confirmacion", MessageBoxButton.YesNo);
            if (resultMessage == MessageBoxResult.Yes)
            {
               try
               {
                    StaffDAO staffDAO = new StaffDAO();
                    int result = staffDAO.DeleteStaff(idStaffSelected);
                    if (result == 1)
                    {
                        MessageBox.Show("Personal eliminado con éxito", "Eliminación", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadStaffs();
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsValidateDataStructure() { 
            bool result = false;
            string name = txtStaffName.Text;
            string lastName = txtStaffLastName.Text;
            string secondLastName = txtStaffSecondLastName.Text;
            string password = txtPassword.Text;

            if (Utilities.IsValidSpanishLettersAndSpaces(name) && 
                Utilities.IsValidSpanishLettersAndSpaces(lastName) && 
                Utilities.IsValidSpanishLettersAndSpaces(secondLastName) &&
                Utilities.IsValidSpanishLettersAndSpecialChars(password)) 
            {
                result = true;
            }
            return result;
        }

        private bool IsNotEmpty()
        {
            bool result = false;

            if (txtStaffName.Text != "" &&
                txtStaffLastName.Text != "" &&
                txtStaffSecondLastName.Text != "" &&
                txtPassword.Text != "" &&
                cbStaffRol.SelectedItem != null) 
            {
                result = true;
            }
            return result;
        }

        private void CleanFields()
        {
            txtStaffName.Text = "";
            txtStaffLastName.Text = "";
            txtStaffSecondLastName.Text = "";
            txtPassword.Text = "";
            cbStaffRol.SelectedItem = null;
        }

        private void BtnSearchStaff_Click(object sender, MouseButtonEventArgs e)
        {  
            if (txtStaffSearch.Text != "") 
            {
                string nameToSearch = txtStaffSearch.Text;
                try
                {
                    StaffDAO staffDAO = new StaffDAO();
                    var result = staffDAO.GetStaffByName(nameToSearch);
                    if (result.Count != 0)
                    {
                        StaffGrid.ItemsSource = result;
                    }
                    else
                    {
                        MessageBox.Show("No se encontraron resultados que coincidan con tu búsqueda", "Busqueda incompleta", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                LoadStaffs();
            }
        }

        private void BtnReloadStaffs_Click(object sender, MouseButtonEventArgs e)
        {
            LoadStaffs();
            txtStaffSearch.Text = "";
        }
    }
}
