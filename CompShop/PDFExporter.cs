using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Text;

namespace CompShop.Utils
{
    public static class PdfExporter
    {
        public static void Export(List<object> data, string filePath)
        {
            if (data == null || !data.Any())
                return;

            // Регистрация кодировки для поддержки Windows-1252
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                PdfWriter.GetInstance(document, stream);
                document.Open();

                // Подключаем системный шрифт Arial (или другой, поддерживающий кириллицу)
                string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");
                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12, Font.NORMAL);

                var firstItem = data.First();
                var properties = firstItem.GetType().GetProperties();

                // Таблица с количеством колонок = количеству свойств
                PdfPTable table = new PdfPTable(properties.Length)
                {
                    WidthPercentage = 100
                };

                // Заголовки таблицы
                foreach (var prop in properties)
                {
                    PdfPCell headerCell = new PdfPCell(new Phrase(prop.Name, font))
                    {
                        BackgroundColor = BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(headerCell);
                }

                // Данные
                foreach (var item in data)
                {
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(item)?.ToString() ?? string.Empty;
                        PdfPCell cell = new PdfPCell(new Phrase(value, font))
                        {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        };
                        table.AddCell(cell);
                    }
                }

                document.Add(table);
                document.Close();
            }
        }
    }
}
