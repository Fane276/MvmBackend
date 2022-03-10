using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Abp.Events.Bus;
using MvManagement.Authorization.Users;

namespace MvManagement.VehicleData.VehicleAccess
{
    [Table("tblVehicleRoleUser", Schema = "veh")]
    public class VehicleRoleUser : Entity<long>, ICreationAudited
    {
        [Required]
        [Column("Id")]
        public override long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public long? CreatorUserId { get; set; }
        [Required]
        [Column("IdVehicleRole")]
        public int IdRole { get; set; }
        [Required]
        [Column("IdVehicle")]
        public long IdVehicle { get; set; }


        [ForeignKey(nameof(User))]
        [Column("UserId")]
        public long? UserId { get; set; }
        public User User { get; set; }
    }
}