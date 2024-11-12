using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LdapAuthorizationService.Common.Extensions
{
    public static class PaginationExtensions
    {
        /// <summary>
        /// Get page count
        /// </summary>
        /// <param name="queryable">Queryable entities</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="token">Cancellation token</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static async Task<int> GetPageCountAsync<T>(this IQueryable<T> queryable, int pageSize, CancellationToken token = default)
        {
            var itemsCount = await queryable.CountAsync(token);
            var pageCount = itemsCount / pageSize;
            if ((pageCount * pageSize) < itemsCount)
                pageCount += 1;

            return pageCount;
        }

        /// <summary>
        /// Paginate entities
        /// </summary>
        /// <param name="queryable">Queryable entities</param>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int pageSize)
        {
            return queryable
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
