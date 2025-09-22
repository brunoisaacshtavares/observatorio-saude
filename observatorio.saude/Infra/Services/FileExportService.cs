using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;
using CsvHelper;
using observatorio.saude.Application.Services;

namespace observatorio.saude.Infra.Services;

public class FileExportService : IFileExportService
{
    public byte[] GenerateCsv<T>(IEnumerable<T> data) where T : class
    {
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream, new UTF8Encoding(true), leaveOpen: true);
        using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var propsToWrite = props.Where(p => p.GetCustomAttribute<DisplayAttribute>() != null).ToArray();

        foreach (var prop in propsToWrite)
            csvWriter.WriteField(prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name);
        csvWriter.NextRecord();

        csvWriter.WriteRecords(data);

        streamWriter.Flush();
        memoryStream.Position = 0;
        return memoryStream.ToArray();
    }

    public byte[] GenerateExcel<T>(IEnumerable<T> data) where T : class
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(typeof(T).Name.Replace("Dto", ""));

        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var propsToWrite = props.Where(p => p.GetCustomAttribute<DisplayAttribute>() != null).ToArray();

        var colIndex = 1;
        foreach (var prop in propsToWrite)
        {
            worksheet.Cell(1, colIndex).Value = prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name;
            colIndex++;
        }

        worksheet.Row(1).Style.Font.Bold = true;

        var rowIndex = 2;
        foreach (var item in data)
        {
            colIndex = 1;
            foreach (var prop in propsToWrite)
            {
                var cellValue = prop.GetValue(item);

                if (cellValue is null)
                    worksheet.Cell(rowIndex, colIndex).SetValue("");
                else
                    worksheet.Cell(rowIndex, colIndex).SetValue((dynamic)cellValue);
                colIndex++;
            }

            rowIndex++;
        }

        worksheet.Columns().AdjustToContents();
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
    
    public async Task GenerateExcelStreamAsync<T>(IAsyncEnumerable<T> data, Stream outputStream) where T : class
    {
        await using var memoryStream = new MemoryStream();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add(typeof(T).Name.Replace("Dto", ""));

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var propsToWrite = props.Where(p => p.GetCustomAttribute<DisplayAttribute>() != null).ToArray();

            var colIndex = 1;
            foreach (var prop in propsToWrite)
            {
                worksheet.Cell(1, colIndex).Value = prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name;
                colIndex++;
            }

            worksheet.Row(1).Style.Font.Bold = true;

            var rowIndex = 2;
            await foreach (var item in data)
            {
                colIndex = 1;
                foreach (var prop in propsToWrite)
                {
                    var cellValue = prop.GetValue(item);

                    if (cellValue is null)
                        worksheet.Cell(rowIndex, colIndex).SetValue("");
                    else
                        worksheet.Cell(rowIndex, colIndex).SetValue((dynamic)cellValue);
                    colIndex++;
                }

                rowIndex++;
            }

            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(memoryStream);
        }

        memoryStream.Position = 0;
        await memoryStream.CopyToAsync(outputStream);
    }
}