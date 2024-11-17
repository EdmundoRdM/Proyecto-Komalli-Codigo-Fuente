using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Text.RegularExpressions;

namespace Komalli.Util
{
    internal class Utilities
    {
        public static void ChangePage(Page page)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                Frame framePrincipal = mainWindow.FindName("frameContainer") as Frame;
                if (framePrincipal != null)
                {
                    framePrincipal.Navigate(page);
                }
            }
        }
        public static bool IsValidSpanishLettersAndSpaces(string input)
        {
            string pattern = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]*$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            if (match.Success)
            {
                int letterCount = Regex.Matches(input, @"[a-zA-ZáéíóúÁÉÍÓÚñÑ]").Count;
                return letterCount >= 3;
            }

            return false;
        }

        public static bool IsValidSpanishLettersAndSpecialChars(string input)
        {
            string pattern = @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ0-9!@#$%^&*(),.¿?¡;:_\-+=\[\]{}<>/|\\]*$";
            Regex regex = new Regex(pattern);
            Match match = regex.Match(input);
            return match.Success && input.Length >= 5;
        }

    }
}
