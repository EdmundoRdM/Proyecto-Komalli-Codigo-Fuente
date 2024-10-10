using Komalli.DataBaseManagement.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class StaffDAO
    {
        public int RegisterStaff(Staff staff)
        {
            int result = 0;
            using (var context = new KomalliDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.Staffs.Add(staff);
                        result = context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (EntityException ex)
                    {
                        transaction.Rollback();
                        throw new EntityException("Error al acceder a la base de datos.", ex);
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        throw new DbUpdateException("Error al actualizar la base de datos.", ex);
                    }
                }
            }
            return result;
        }
        public int ModifyStaff(Staff staff)
        {
            int result = 0;
            using (var context = new KomalliDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var originalStaff = context.Staffs.FirstOrDefault(s => s.EmployeeNumber == staff.EmployeeNumber);
                        if (originalStaff != null)
                        {
                            originalStaff.FirstName = staff.FirstName;
                            originalStaff.LastName = staff.LastName;
                            originalStaff.MiddleName = staff.MiddleName;
                            originalStaff.Password = staff.Password;
                            originalStaff.Role = staff.Role;

                            result = context.SaveChanges();
                            transaction.Commit();
                        }
                    }
                    catch (EntityException ex)
                    {
                        transaction.Rollback();
                        throw new EntityException("Error al acceder a la base de datos.", ex);
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        throw new DbUpdateException("Error al actualizar la base de datos.", ex);
                    }
                }
            }
            return result;
        }

        public int DeleteStaff(int employeeNumber)
        {
            int result = 0;
            using (var context = new KomalliDBEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var staff = context.Staffs.FirstOrDefault(s => s.EmployeeNumber == employeeNumber);
                        if (staff != null)
                        {
                            context.Staffs.Remove(staff);
                            result = context.SaveChanges();
                            transaction.Commit();
                        }
                    }
                    catch (EntityException ex)
                    {
                        transaction.Rollback();
                        throw new EntityException("Error al acceder a la base de datos.", ex);
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
                        throw new DbUpdateException("Error al actualizar la base de datos.", ex);
                    }
                }
            }
            return result;
        }

        public int LogIn(string username, string password)
        {
            int result = 0;
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var staff = context.Staffs.FirstOrDefault(s => s.FirstName == username && s.Password == password);
                    if (staff != null)
                    {
                        result = 0; 
                    }
                    else
                    {
                        result = 1;                     }
                }
            }
            catch (EntityException ex)
            {
                result = 5;
                throw new EntityException("Error al acceder a la base de datos.", ex);
            }
            catch (InvalidOperationException ex)
            {
                result = 5;
                throw new InvalidOperationException("Error al procesar la solicitud.", ex);
            }
            return result;
        }
    }
}
