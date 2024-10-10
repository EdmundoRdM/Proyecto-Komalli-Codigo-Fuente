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
                    // Contraseña oculta, ambos campos están llenos (Usuario y PasswordBox)
                    // Aquí puedes ejecutar la lógica de inicio de sesión normal
                    MessageBox.Show("Inicio de sesión con contraseña oculta exitoso.", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                    username = txtUserName.Text;
                    password = pwdPassword.Password;
                    break;
                case 2:
                    // Contraseña visible, ambos campos están llenos (Usuario y TextBox de la contraseña visible)
                    // Lógica de inicio de sesión para contraseña visible
                    MessageBox.Show("Inicio de sesión con contraseña visible exitoso.", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                    username = txtUserName.Text;
                    password = txtPassword.Text;
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
