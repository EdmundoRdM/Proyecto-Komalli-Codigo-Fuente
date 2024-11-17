using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class RoleDAO
    {
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
