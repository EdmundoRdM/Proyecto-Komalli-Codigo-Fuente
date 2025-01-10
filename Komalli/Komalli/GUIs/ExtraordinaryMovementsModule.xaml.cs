using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.DataBaseManagement.POCOs;
using Komalli.Util;
using System;
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
    /// Lógica de interacción para ExtraordinaryMovementsModule.xaml
    /// </summary>
    public partial class ExtraordinaryMovementsModule : Page
    {
        int idMovementSelected;
        MovementDAO movementDAO = new MovementDAO();
        

        public ExtraordinaryMovementsModule()
        {
            InitializeComponent();
            LoadMovements();
        }

        private void BtnRegisterMovement_Click(object sender, MouseButtonEventArgs e)
        {
            RegisterMovementForm.Visibility = Visibility.Visible;
            TitleForm.Content = "Registrar movimiento extraordinario";
        }

        private void BtnUpdateMovement_Click(object sender, RoutedEventArgs e)
        {
            int movementID = (int)((Button)sender).CommandParameter;
            idMovementSelected = movementID;
            RegisterMovementForm.Visibility = Visibility.Visible;
            TitleForm.Content = "Editar movimiento extraordinario";
            var movementsSelected = movementDAO.GetMovementByID(idMovementSelected);
            txtAmountMovement.Text = movementsSelected.Amount.ToString();
            txtDescription.Text = movementsSelected.Description;
            if (movementsSelected.MovementType == 1)
            {
                RBIncomeReport.IsChecked = true;
                RBOutcomeReport.IsChecked = false;
            }
            else if(movementsSelected.MovementType == 2)
            {
                RBIncomeReport.IsChecked = false;
                RBOutcomeReport.IsChecked = true;
            }
        }

        private void BtnDeleteMovement_Click(object sender, RoutedEventArgs e)
        {
            int movementID = (int)((Button)sender).CommandParameter;
            idMovementSelected = movementID;
            MessageBoxResult resultMessage = MessageBox.Show("¿Estás seguro de eliminar el movimiento extraodinario? Esta acción no es reversible", "Confirmacion", MessageBoxButton.YesNo);
            if (resultMessage == MessageBoxResult.Yes)
            {
                try
                {
                    int result = movementDAO.DeleteMovement(idMovementSelected);
                    if (result == 1)
                    {
                        MessageBox.Show("Movimiento eliminado con éxito", "Eliminación", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadMovements();
                    }
                }
                catch (SqlException)
                {
                    MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnSaveMovement_Click(object sender, RoutedEventArgs e)
        {
            if (IsNotEmpty())
            {
                StaffPOCO staffPOCO = StaffToken.GetEmployeePOCO();
                int rol = staffPOCO.Role;
                MovementPOCO newMovement = new MovementPOCO();
                newMovement.Description = txtDescription.Text;
                string amountText = txtAmountMovement.Text;
                if (decimal.TryParse(amountText, out decimal amount))
                {
                    newMovement.Amount = amount;
                }
                newMovement.Date = DateTime.Now;

                if (RBIncomeReport.IsChecked == true)
                {
                    newMovement.MovementType = 1;
                }
                else if(RBOutcomeReport.IsChecked == true)
                {
                    newMovement.MovementType = 2;
                }
                newMovement.StaffID = rol;
                newMovement.MovementTypeName = "";

                if (IsValidateDataStructure())
                {
                    if (idMovementSelected == 0)
                    {
                        try
                        {
                            int register = movementDAO.RegisterMovement(newMovement);
                            if (register == 1)
                            {
                                MessageBox.Show("Movimiento registrado con éxito", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                                CleanFields();
                                LoadMovements();
                                RegisterMovementForm.Visibility = Visibility.Collapsed;
                                
                            }
                        }
                        catch (SqlException)
                        {
                            MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else if (idMovementSelected != 0)
                    {
                        try
                        {
                            int update = movementDAO.UpdateMovement(newMovement, idMovementSelected);
                            if (update == 1)
                            {
                                MessageBox.Show("Movimiento actualizado con éxito", "Registro", MessageBoxButton.OK, MessageBoxImage.Information);
                                CleanFields();
                                LoadMovements();
                                RegisterMovementForm.Visibility = Visibility.Collapsed;
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
            RegisterMovementForm.Visibility = Visibility.Collapsed;
            CleanFields();
        }

        private void LoadMovements()
        {
            try
            {     
                var movementsList = movementDAO.GetAllMovements();
                MovementsGrid.ItemsSource = movementsList;
            }
            catch (SqlException)
            {
                MessageBox.Show("Por el momento no se puede conectar a la base de datos. Intente de nuevo más tarde.", "Error al conectar a base de datos", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CleanFields()
        {
            txtAmountMovement.Text = "";
            txtDescription.Text = "";
            RBIncomeReport.IsChecked = false;
            RBOutcomeReport.IsChecked = false;
        }

        private bool IsNotEmpty()
        {
            bool result = false;

            if (txtAmountMovement.Text != "" &&
                txtDescription.Text != "" &&
                (RBIncomeReport.IsChecked == true || RBOutcomeReport.IsChecked == true))
            {
                result = true;
            }
            return result;
        }

        private bool IsValidateDataStructure()
        {
            bool result = false;
            string description = txtDescription.Text;
            string amountText = txtAmountMovement.Text;
            Console.WriteLine(Utilities.IsValidSpanishLettersAndSpaces(description));
            Console.WriteLine(Utilities.IsValidDecimal(amountText));
            if (Utilities.IsValidSpanishLettersAndSpaces(description) &&
                Utilities.IsValidDecimal(amountText))
            {
                
                result = true;
            }
            return result;
        }
    }
}
