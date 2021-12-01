using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProEventos.API.Models;

namespace ProEventos.API.Extensions
{
    public static class Pagination
    {
        public static void AddPagination(this HttpResponse response,
            int currentPage, int itemsPerPage, int totalItems, int totalPages) 
        {
            var pagHeader = new PaginationHeader(currentPage, totalItems, 
                    itemsPerPage, totalPages);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination", JsonSerializer.Serialize(
                pagHeader, options));
            
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}
