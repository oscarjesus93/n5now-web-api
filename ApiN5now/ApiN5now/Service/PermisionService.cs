using ApiN5now.DTO;
using Connection;
using Connection.Entities;
using Microsoft.EntityFrameworkCore;
using ApiN5now.CustomExceptions;
using System.Data.Entity.Core;
using System.Net;
using Microsoft.Extensions.Caching.Memory;
using ApiN5now.Service.IService;
using ApiN5now.CustomException;
using ApiN5now.Request;

namespace ApiN5now.Service
{
    public class PermisionService : IPermissionService
    {
        private readonly ILogger<PermisionService> _logger;
        private readonly DbSet<PermissionEntity> permissionEntities;
        private readonly AppDbContext context;

        public PermisionService( AppDbContext _context, ILogger<PermisionService> logger)
        {
            this.context = _context;
            this.permissionEntities = _context.permissionsEntities;
            this._logger = logger;
        }

        public async Task<List<Permission>> GetAllAsync()
        {
            try
            {
                List<Permission> list = new List<Permission>();
                List<PermissionEntity> result = await this.permissionEntities.Include(c => c.PermissionsTypeEntity).ToListAsync();

                if (result.Count > 0)
                {
                    foreach (PermissionEntity entity in result)
                    {
                        Permission permission = new Permission();
                        permission.PermissionEntityToPermission(entity);
                        list.Add(permission);
                    }
                }
                else
                {
                    throw new ExceptionCustom("No records found", HttpStatusCode.BadRequest);
                }

                return list;
            }
            catch (EntityException ex)
            {
                this._logger.LogError(ex.Message);
                throw new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                throw new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MessageResponse> SendPermissionAsync(PermissionRequest permission)
        {
            try
            {
                Permission permission_dto = new Permission(permission.EmployeeForename, permission.EmployeeSurname, permission.PermissionDate, permission.PermissionsType);
                PermissionEntity entity = permission_dto.PErmissionToEntity();
                
                await this.context.permissionsEntities.AddAsync(entity);

                await this.context.SaveChangesAsync();

                return new MessageResponse("Registration completed successfully");
            }
            catch (EntityException ex)
            {
                this._logger.LogError(ex.Message);
                throw new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                throw new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<MessageResponse> UpdatePermissionAsync(PermissionUpdate permission_update, int permission_id)
        {
            try
            {
                PermissionEntity permission = await this.context.permissionsEntities.FirstOrDefaultAsync(c => c.Id == permission_id);

                if (permission == null)
                    throw new ExceptionCustom("No records found", HttpStatusCode.NotFound);

                permission.EmployeeForename = permission_update.EmployeeForename;
                permission.EmployeeSurname = permission_update.EmployeeSurname;
                permission.PermissionsDate = permission_update.PermissionDate;
                permission.ModificationDate = DateTime.Now;

                await this.context.SaveChangesAsync();

                return new MessageResponse("Update completed successfully");
                
            }
            catch (EntityException ex)
            {
                this._logger.LogError(ex.Message);
                throw new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                throw new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError);
            }
        }

        
    }
}
