using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Infrastructure;
using ApiSitemaClinico.Clinic.Domain.Entities;

namespace ApiSitemaClinico.Clinic.Application.Features.Payments
{
  public static class PdfGenerator
  {
    public static byte[] GeneratePaymentReceipt(Payment payment)
    {
      var byteArray = Document.Create(container =>
      {
        container.Page(page =>
        {
          page.Size(PageSizes.A4);
          page.Margin(20);
          page.DefaultTextStyle(x => x.FontSize(12));

          page.Header().Row(row =>
          {
            row.RelativeItem().Column(col =>
            {
              col.Item().Text("Clinic Name").Bold().FontSize(18);
              col.Item().Text("Address Line 1");
              col.Item().Text("Phone: 123456789");
            });
            row.ConstantItem(100).Height(50).Placeholder();
          });

          page.Content().PaddingVertical(10).Column(col =>
          {
            col.Item().Text($"Receipt #{payment.Id}").Bold();
            col.Item().Text($"Date: {payment.Date:yyyy-MM-dd HH:mm}");
            col.Item().Text($"PatientId: {payment.PatientId}");
            col.Item().BorderBottom(1).PaddingBottom(5);

            col.Item().Table(table =>
            {
              table.ColumnsDefinition(columns =>
              {
                columns.RelativeColumn(6);
                columns.RelativeColumn(2);
                columns.RelativeColumn(2);
              });

              table.Header(header =>
              {
                header.Cell().Element(CellStyle).Text("Concept");
                header.Cell().Element(CellStyle).AlignRight().Text("Qty");
                header.Cell().Element(CellStyle).AlignRight().Text("Subtotal");

                static IContainer CellStyle(IContainer container)
                {
                  return container.DefaultTextStyle(x => x.SemiBold()).Padding(5);
                }
              });

              foreach (var d in payment.Details)
              {
                table.Cell().Element(CellStyle).Text(d.Concept);
                table.Cell().Element(CellStyle).AlignRight().Text(d.Quantity.ToString());
                table.Cell().Element(CellStyle).AlignRight().Text(d.Subtotal.ToString("C"));

                static IContainer CellStyle(IContainer container)
                {
                  return container.Padding(5);
                }
              }
            });

            col.Item().BorderBottom(1).PaddingBottom(5);
            col.Item().AlignRight().Text($"Total: {payment.Total:C}").Bold();
            col.Item().AlignRight().Text($"Paid: {payment.PaidAmount:C}");
            col.Item().AlignRight().Text($"Status: {payment.Status}");
          });

          page.Footer().AlignCenter().Text(text =>
          {
            text.Span("Thank you for your payment.");
          });
        });
      }).GeneratePdf();

      return byteArray;
    }
  }
}
