namespace ApiN5now.Request
{
    public class PermissionRequest
    {

        public string? EmployeeForename { get; set; }
        public string? EmployeeSurname { get; set; }
        public DateTime PermissionDate { get; set; }
        public int PermissionsType { get; set; }

    }
}
