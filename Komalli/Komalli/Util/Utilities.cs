using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Komalli.Util
{
    internal class Utilities
    {
        public static void ChangePage(Page page)
        {
            // Obtener la ventana principal (MainWindow) de la aplicación
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                // Buscar el Frame por su nombre
                Frame framePrincipal = mainWindow.FindName("frameContainer") as Frame;
                if (framePrincipal != null)
                {
                    // Navegar a la página especificada
                    framePrincipal.Navigate(page);
                }
            }
        }

    }
}
