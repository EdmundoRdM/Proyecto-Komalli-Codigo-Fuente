using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.Util
{
    internal class StaffToken
    {
        private static StaffPOCO employeeIdInstance;
        private static readonly object lockObject = new object();

        private StaffToken() { }

        public static void SetEmployeePOCO(StaffPOCO employeeId)
        {
            lock (lockObject)
            {
                employeeIdInstance = employeeId;
                
            }
        }

        public static StaffPOCO GetEmployeePOCO()
        {
            return employeeIdInstance;
        }
    }
}
