using System;
using System.ComponentModel.DataAnnotations;

namespace MvManagement.Documents
{
    public class DatedDocumentAudited<T> : DocumentAudited<T>
    {
        [Required]
        public DateTime ValidFrom { get; set; }
        [Required]
        public DateTime ValidTo { get; set; }
    }

    public class DatedDocumentAudited : DatedDocumentAudited<int>
    {

    }
}