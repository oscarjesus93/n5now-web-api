using ApiN5now.CustomException;
using ApiN5now.DTO;
using ApiN5now.Request;

namespace ApiN5now.Service.IService
{
    public interface IPermissionService
    {

        public Task<List<Permission>> GetAllAsync();

        public Task<MessageResponse> SendPermissionAsync(PermissionRequest permission);

        public Task<MessageResponse> UpdatePermissionAsync(PermissionUpdate permission_update, int permission_id);

    }
}
