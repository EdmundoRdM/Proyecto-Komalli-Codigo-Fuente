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
    /// Lógica de interacción para CashierLanding.xaml
    /// </summary>
    public partial class CashierLanding : Page
    {
        public CashierLanding()
        {
            InitializeComponent();
        }

        public void ShowMenuForm()
        {
            MenuForm.Visibility = Visibility.Visible;
            DefaultBackground.Visibility = Visibility.Hidden;
        }

    }
}
