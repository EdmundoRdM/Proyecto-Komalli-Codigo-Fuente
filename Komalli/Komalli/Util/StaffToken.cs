using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.Util
{
    internal class StaffToken
    {
        private static int employeeIdInstance;
        private static readonly object lockObject = new object();

        private StaffToken() { }

        public static void SetEmployeeID(int employeeId)
        {
            lock (lockObject)
            {
                employeeIdInstance = employeeId;
            }
        }

        public static int? GetEmployeeID()
        {
            return employeeIdInstance;
        }
    }
}
