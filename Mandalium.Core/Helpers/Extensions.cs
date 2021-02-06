using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Mail;
using System.Net;
using Mandalium.Core.Helpers.Pagination;
using NLog;

namespace Mandalium.API.Helpers
{
    public static class Extensions
    {
        public static int ActivationPin { get; set; }
        public static string FromMail { get; set; }
        public static string FromMailPassword { get; set; }     

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
            LogManager.GetLogger("Mandalium Nlog").Error(exception, exception.StackTrace);
        }

        public static void SendMail(string mailTo, string mailSubject, string mailBody, bool bodyhtml)
        {
            var fromAddress = new MailAddress(FromMail, "noreply-mandalium");
            var toAddress = new MailAddress(mailTo, mailTo);
           
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, FromMailPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = mailSubject,
                Body = mailBody,
                IsBodyHtml = bodyhtml
            })
            {
                smtp.Send(message);
            }

        }
    }
}