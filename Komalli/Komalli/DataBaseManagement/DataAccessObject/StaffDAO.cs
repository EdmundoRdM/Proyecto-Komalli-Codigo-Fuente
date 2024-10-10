using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class StaffDAO
    {
        public int RegisterStaff(Staff staff)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    context.Staffs.Add(staff);
                    context.SaveChanges();
                    return staff.StaffID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el empleado.", ex);
            }
        }

        public int UpdateStaff(Staff staff)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var existingStaff = context.Staffs.FirstOrDefault(s => s.EmployeeNumber == staff.EmployeeNumber);
                    if (existingStaff != null)
                    {
                        existingStaff.FirstName = staff.FirstName;
                        existingStaff.LastName = staff.LastName;
                        existingStaff.MiddleName = staff.MiddleName;
                        existingStaff.Password = staff.Password;
                        existingStaff.Role = staff.Role;
                        existingStaff.Status = staff.Status;
                        context.SaveChanges();
                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el empleado.", ex);
            }
        }

        public int DeleteStaff(string employeeNumber)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var staff = context.Staffs.FirstOrDefault(s => s.EmployeeNumber == employeeNumber);
                    if (staff != null)
                    {
                        context.Staffs.Remove(staff);
                        context.SaveChanges();
                        return 1;
                    }
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el empleado.", ex);
            }
        }

        public StaffPOCO VerifyLogin(StaffPOCO staffPoco)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var staff = context.Staffs
                        .FirstOrDefault(s => s.EmployeeNumber == staffPoco.EmployeeNumber && s.Password == staffPoco.Password);

                    if (staff != null)
                    {
                        return new StaffPOCO
                        {
                            Role = staff.Role,
                            StaffId = staff.StaffID,
                        };
                    }
                    else
                    {
                        return new StaffPOCO
                        {
                            Role = -1 
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar el inicio de sesión.", ex);
            }
        }
    }
}
