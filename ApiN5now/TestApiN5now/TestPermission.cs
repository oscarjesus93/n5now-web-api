using ApiN5now.Controllers;
using ApiN5now.CustomException;
using ApiN5now.CustomExceptions;
using ApiN5now.DTO;
using ApiN5now.Service.IService;
using Connection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;


namespace TestApiN5now
{
    [TestClass]
    public class TestPermission
    {
        [TestMethod]
        public async Task TestListOkPermission()
        {

            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();
            permissionServiceMock.Setup(m => m.GetAllAsync()).ReturnsAsync(new List<Permission>());

            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<List<Permission>> result = await controllerPermission.GetListAsync();

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            OkObjectResult statusCodeResult = (OkObjectResult)result.Result;

            Assert.AreEqual(200, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task TestListPermissionNotFound()
        {
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();
            permissionServiceMock.Setup(m => m.GetAllAsync()).ThrowsAsync(new ExceptionCustom("No records found", HttpStatusCode.NotFound));

            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<List<Permission>> result = await controllerPermission.GetListAsync();

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            NotFoundObjectResult statusCodeResult = (NotFoundObjectResult)result.Result;

            Assert.AreEqual(404, statusCodeResult.StatusCode);

            // Verifica que el contenido es un MessageResponse
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            // Convierte el contenido a un MessageResponse para realizar más aserciones si es necesario
            var messageResponse = (MessageResponse)statusCodeResult.Value;

            // Verifica el mensaje específico del error
            Assert.AreEqual("No records found", messageResponse.Message);
        }

        [TestMethod]
        public async Task TestListPermissionBabRequest()
        {
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();
            permissionServiceMock.Setup(m => m.GetAllAsync()).ThrowsAsync(new ExceptionCustom("Internal server error", HttpStatusCode.BadRequest));

            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<List<Permission>> result = await controllerPermission.GetListAsync();

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            BadRequestObjectResult statusCodeResult = (BadRequestObjectResult)result.Result;

            Assert.AreEqual(400, statusCodeResult.StatusCode);
            
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("Internal server error", messageResponse.Message);
        }

        [TestMethod]
        public async Task TestListPermissionErrorServer()
        {
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();
            permissionServiceMock.Setup(m => m.GetAllAsync()).ThrowsAsync(new Exception("Internal server error"));

            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<List<Permission>> result = await controllerPermission.GetListAsync();

            Assert.IsInstanceOfType(result, typeof(ActionResult<List<Permission>>));

            ObjectResult objectResult = (ObjectResult)result.Result;
            Assert.AreEqual(500, objectResult.StatusCode);

            Assert.IsInstanceOfType(objectResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)objectResult.Value;
            Assert.AreEqual("Internal server error", messageResponse.Message);
        }
    }
}