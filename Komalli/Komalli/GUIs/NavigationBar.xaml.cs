using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
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
    /// <summary>
    /// Lógica de interacción para NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        Dictionary<string, Button> ButtonsNavegationBar = new Dictionary<string, Button>();
        public NavigationBar()
        {
            InitializeComponent();
            LoadButtons();
            ShowButtonsbyUserType();
        }

        private void LoadButtons()
        {        
            ButtonsNavegationBar.Add("ReportsModule", btnReports);
            ButtonsNavegationBar.Add("ProductsModule", btnProducts);
            ButtonsNavegationBar.Add("StaffModule", btnStaff);
            ButtonsNavegationBar.Add("ExtraordinaryMovementsModule", btnExtraordinaryMovements);
            ButtonsNavegationBar.Add("GenerateOrderModule", btnGenerateOrder);
            ButtonsNavegationBar.Add("OrderModule", btnOrder);
            ButtonsNavegationBar.Add("SpoilageModule", btnSpoilage);
            
        }

        private void ShowButtonsbyUserType()
        {
            StaffPOCO staffPOCO = StaffToken.GetEmployeePOCO();
            int rol = staffPOCO.Role;
            if (rol != 1 || rol != 2 || rol != 3 || rol != 4)
            {
               /* 1 gerente
                    2 caajero
                    3 cocinero 
                    4 acnonimo*/
                switch (rol)
                {
                    case 1:
                        btnGenerateOrder.Visibility = Visibility.Hidden;
                        btnOrder.Visibility = Visibility.Hidden;
                        btnSpoilage.Visibility = Visibility.Hidden;
                        break;
                    case 2:
                        btnReports.Visibility = Visibility.Hidden;
                        btnProducts.Visibility = Visibility.Hidden;
                        btnStaff.Visibility = Visibility.Hidden;
                        btnSpoilage.Visibility = Visibility.Hidden;
                        break;
                    case 3:
                        btnReports.Visibility = Visibility.Hidden;
                        btnProducts.Visibility = Visibility.Hidden;
                        btnStaff.Visibility = Visibility.Hidden;
                        btnGenerateOrder.Visibility = Visibility.Hidden;
                        btnExtraordinaryMovements.Visibility = Visibility.Hidden;
                        break;
                }

                CheckVisibility();
            }
        }

        public void CheckVisibility()
        {
            double distanceBetweenElements = 10;

            for (int i = 1; i < ButtonsNavegationBar.Count; i++)
            {
                Button button = ButtonsNavegationBar.ElementAt(i).Value;

                Button buttonAnterior = ButtonsNavegationBar.ElementAt(i - 1).Value;
                double scrollingY = 0;

                if (buttonAnterior.Visibility == Visibility.Hidden)
                {
                    scrollingY = buttonAnterior.Margin.Top + buttonAnterior.ActualHeight + distanceBetweenElements;
                    button.Margin = new Thickness(button.Margin.Left, scrollingY, button.Margin.Right, button.Margin.Bottom);
                }
                else
                {
                    scrollingY = buttonAnterior.Margin.Top + 90;
                    button.Margin = new Thickness(button.Margin.Left, scrollingY, button.Margin.Right, button.Margin.Bottom);
                }
            }
        }

        private void BtnReportsModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnProductsModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStaffModule_Click(object sender, RoutedEventArgs e)
        {
            Utilities.ChangePage(new StaffModule());
        }

        private void BtnExtraordinaryMovementsModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOut_Click(object sender, MouseButtonEventArgs e)
        {
            Utilities.ChangePage(new LogIn());
        }


        private void BtnGenerateOrderModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOrderModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSpoilageModule_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
