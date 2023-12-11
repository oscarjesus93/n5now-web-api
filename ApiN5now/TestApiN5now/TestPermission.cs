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

        private readonly Mock<IPermissionService> _permisionService;
        private readonly Mock<ILogger<PermissionsController>> _loggerController;
        private readonly PermissionsController _controller;

        public TestPermission()
        {
            this._permisionService = new Mock<IPermissionService>();
            this._loggerController = new Mock<ILogger<PermissionsController>>();
            this._controller = new PermissionsController(_loggerController.Object, _permisionService.Object);
        }

        [TestMethod]
        public async Task TestListOkPermission()
        {    
            this._permisionService.Setup(m => m.GetAllAsync()).ReturnsAsync(new List<Permission>());
            ActionResult<List<Permission>> result = await this._controller.GetListAsync();
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            OkObjectResult statusCodeResult = (OkObjectResult)result.Result;
            Assert.AreEqual(200, statusCodeResult.StatusCode);
        }

        [TestMethod]
        public async Task TestListPermissionNotFound()
        {            
            this._permisionService.Setup(m => m.GetAllAsync()).ThrowsAsync(new ExceptionCustom("No records found", HttpStatusCode.NotFound));
            ActionResult<List<Permission>> result = await this._controller.GetListAsync();
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
            NotFoundObjectResult statusCodeResult = (NotFoundObjectResult)result.Result;
            Assert.AreEqual(404, statusCodeResult.StatusCode);           
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));
            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("No records found", messageResponse.Message);
        }

        [TestMethod]
        public async Task TestListPermissionBabRequest()
        {
            this._permisionService.Setup(m => m.GetAllAsync()).ThrowsAsync(new ExceptionCustom("Internal server error", HttpStatusCode.BadRequest));
            ActionResult<List<Permission>> result = await this._controller.GetListAsync();
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
            this._permisionService.Setup(m => m.GetAllAsync()).ThrowsAsync(new Exception("Internal server error"));

            ActionResult<List<Permission>> result = await this._controller.GetListAsync();

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

            this._permisionService.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ReturnsAsync(new MessageResponse("Registration completed successfully"));

            ActionResult<MessageResponse> result = await this._controller.PostAsync(permission);

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

            this._permisionService.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("The permission date cannot be less than the current date", HttpStatusCode.BadRequest));

            ActionResult<MessageResponse> result = await this._controller.PostAsync(permission);

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

            this._permisionService.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("The Employeeforeme field cannot be empty", HttpStatusCode.BadRequest));

            ActionResult<MessageResponse> result = await this._controller.PostAsync(permission);

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

            this._permisionService.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("The EmployeeSurname field cannot be empty", HttpStatusCode.BadRequest));

            ActionResult<MessageResponse> result = await this._controller.PostAsync(permission);

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

            this._permisionService.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError));

            ActionResult<MessageResponse> result = await this._controller.PostAsync(permission);

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

            this._permisionService.Setup(m => m.SendPermissionAsync(It.IsAny<PermissionRequest>())).ThrowsAsync(new Exception("Internal server error"));

            ActionResult<MessageResponse> result = await this._controller.PostAsync(permission);

            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            ObjectResult statusCodeResult = (ObjectResult)result.Result;

            Assert.AreEqual(500, statusCodeResult.StatusCode);

            Assert.IsNotNull(statusCodeResult.Value);
            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));

            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("Internal server error", messageResponse.Message);

        }

        [TestMethod]
        public async Task TestMethodPutOk()
        {
            int id_permission = 1;
            PermissionUpdate permission = new PermissionUpdate()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now
            };

            this._permisionService.Setup(m => m.UpdatePermissionAsync(It.IsAny<PermissionUpdate>(), It.IsAny<int>())).ReturnsAsync(new MessageResponse("Update completed successfully"));

            ActionResult<MessageResponse> result = await this._controller.PutAsync(permission, id_permission);

            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            OkObjectResult statusCodeResult = (OkObjectResult)result.Result;

            Assert.AreEqual(200, statusCodeResult.StatusCode);

        }

        [TestMethod]
        public async Task TestMethodNotFoundPut()
        {
            int id_permission = 1;
            PermissionUpdate permission = new PermissionUpdate()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now
            };

            this._permisionService.Setup(m => m.UpdatePermissionAsync(It.IsAny<PermissionUpdate>(), It.IsAny<int>())).ThrowsAsync(new ExceptionCustom("No records found", HttpStatusCode.NotFound));

            ActionResult<MessageResponse> result = await this._controller.PutAsync(permission, id_permission);

            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            ObjectResult statusCodeResult = (ObjectResult)result.Result;

            Assert.AreEqual(404, statusCodeResult.StatusCode);

            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));
            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("No records found", messageResponse.Message);

        }

        [TestMethod]
        public async Task TestMethodPutErrorServerEntityFramerwork()
        {
            int id_permission = 1;
            PermissionUpdate permission = new PermissionUpdate()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now
            };

            this._permisionService.Setup(m => m.UpdatePermissionAsync(It.IsAny<PermissionUpdate>(), It.IsAny<int>())).ThrowsAsync(new ExceptionCustom("Internal server error", HttpStatusCode.InternalServerError));

            ActionResult<MessageResponse> result = await this._controller.PutAsync(permission, id_permission);

            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            ObjectResult statusCodeResult = (ObjectResult)result.Result;

            Assert.AreEqual(500, statusCodeResult.StatusCode);

            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));
            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("Internal server error", messageResponse.Message);


        }

        [TestMethod]
        public async Task TestMethodPutErrorServer()
        {
            int id_permission = 1;
            PermissionUpdate permission = new PermissionUpdate()
            {
                EmployeeForename = "Test Permission Oscar",
                EmployeeSurname = "Test Permission Sanchez",
                PermissionDate = DateTime.Now
            };

            this._permisionService.Setup(m => m.UpdatePermissionAsync(It.IsAny<PermissionUpdate>(), It.IsAny<int>())).ThrowsAsync(new Exception("Internal server error"));

            ActionResult<MessageResponse> result = await this._controller.PutAsync(permission, id_permission);

            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            ObjectResult statusCodeResult = (ObjectResult)result.Result;

            Assert.AreEqual(500, statusCodeResult.StatusCode);

            Assert.IsInstanceOfType(statusCodeResult.Value, typeof(MessageResponse));
            var messageResponse = (MessageResponse)statusCodeResult.Value;
            Assert.AreEqual("Internal server error", messageResponse.Message);
        }

    }
}