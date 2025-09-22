namespace observatorio.saude.Application.Services;

public interface IFileExportService
{
    byte[] GenerateCsv<T>(IEnumerable<T> data) where T : class;
    byte[] GenerateExcel<T>(IEnumerable<T> data) where T : class;

    Task GenerateExcelStreamAsync<T>(IAsyncEnumerable<T> data, Stream outputStream) where T : class;
}