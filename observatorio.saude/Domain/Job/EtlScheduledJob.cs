using System.Diagnostics;

namespace observatorio.saude.Domain.Job;

public class EtlScheduledJob(ILogger<EtlScheduledJob> logger, IConfiguration configuration) : BackgroundService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<EtlScheduledJob> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Serviço de ETL Agendado está iniciando.");

        var intervalInMinutes = _configuration.GetValue("EtlScheduledJobSettings:IntervalInMinutes", 60);

        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(intervalInMinutes));

        while (await timer.WaitForNextTickAsync(stoppingToken))
            try
            {
                _logger.LogInformation("Iniciando execução do script Python...");

                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "cnes.py");

                if (!File.Exists(scriptPath))
                    throw new FileNotFoundException("O script Python não foi encontrado.", scriptPath);

                var startInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = scriptPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(startInfo);

                if (process is null)
                    throw new InvalidOperationException(
                        $"Não foi possível iniciar o processo para o script: {scriptPath}");

                var resultTask = process.StandardOutput.ReadToEndAsync(stoppingToken);
                var errorTask = process.StandardError.ReadToEndAsync(stoppingToken);

                await process.WaitForExitAsync(stoppingToken);

                var result = await resultTask;
                var error = await errorTask;

                if (process.ExitCode != 0)
                {
                    _logger.LogError("Script Python falhou (Exit Code: {ExitCode}): {ErrorOutput}",
                        process.ExitCode,
                        error);
                }
                else
                {
                    _logger.LogInformation("Script Python executado com sucesso.");
                    if (!string.IsNullOrWhiteSpace(result))
                        _logger.LogInformation("Saída do script: {ScriptOutput}", result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado ao executar a tarefa de ETL agendada.");
            }
    }
}