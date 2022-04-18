﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using MvManagement.Documents.UserDocuments.Dto;
using MvManagement.Extensions;

namespace MvManagement.Documents.UserDocuments
{
    [RemoteService(IsEnabled = false, IsMetadataEnabled = false)]
    public class UserDocumentAppService : MvManagementAppServiceBase, IUserDocumentAppService
    {
        private readonly IRepository<UserDocument, long> _userDocumentRepository;

        public UserDocumentAppService(IRepository<UserDocument, long> userDocumentRepository)
        {
            _userDocumentRepository = userDocumentRepository;
        }

        private async Task<List<UserDocumentDto>> GetAllUserDocumentsAsync(long idUser)
        {
            return await _userDocumentRepository.GetAll()
                .Where(d => d.UserId == idUser)
                .Select(x => ObjectMapper.Map<UserDocumentDto>(x))
                .ToListAsync();
        }

        public async Task<ListResultDto<UserDocumentDto>> GetCurrentUserDocumentsAsync()
        {

            var currentUserId = AbpSession.UserId;
            var listOfDocuments = await GetAllUserDocumentsAsync((long) currentUserId);

            return new ListResultDto<UserDocumentDto>(listOfDocuments);
        }

        public async Task<long> AddUserDocumentAsync(UserDocumentInputDto input)
        {
            if (AbpSession.TenantId == null)
            {
                throw new UserFriendlyException("Not authorized!");
            }

            if (input.UserId == null)
            {
                input.UserId = AbpSession.UserId;
            }

            var entity = ObjectMapper.Map<UserDocument>(input);
            entity.TenantId = (int)AbpSession.TenantId;

            var docId = await _userDocumentRepository.InsertAndGetIdAsync(entity);

            return docId;
        }

        public async Task UpdateUserDocumentAsync(UserDocumentDto input)
        {
            var entity = ObjectMapper.Map<UserDocument>(input);

            await _userDocumentRepository.UpdateAsync(entity);
        }
        public async Task<UserDocumentDto> GetUserDocumentAsync(long idDocument)
        {
            var document = await _userDocumentRepository.FirstOrDefaultAsync(d => d.Id == idDocument);
            return ObjectMapper.Map<UserDocumentDto>(document);
        }

        public async Task DeleteUserDocumentAsync(long idDocument)
        {
            await _userDocumentRepository.DeleteAsync(d => d.Id == idDocument);
        }
    }
}