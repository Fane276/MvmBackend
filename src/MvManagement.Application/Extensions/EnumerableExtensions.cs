using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;

namespace MvManagement.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> PageBy<T>(this IEnumerable<T> query, int skipCount, int maxResultCount)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            return query.Skip(skipCount).Take(maxResultCount);
        }

        public static IEnumerable<T> PageBy<T>(this IEnumerable<T> query, IPagedResultRequest pagedResultRequest)
        {
            return query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);
        }
    }
}