using ApiN5now.Controllers;
using ApiN5now.CustomException;
using ApiN5now.CustomExceptions;
using ApiN5now.DTO;
using ApiN5now.Request;
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

            // Convierte el contenido a un MessageResponse para realizar m�s aserciones si es necesario
            var messageResponse = (MessageResponse)statusCodeResult.Value;

            // Verifica el mensaje espec�fico del error
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

        [TestMethod]
        public async Task TestMethodPostOk()
        {
            PermissionRequest permission = new PermissionRequest()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now,
                PermissionsType = 1
            };
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();
            permissionServiceMock.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ReturnsAsync(new MessageResponse("Registration completed successfully"));

            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<MessageResponse> result = await controllerPermission.PostAsync(permission);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            OkObjectResult statusCodeResult = (OkObjectResult)result.Result;

            Assert.AreEqual(200, statusCodeResult.StatusCode);

        }

        [TestMethod]
        public async Task TestMethodPostBabRequestPermissionDate()
        {
            PermissionRequest permission = new PermissionRequest()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now.AddDays(-1),
                PermissionsType = 1
            };
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();

            permissionServiceMock.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("The permission date cannot be less than the current date", HttpStatusCode.BadRequest));


            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<MessageResponse> result = await controllerPermission.PostAsync(permission);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            BadRequestObjectResult statusCodeResult = (BadRequestObjectResult)result.Result;

            Assert.AreEqual(400, statusCodeResult.StatusCode);

            Assert.IsNotNull(statusCodeResult.Value);
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("The permission date cannot be less than the current date", messageResponse.Message);

        }

        [TestMethod]
        public async Task TestMethodPostBabRequestEmployeeForename()
        {
            PermissionRequest permission = new PermissionRequest()
            {
                EmployeeForename = "",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now,
                PermissionsType = 1
            };
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();

            permissionServiceMock.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("The Employeeforeme field cannot be empty", HttpStatusCode.BadRequest));


            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<MessageResponse> result = await controllerPermission.PostAsync(permission);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            BadRequestObjectResult statusCodeResult = (BadRequestObjectResult)result.Result;

            Assert.AreEqual(400, statusCodeResult.StatusCode);

            Assert.IsNotNull(statusCodeResult.Value);
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("The Employeeforeme field cannot be empty", messageResponse.Message);

        }

        [TestMethod]
        public async Task TestMethodPostBabRequestEmployeeSurname()
        {
            PermissionRequest permission = new PermissionRequest()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "",
                PermissionDate = DateTime.Now,
                PermissionsType = 1
            };
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();

            permissionServiceMock.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("The EmployeeSurname field cannot be empty", HttpStatusCode.BadRequest));


            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<MessageResponse> result = await controllerPermission.PostAsync(permission);

            Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
            BadRequestObjectResult statusCodeResult = (BadRequestObjectResult)result.Result;

            Assert.AreEqual(400, statusCodeResult.StatusCode);

            Assert.IsNotNull(statusCodeResult.Value);
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("The EmployeeSurname field cannot be empty", messageResponse.Message);

        }

        [TestMethod]
        public async Task TestMethodPostErrorServerEntityFramerwork()
        {
            PermissionRequest permission = new PermissionRequest()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now,
                PermissionsType = 1
            };
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();

            permissionServiceMock.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError));


            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<MessageResponse> result = await controllerPermission.PostAsync(permission);

            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            ObjectResult statusCodeResult = (ObjectResult)result.Result;

            Assert.AreEqual(500, statusCodeResult.StatusCode);

            Assert.IsNotNull(statusCodeResult.Value);
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("Internal server error", messageResponse.Message);

        }

        [TestMethod]
        public async Task TestMethodPostErrorServer()
        {
            PermissionRequest permission = new PermissionRequest()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now,
                PermissionsType = 1
            };
            Mock<IPermissionService> permissionServiceMock = new Mock<IPermissionService>();

            permissionServiceMock.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new Exception("Internal server error"));


            Mock<ILogger<PermissionsController>> loggerMock = new Mock<ILogger<PermissionsController>>();
            PermissionsController controllerPermission = new PermissionsController(loggerMock.Object, permissionServiceMock.Object);

            ActionResult<MessageResponse> result = await controllerPermission.PostAsync(permission);

            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            ObjectResult statusCodeResult = (ObjectResult)result.Result;

            Assert.AreEqual(500, statusCodeResult.StatusCode);

            Assert.IsNotNull(statusCodeResult.Value);
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("Internal server error", messageResponse.Message);

        }



    }
}