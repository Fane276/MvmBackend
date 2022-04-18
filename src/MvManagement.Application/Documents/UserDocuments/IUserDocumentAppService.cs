using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using MvManagement.Documents.UserDocuments.Dto;

namespace MvManagement.Documents.UserDocuments
{
    public interface IUserDocumentAppService
    {
        Task<ListResultDto<UserDocumentDto>> GetCurrentUserDocumentsAsync();
        Task<long> AddUserDocumentAsync(UserDocumentInputDto input);
        Task DeleteUserDocumentAsync(long idDocument);
        Task<UserDocumentDto> GetUserDocumentAsync(long idDocument);
        Task UpdateUserDocumentAsync(UserDocumentDto input);
    }
}