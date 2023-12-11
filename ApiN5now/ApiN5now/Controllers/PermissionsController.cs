using ApiN5now.CustomExceptions;
using ApiN5now.DTO;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ApiN5now.CustomException;
using ApiN5now.Service;

namespace ApiN5now.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {

        private readonly PermisionService permisionService;
        private readonly ILogger<PermissionsController> _logger;

        public PermissionsController(ILogger<PermissionsController> logger, PermisionService permision)
        {
            this.permisionService = permision;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Permission>>> GetListAsync()
        {
            try
            {
                List<Permission> list_result = new List<Permission>();
                list_result = await this.permisionService.getListAsync();

                return Ok(list_result);
            }catch(ExceptionCustom ex)
            {
                this._logger.LogError(ex.Message);
                if(ex.code == HttpStatusCode.BadRequest)
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
