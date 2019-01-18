using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DatingApp.Api.Helper
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response,
        IExceptionHandlerFeature error)
        {
            response.Headers.Add("Application-Error", error.Error.Message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
        public static int GetAge(this DateTime dateTime)
        {
            var age = DateTime.Now.Year - dateTime.Year;
            if (DateTime.Now.AddYears(age) > DateTime.Now)
                --age;
            return age;
        }

        public static async Task<PagedList<T>> CreateAsync<T>(
          this IQueryable<T> source, int pageNumber, int pageSize)
        => await PagedList<T>.CreateAsync(source, pageNumber, pageSize);

        public static void AddPagination(this HttpResponse response,
        int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaganationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader,camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");

        }

    }
}