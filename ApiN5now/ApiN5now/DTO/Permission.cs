using Connection.Entities;

namespace ApiN5now.DTO
{
    public class Permission
    {
        public int id {  get; set; }
        public string EmployeeForename { get; set; }
        public string EmployeeSurname { get; set; }
        public DateTime PermissionDate { get; set; }
        public PermissionType PermissionType { get; set; }

        public Permission()
        {
            
        }

        public void PermissionEntityToPermission(PermissionEntity entity)
        {
            this.id = entity.Id;
            this.EmployeeForename = entity.EmployeeForename;
            this.EmployeeSurname = entity.EmployeeSurname;
            this.PermissionDate = entity.PermissionsDate;
            this.PermissionType = new PermissionType() { 
                id = entity.PermissionsTypeEntity.Id,
                description = entity.PermissionsTypeEntity.Description
            };
        }

        public PermissionEntity PErmissionToEntity()
        {
            return new PermissionEntity()
            {
                Id = this.id,
                EmployeeForename = this.EmployeeForename,
                EmployeeSurname = this.EmployeeSurname,
                PermissionsDate = this.PermissionDate,
                PermissionsType = this.PermissionType.id
            };
        }

    }
}
