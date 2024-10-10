﻿using Komalli.DataBaseManagement.DataAccessObject;
using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using Komalli.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    /// Lógica de interacción para LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void LogIng_Click(object sender, RoutedEventArgs e)
        {
            int fieldsOk = ValidateFields();
            string username = "";
            string password = "";
            switch (fieldsOk)
            {
                case 0:
                    break;

                case 1:
                    username = txtUserName.Text;
                    password = pwdPassword.Password;
                    VerifyLogin(username, password);
                    break;
                case 2:
                    username = txtUserName.Text;
                    password = txtPassword.Text;
                    VerifyLogin(username, password);
                    break;

                default:
                    MessageBox.Show("Ocurrió un error inesperado.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }

        private int ValidateFields()
        {
            int result = 0;
            if(txtPassword.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    MessageBox.Show("Por favor escriba su usuario y contraseña para continuar", "Campos requeridos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    result = 2;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(txtUserName.Text) || string.IsNullOrWhiteSpace(pwdPassword.Password))
                {
                    MessageBox.Show("Por favor escriba su usuario y contraseña para continuar", "Campos requeridos", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    result = 1;
                }
            }
            return result;
        }


        private void VerifyLogin(string employeeNumber, string password)
        {
            StaffPOCO staffPoco = new StaffPOCO
            {
                EmployeeNumber = employeeNumber,
                Password = password
            };

            try
            {
                StaffDAO staffDAO = new StaffDAO();
                StaffPOCO pocoLogIn = staffDAO.VerifyLogin(staffPoco);

                int role = pocoLogIn.Role;

                if (role != -1)
                {
                    StaffToken.SetEmployeeID(pocoLogIn.StaffId);
                    GoToRoleLanding(role);
                }
                else
                {
                    MessageBox.Show("Número de empleado o contraseña incorrectos.", "Error de inicio de sesión", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar sesión: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GoToRoleLanding(int role)
        {
            switch (role)
            {
                //Aqui va la navegacion al landin que corresponda. LEs dejo ejemplos. leugo borran los comentarios paro.

                case 1:
                    MessageBox.Show("Bienvenido Gerente. Id emleado: " +StaffToken.GetEmployeeID() , "Inicio de Sesión", MessageBoxButton.OK, MessageBoxImage.Information);
                    //NavigationService.Navigate(new Uri("GerentePage.xaml", UriKind.Relative));
                    break;

                case 2:
                    MessageBox.Show("Bienvenido Cajero. Id emleado:" +StaffToken.GetEmployeeID() , "Inicio de Sesión", MessageBoxButton.OK, MessageBoxImage.Information);
                    //NavigationService.Navigate(new Uri("CajeroPage.xaml", UriKind.Relative));
                    break;

                case 3: 
                    MessageBox.Show("Bienvenido Cocinero. Id emleado:" +StaffToken.GetEmployeeID(), "Inicio de Sesión", MessageBoxButton.OK, MessageBoxImage.Information);
                    //NavigationService.Navigate(new Uri("CocineroPage.xaml", UriKind.Relative));
                    break;

                case 4: 
                    MessageBox.Show("Bienvenido Usuario Anónimo. Id emleado:" +StaffToken.GetEmployeeID(), "Inicio de Sesión", MessageBoxButton.OK, MessageBoxImage.Information);
                    //NavigationService.Navigate(new Uri("AnonimoPage.xaml", UriKind.Relative));
                    break;

                default:
                    MessageBox.Show("Usuario o contraseña incorrectos. Por favor vuelva a intentarlo.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    break;
            }
        }

        private void SetStaffToken(int staffID)
        {
            StaffToken.SetEmployeeID(staffID);
        }



        private void HidePassword(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Text = pwdPassword.Password;

            pwdPassword.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;

            btnHidePassword.Visibility = Visibility.Collapsed;
            btnSeePassword.Visibility = Visibility.Visible;


        }
        private void SeePassword(object sender, MouseButtonEventArgs e)
        {
            pwdPassword.Password = txtPassword.Text;

            pwdPassword.Visibility = Visibility.Visible;
            txtPassword.Visibility = Visibility.Collapsed;

            btnHidePassword.Visibility = Visibility.Visible;
            btnSeePassword.Visibility = Visibility.Collapsed;

        }

    }
}