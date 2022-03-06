using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;

namespace Catalogue.InsuranceCompany
{
    [Table("tblCatInsuranceCompany", Schema = "cat")]
    public class InsuranceCompany : Entity
    {
        [Key]
        [Required]
        [Column("IdInsuranceCompany")]
        public int Id { get; set; }

        [Required]
        [Column("InsuranceCompany")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}