using Cinema.Domain.Domain_models;
using Cinema.Service.Interface;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Web.Controllers
{
    [Authorize(Roles = "ADMINISTRATOR")]
    public class OrderController : Controller
    {

        private readonly IOrderService _orderService;

        public OrderController(IOrderService _orderService)
        {
            this._orderService = _orderService;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }

        public IActionResult Index()
        {
            var result = _orderService.GetAllOrders();
            return View(result);
        }

        public IActionResult Details(int id)
        {
            var model = new BaseEntity { Id = id };
            var result = _orderService.GetOrderDetails(model);
            return View(result);

        }

        public IActionResult CreateInvoice(int id)
        {
            var model = new BaseEntity { Id = id };
            var result = _orderService.GetOrderDetails(model);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.Email);

            StringBuilder sb = new StringBuilder();

            var total = 0.0;
            foreach (var item in result.TicketInOrders)
            {
                total += item.Quantity * item.Ticket.Price;
                sb.AppendLine(item.Ticket.MovieName + " with quantity of: " + item.Quantity + " and price of: $" + item.Ticket.Price);
            }

            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{PriceTotal}}", total.ToString());

            var stream = new MemoryStream();
            document.Save(stream, new PdfSaveOptions());
            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        public FileContentResult ExportAllOrders(Guid id)
        {

            List<Order> orders = _orderService.GetAllOrders();

            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workBook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workBook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Customer Name";
                worksheet.Cell(1, 3).Value = "Customer Last Name";
                worksheet.Cell(1, 4).Value = "Customer Email";

                for (int i = 1; i <= orders.Count(); i++)
                {
                    var item = orders[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Name;
                    worksheet.Cell(i + 1, 3).Value = item.User.Surname;
                    worksheet.Cell(i + 1, 4).Value = item.User.Email;

                    for (int t = 1; t <= item.TicketInOrders.Count(); t++)
                    {
                        worksheet.Cell(1, t + 4).Value = "Ticket -" + (t + 1);
                        worksheet.Cell(i + 1, t + 4).Value = item.TicketInOrders.ElementAt(t - 1).Ticket.MovieName;
                    }

                }

                using (var stream = new MemoryStream())
                {
                    workBook.SaveAs(stream);

                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }
            }
        }
    }
}
