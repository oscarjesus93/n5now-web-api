using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Connection.Entities
{
    [Table("PermissionsTypes")]
    public partial class PermissionsTypeEntity
    {
        [Key]
        public int Id { get; set; }

        public string? Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreationDate { get; set; }

        public ICollection<PermissionEntity> Permissions { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModificationDate { get; set; }

        public static Action<EntityTypeBuilder<PermissionsTypeEntity>> PermissionsTypeEntityBuilder = entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07D367EE65");

            entity.Property(e => e.CreationDate).HasDefaultValueSql("(getdate())");
        };

    }

   
}


