using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class StaffDAO
    {
        public int RegisterStaff(StaffPOCO staff)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var lastStaff = context.Staff
                                            .OrderByDescending(s => s.StaffID)
                                            .FirstOrDefault();

                    string newEmployeeNumber = GenerateEmployeeNumber(lastStaff);

                    var newStaff = new Staff();
                    {
                        newStaff.EmployeeNumber = newEmployeeNumber;
                        newStaff.Password = staff.Password;
                        newStaff.FirstName = staff.FirstName;
                        newStaff.LastName = staff.LastName;
                        newStaff.MiddleName = staff.MiddleName;
                        newStaff.Role = staff.Role;
                        newStaff.Status = true;
                    };

                    context.Staff.Add(newStaff);
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el empleado.", ex);
            }
        }


        public int UpdateStaff(StaffPOCO staff, int idEmployeeSelected)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var existingStaff = context.Staff.FirstOrDefault(s => s.StaffID == idEmployeeSelected);
                    if (existingStaff != null)
                    {
                        existingStaff.FirstName = staff.FirstName;
                        existingStaff.LastName = staff.LastName;
                        existingStaff.MiddleName = staff.MiddleName;
                        existingStaff.Password = staff.Password;
                        existingStaff.Role = staff.Role;
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

        public int DeleteStaff(int idEmployeeSelected)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var existingStaff = context.Staff.FirstOrDefault(s => s.StaffID == idEmployeeSelected);
                    if (existingStaff != null)
                    {
                        existingStaff.Status = false;
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

        public List<StaffWhitRole> GetLastTenStaffRecords()
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var lastTenStaff = (from staff in context.Staff
                                        join role in context.Role on staff.Role equals role.RoleId
                                        where staff.Status == true
                                        orderby staff.EmployeeNumber descending
                                        select new StaffWhitRole
                                        {
                                            IdStaff = staff.StaffID,
                                            EmployeeNumber = staff.EmployeeNumber,
                                            FirstName = staff.FirstName,
                                            LastName = staff.LastName,
                                            RoleName = role.Role1
                                        })
                                        .Take(10)
                                        .ToList();
                    return lastTenStaff;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los últimos 10 registros del personal.", ex);
            }
        }

        public StaffPOCO GetStaffById(int idStaff)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var staffRecord = (from staff in context.Staff
                                       where staff.StaffID == idStaff && staff.Status == true
                                       select new StaffPOCO
                                       {
                                           StaffId = staff.StaffID,
                                           EmployeeNumber = staff.EmployeeNumber,
                                           FirstName = staff.FirstName,
                                           LastName = staff.LastName,
                                           MiddleName = staff.MiddleName,
                                           Role = staff.Role,
                                           Password = staff.Password,
                                           Status = staff.Status
                                       }).FirstOrDefault();
                    return staffRecord;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el registro del personal con ID {idStaff}.", ex);
            }
        }

        public List<StaffWhitRole> GetStaffByName(string partialName)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var staffList = (from staff in context.Staff
                                     join role in context.Role on staff.Role equals role.RoleId
                                     where staff.Status == true &&
                                           staff.FirstName.ToLower().Contains(partialName.ToLower())
                                     orderby staff.EmployeeNumber descending
                                     select new StaffWhitRole
                                     {
                                         IdStaff = staff.StaffID,
                                         EmployeeNumber = staff.EmployeeNumber,
                                         FirstName = staff.FirstName,
                                         LastName = staff.LastName,
                                         RoleName = role.Role1
                                     })
                             .ToList();
                    return staffList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el personal por nombre parcial.", ex);
            }
        }

        public StaffPOCO VerifyLogin(StaffPOCO staffPoco)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var staff = context.Staff
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

        private string GenerateEmployeeNumber(Staff lastStaff)
        {
            try
            {
                int year = DateTime.Now.Year;
                string prefix = "UV" + year;
                int nextNumber = 1;

                if (lastStaff != null)
                {
                    if (string.IsNullOrEmpty(lastStaff.EmployeeNumber) || lastStaff.EmployeeNumber.Length < 6)
                    {
                        throw new ArgumentOutOfRangeException(nameof(lastStaff.EmployeeNumber), "El EmployeeNumber no tiene el formato esperado.");
                    }

                    string lastNumberPart = lastStaff.EmployeeNumber.Substring(6);
                    if (!int.TryParse(lastNumberPart, out nextNumber))
                    {
                        throw new FormatException("El EmployeeNumber no tiene un formato numérico válido.");
                    }
                    nextNumber++;
                }

                string newEmployeeNumber = prefix + nextNumber.ToString("D5");
                return newEmployeeNumber;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
        public List<RolePOCO> GetAllRoles()
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var roleList = context.Role.ToList();
                    var rolePOCOList = roleList.Select(role => new RolePOCO
                    {
                        RoleId = role.RoleId,
                        RoleName = role.Role1
                    }).ToList();

                    return rolePOCOList;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el empleado.", ex);
            }
        }
    }

}
