using Komalli.DataBaseManagement.DataModel;
using Komalli.DataBaseManagement.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Komalli.DataBaseManagement.DataAccessObject
{
    internal class MovementDAO
    {
        public int RegisterMovement(MovementPOCO movement)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var newMovement = new ExtraordinaryMovements
                    {
                        Amount = movement.Amount,
                        Description = movement.Description,
                        Date = movement.Date,
                        MovementType = movement.MovementType,
                        StaffID = movement.StaffID
                    };

                    context.ExtraordinaryMovements.Add(newMovement);
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar el movimiento.", ex);
            }
        }

        public int UpdateMovement(MovementPOCO movement, int movementID)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var existingMovement = context.ExtraordinaryMovements.FirstOrDefault(m => m.MovementId == movementID);
                    if (existingMovement == null)
                    {
                        throw new Exception("El movimiento no existe.");
                    }
                    existingMovement.Amount = movement.Amount;
                    existingMovement.Description = movement.Description;
                    existingMovement.Date = movement.Date;
                    existingMovement.MovementType = movement.MovementType;
                    existingMovement.StaffID = movement.StaffID;

                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el movimiento.", ex);
            }
        }

        public int DeleteMovement(int movementId)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var movement = context.ExtraordinaryMovements.FirstOrDefault(m => m.MovementId == movementId);
                    if (movement == null)
                    {
                        throw new Exception("El movimiento no existe.");
                    }

                    context.ExtraordinaryMovements.Remove(movement);
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el movimiento.", ex);
            }
        }

        public List<MovementPOCO> GetAllMovements()
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var movements = (from m in context.ExtraordinaryMovements
                                     join mt in context.MovementType on m.MovementType equals mt.MovementTypeId
                                     orderby m.Date descending
                                     select new MovementPOCO
                                     {
                                         MovementId = m.MovementId,
                                         Amount = m.Amount,
                                         Description = m.Description,
                                         Date = m.Date,
                                         MovementType = m.MovementType,
                                         MovementTypeName = mt.MovementType1,
                                         StaffID = m.StaffID
                                     }).ToList();

                    return movements;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los últimos 15 movimientos.", ex);
            }
        }

        public MovementPOCO GetMovementByID(int idMovement)
        {
            try
            {
                using (var context = new KomalliDBEntities())
                {
                    var movement = (from m in context.ExtraordinaryMovements
                                    join mt in context.MovementType on m.MovementType equals mt.MovementTypeId
                                    where m.MovementId == idMovement
                                    select new MovementPOCO
                                    {
                                        MovementId = m.MovementId,
                                        Amount = m.Amount,
                                        Description = m.Description,
                                        Date = m.Date,
                                        MovementType = m.MovementType,
                                        MovementTypeName = mt.MovementType1,
                                        StaffID = m.StaffID
                                    }).FirstOrDefault();

                    if (movement == null)
                    {
                        throw new Exception($"No se encontró un movimiento con el ID {idMovement}.");
                    }

                    return movement;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el movimiento por ID.", ex);
            }
        }
    }
}
