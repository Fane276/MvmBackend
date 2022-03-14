using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MvManagement.Extensions.Dto.PageResult
{
    public class PagedResultQueryableDto<T>: PagedResultDto<T>
    {
        private IQueryable<T> _query;
        private readonly object _input;
        private int _totalCount;

        public PagedResultQueryableDto(IQueryable<T> query):this(query,null)
        {
            _query = query;
        }

        public PagedResultQueryableDto(IQueryable<T> query, object input)
        {
            _query = query;
            _input = input;
        }

        public async Task<PagedResultDto<T>> GetAsync()
        {
            _totalCount = await _query.CountAsync();
            if (_input is ISortedResultRequest sort)
            {
                if (!sort.Sorting.IsNullOrEmpty())
                {
                    _query = _query.OrderBy(sort.Sorting);
                }
            }

            if (_input is IPagedResultRequest pagination)
            {
                if (pagination.MaxResultCount > 0)
                {
                    _query = (IQueryable<T>) _query.PageBy(pagination);
                }
            }

            var entities = await _query.ToListAsync();
            return new PagedResultDto<T>(_totalCount, entities);
        }
    }
}