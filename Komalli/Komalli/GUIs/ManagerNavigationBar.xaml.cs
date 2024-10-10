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
using Page = System.Windows.Controls.Page;

namespace Komalli.GUIs
{
    /// <summary>
    /// Lógica de interacción para ManagerNavigationBar.xaml
    /// </summary>
    public partial class ManagerNavigationBar : UserControl
    {
        public ManagerNavigationBar()
        {
            InitializeComponent();
        }
        private void BtnReportsModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnProductsModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnStaffModule_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(new StaffModule());
        }

        private void BtnExtraordinaryMovementsModule_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogOut_Click(object sender, MouseButtonEventArgs e)
        {

        }
        private void ChangePage(Page page)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            Frame framePrincipal = mainWindow.FindName("frameContainer") as Frame;
            framePrincipal.Navigate(page);
        }
    }
}
