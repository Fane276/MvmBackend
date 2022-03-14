using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using IObjectMapper = Abp.ObjectMapping.IObjectMapper;

namespace MvManagement.Extensions.Dto.PageResult
{
    public class PagedResultQueryableMapDto<T> : PagedResultDto<T>
    {
        private IQueryable<T> _query;
        private readonly object _input;
        private int _totalCount;

        private readonly IObjectMapper _objectMapper;

        public PagedResultQueryableMapDto(IQueryable<T> query, IObjectMapper objectMapper) : this(query, null, objectMapper)
        {
            _query = query;
        }

        public PagedResultQueryableMapDto(IQueryable<T> query, object input, IObjectMapper objectMapper)
        {
            _query = query;
            _input = input;
            _objectMapper = objectMapper;
        }


        public async Task<PagedResultDto<T>> GetAsync()
        {
            return await GetAsync<T>();
        }

        public async Task<PagedResultDto<TR>> GetAsync<TR>() 
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

            var typeIn = typeof(T);
            var typeOut = typeof(TR);
            if (typeIn != typeOut)
            {
                var mapped =_objectMapper.Map<IReadOnlyList<TR>>(entities);
                return new PagedResultDto<TR>(_totalCount, mapped);
            }

            return new PagedResultDto<TR>(_totalCount, entities.As<IReadOnlyList<TR>>());
        }
    }
}