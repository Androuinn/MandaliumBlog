using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Mandalium.API.Helpers
{
    public static class Extensions
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }



        public static void ReportError(Exception exception)
        {
            string logfile = String.Empty;
            try
            {
                logfile = "result.txt";

                if (!File.Exists(logfile))
                {
                    using (var writer = new StreamWriter(logfile, true))
                    {
                        writer.WriteLine(
                            "=>{0} An Error occurred: {1}  Message: {2}{3}",
                            DateTime.Now,
                            exception.StackTrace,
                            exception.Message,
                            Environment.NewLine
                            );
                    }
                }
                else
                {
                    using (var writer = new StreamWriter(logfile, true))
                    {
                        writer.WriteLine(
                            "=>{0} An Error occurred: {1}  Message: {2}{3}",
                            DateTime.Now,
                            exception.StackTrace,
                            exception.Message,
                            Environment.NewLine
                            );
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}