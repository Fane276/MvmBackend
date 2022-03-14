using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;

namespace MvManagement.Extensions.Dto.PageResult
{
    public class PagedResultEnumerableDto<T>:PagedResultDto<T>
    {
        private IEnumerable<T> _query;
        private readonly object _input;
        private int _totalCount;

        public PagedResultEnumerableDto(IEnumerable<T> query): this(query, null)
        {
        }

        public PagedResultEnumerableDto(IEnumerable<T> query, object input)
        {
            _query = query;
            _input = input;
        }

        public PagedResultDto<T> Get()
        {
            _totalCount = _query.Count();
            if (_input is ISortedResultRequest sort)
            {
                _query = _query.OrderBy(sort.Sorting);
            }

            if (_input is IPagedResultRequest pagination)
            {
                if (pagination.MaxResultCount > 0)
                {
                    _query = _query.PageBy(pagination);
                }
            }

            var entities = _query.ToList();
            return new PagedResultDto<T>(_totalCount,entities);
        }
    }
}