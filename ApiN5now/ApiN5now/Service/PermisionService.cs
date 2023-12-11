using ApiN5now.DTO;
using Connection;
using Connection.Entities;
using Microsoft.EntityFrameworkCore;
using ApiN5now.CustomExceptions;
using System.Data.Entity.Core;
using System.Net;
using Microsoft.Extensions.Caching.Memory;

namespace ApiN5now.Service
{
    public class PermisionService
    {
        private readonly ILogger<PermisionService> _logger;
        private readonly DbSet<PermissionEntity> permissionEntities;
        private readonly AppDbContext context;

        public PermisionService( AppDbContext _context, ILogger<PermisionService> logger)
        {
            this.context = _context;
            //this.permissionEntities = context.permissionsEntities;
            _logger = logger;

        }

        public async Task<List<Permission>> getListAsync()
        {
            try
            {
                List<Permission> list = new List<Permission>();
                List<PermissionEntity> result = await this.context.permissionsEntities.Include(c => c.PermissionsTypeEntity).ToListAsync();

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
    }
}
