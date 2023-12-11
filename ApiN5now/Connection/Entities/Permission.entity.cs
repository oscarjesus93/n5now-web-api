using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Connection.Entities 
{
    [Table("Permissions")]
    public partial class PermissionEntity
    {
        [Key]
        public int Id { get; set; }

        public string? EmployeeForename { get; set; }

        public string? EmployeeSurname { get; set; }

        public int PermissionsType { get; set; }

        public PermissionsTypeEntity PermissionsTypeEntity { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime PermissionsDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }

        public static Action<EntityTypeBuilder<PermissionEntity>> PermissionBuilder = entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07664B7CAE");

            entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");
        };
    }

};

