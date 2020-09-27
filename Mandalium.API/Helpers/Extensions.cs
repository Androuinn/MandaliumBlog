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
                logfile = "../Mandalium.API/Errors/" + "Errors.txt";
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
            catch (Exception)
            {
                throw;
            }
        }
<<<<<<< Updated upstream
=======


        public static void SendMail(string mailSubject, string mailBody)
        {
            var fromAddress = new MailAddress("noreply.mandalium@gmail.com", "noreply-mandalium");
            var toAddress = new MailAddress("noreply.mandalium@gmail.com", "Deneme");
            const string password = "";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = mailSubject,
                Body = mailBody
            })
            {
                smtp.Send(message);
            }

        }
>>>>>>> Stashed changes
    }
}