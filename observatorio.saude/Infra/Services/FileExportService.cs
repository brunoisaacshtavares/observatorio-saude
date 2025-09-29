using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using ClosedXML.Excel;
using CsvHelper;
using CsvHelper.Configuration;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
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

    public async Task GenerateCsvStreamAsync<T>(IAsyncEnumerable<T> data, Stream outputStream) where T : class
    {
        await using var streamWriter = new StreamWriter(outputStream, new UTF8Encoding(true), leaveOpen: true);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };
        await using var csvWriter = new CsvWriter(streamWriter, config);

        var propsToKeep =
            typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(DisplayAttribute), false))
                .ToArray();

        var map = new DefaultClassMap<T>();
        foreach (var prop in propsToKeep)
        {
            var displayAttribute = prop.GetCustomAttribute<DisplayAttribute>();
            map.Map(typeof(T), prop).Name(displayAttribute?.Name ?? prop.Name);
        }

        csvWriter.Context.RegisterClassMap(map);

        csvWriter.WriteHeader<T>();
        await csvWriter.NextRecordAsync();

        await foreach (var record in data)
        {
            csvWriter.WriteRecord(record);
            await csvWriter.NextRecordAsync();
        }

        await streamWriter.FlushAsync();
    }

    public async Task GenerateXlsxStreamAsync<T>(IAsyncEnumerable<T> data, Stream outputStream) where T : class
    {
        var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<DisplayAttribute>() != null).ToArray();

        await using (var memoryStream = new MemoryStream())
        {
            using (var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = spreadsheetDocument.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                
                var workbookStylesPart = workbookPart.AddNewPart<WorkbookStylesPart>();
                workbookStylesPart.Stylesheet = CreateStylesheet();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                worksheetPart.Worksheet = new Worksheet(sheetData);

                var sheets = spreadsheetDocument.WorkbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet() { Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = typeof(T).Name.Replace("Dto", "") };
                sheets.Append(sheet);

                var headerRow = new Row();
                foreach (var prop in props)
                {
                    var cell = new Cell(new InlineString(new Text(prop.GetCustomAttribute<DisplayAttribute>()?.Name ?? prop.Name)))
                    {
                        DataType = CellValues.InlineString
                    };
                    headerRow.AppendChild(cell);
                }
                sheetData.AppendChild(headerRow);

                await foreach (var item in data)
                {
                    var newRow = new Row();
                    foreach (var prop in props)
                    {
                        var value = prop.GetValue(item);
                        var cell = new Cell();

                        if (value is null)
                        {
                            cell.DataType = CellValues.String;
                            cell.CellValue = new CellValue("");
                        }
                        else
                        {
                            switch (Type.GetTypeCode(value.GetType()))
                            {
                                case TypeCode.DateTime:
                                    cell.CellValue = new CellValue(((DateTime)value));
                                    cell.StyleIndex = 1; 
                                    break;
                                case TypeCode.Int32:
                                case TypeCode.Int64:
                                case TypeCode.Decimal:
                                case TypeCode.Double:
                                case TypeCode.Single:
                                    cell.DataType = CellValues.Number;
                                    cell.CellValue = new CellValue(Convert.ToString(value, CultureInfo.InvariantCulture));
                                    break;
                                default:
                                    cell.DataType = CellValues.String;
                                    cell.CellValue = new CellValue(Convert.ToString(value) ?? "");
                                    break;
                            }
                        }
                        newRow.AppendChild(cell);
                    }
                    sheetData.AppendChild(newRow);
                }
            }

            memoryStream.Position = 0;
            await memoryStream.CopyToAsync(outputStream);
        }
    }
    
    private Stylesheet CreateStylesheet()
    {
        var fonts = new Fonts(new Font(), new Font());
        var fills = new Fills(new Fill());
        var borders = new Borders(new Border());
        
        var numberingFormats = new NumberingFormats(
            new NumberingFormat { NumberFormatId = 164, FormatCode = "dd/mm/yyyy;@" }
        );

        var cellFormats = new CellFormats(
            new CellFormat(), 
            new CellFormat { FontId = 0, FillId = 0, BorderId = 0, NumberFormatId = 164, ApplyNumberFormat = true }
        );

        return new Stylesheet(fonts, fills, borders, numberingFormats, cellFormats);
    }
}