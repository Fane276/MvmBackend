﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace MvManagement.Documents.Dto
{
    public class ExpiredDocumentDto : EntityDto<long>
    {
        public DateTime ValidTo { get; set; }
        public string Name { get; set; }
        public DocumentType DocumentType { get; set; }
        public string VehicleTitle { get; set; }
        public string RegistrationNumber { get; set; }
        public int TenantId { get; set; }
        public long UserId { get; set; }
    }
}