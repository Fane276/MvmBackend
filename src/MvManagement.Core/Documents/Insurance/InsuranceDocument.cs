using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Catalogue.InsuranceCompany;
using JetBrains.Annotations;

namespace MvManagement.Documents.Insurance
{
    [Table("tblInsuranceDocuments", Schema = "doc")]
    public class InsuranceDocument : DatedDocumentAudited
    {
        [Key]
        [Required]
        [Column("IdInsuranceDocument")]
        public long Id { get; set; }

        [Required]
        public InsuranceType InsuranceType { get; set; }
        [Required]
        [MaxLength(250)]
        public string InsurancePolicyNumber { get; set; }

        [CanBeNull]
        [ForeignKey(nameof(InsuranceCompany))]
        public int? IdInsuranceCompany { get; set; }
        public InsuranceCompany InsuranceCompany { get; set; }

    }
}