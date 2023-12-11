using ApiN5now.CustomExceptions;
using ApiN5now.DTO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ApiN5now.CustomException;
using ApiN5now.Service;
using ApiN5now.Service.IService;
using ApiN5now.Request;
using System.ComponentModel.DataAnnotations;

namespace ApiN5now.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {

        private readonly IPermissionService permisionService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(ILogger<PermissionsController> logger, IPermissionService permissionService)
        {
            this.permisionService = permissionService;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Permission>>> GetListAsync()
        {
            try
            {
                List<Permission> list_result = new List<Permission>();
                list_result = await this.permisionService.GetAllAsync();

                return Ok(list_result);
            }catch(ExceptionCustom ex)
            {
                this._logger.LogError(ex.Message);
                if(ex.code == HttpStatusCode.NotFound)
                    return NotFound(new MessageResponse(ex.Message));

                return BadRequest(new MessageResponse(ex.Message));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                return StatusCode(500, new MessageResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<ActionResult<MessageResponse>> PostAsync([FromBody] PermissionRequest permission)
        {
            try
            {
                return Ok(await this.permisionService.SendPermissionAsync(permission));
            }
            catch (ExceptionCustom ex)
            {
                this._logger.LogError(ex.Message);
                if (ex.code == HttpStatusCode.BadRequest)
                    return BadRequest(new MessageResponse(ex.Message));

                return StatusCode(500, new MessageResponse(ex.Message));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                return StatusCode(500, new MessageResponse(ex.Message));
            }
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<MessageResponse>> PutAsync([FromBody] PermissionUpdate permission, [Required] int id)
        {
            try
            {
                return Ok(await this.permisionService.UpdatePermissionAsync(permission, id));
            }
            catch (ExceptionCustom ex)
            {
                this._logger.LogError(ex.Message);
                if (ex.code == HttpStatusCode.BadRequest)
                    return BadRequest(new MessageResponse(ex.Message));

                return StatusCode(500, new MessageResponse(ex.Message));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
                return StatusCode(500, new MessageResponse(ex.Message));
            }
        }
    }
}
