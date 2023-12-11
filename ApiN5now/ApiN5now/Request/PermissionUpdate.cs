using ApiN5now.DTO;

namespace ApiN5now.Request
{
    public class PermissionUpdate
    {       
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public DateTime PermissionDate { get; set; }
    }
}
