using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;

namespace MvManagement.Extensions.Dto.PageFilter
{
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest, IShouldNormalize
    {
        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "1 ASC";
            }
        }
    }
}