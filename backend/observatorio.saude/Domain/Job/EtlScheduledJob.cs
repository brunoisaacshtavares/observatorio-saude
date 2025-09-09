using System.Diagnostics;

namespace observatorio.saude.Domain.Job;

public class EtlScheduledJob(ILogger<EtlScheduledJob> logger, IConfiguration configuration) : BackgroundService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<EtlScheduledJob> _logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalInMinutes = _configuration.GetValue("EtlScheduledJobSettings:IntervalInMinutes", 60);
        using var timer = new PeriodicTimer(TimeSpan.FromMinutes(intervalInMinutes));

        _logger.LogInformation($"Serviço de ETL Agendado está iniciando com o tempo de {intervalInMinutes} minutos.");

        while (await timer.WaitForNextTickAsync(stoppingToken))
            try
            {
                _logger.LogInformation("Iniciando execução do script Python...");

                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.LogError("ConnectionString 'DefaultConnection' não encontrada. Verifique a configuração.");
                    continue;
                }

                var scriptPath = Path.Combine(Directory.GetCurrentDirectory(), "cnes.py");
                var arguments = BuildPythonArguments(scriptPath, connectionString);
                if (string.IsNullOrEmpty(arguments)) continue;

                if (!File.Exists(scriptPath))
                    throw new FileNotFoundException($"O script Python não foi encontrado. {scriptPath}");

                var startInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = arguments,
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
                    _logger.LogError("Script Python falhou (Exit Code: {ExitCode}): {ErrorOutput}", process.ExitCode,
                        error);
                }
                else
                {
                    _logger.LogInformation("Script Python executado com sucesso.");
                    if (!string.IsNullOrWhiteSpace(result))
                        _logger.LogInformation("Saída do script: {ScriptOutput}", result.Trim());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro inesperado ao executar a tarefa de ETL agendada.");
            }
    }

    private string? BuildPythonArguments(string scriptPath, string connectionString)
    {
        try
        {
            var connParams = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(part => part.Split(new[] { '=' }, 2))
                .ToDictionary(
                    split => split[0].Trim().ToLowerInvariant(),
                    split => split[1].Trim()
                );

            var host = connParams.GetValueOrDefault("server") ?? connParams.GetValueOrDefault("host");
            var port = connParams.GetValueOrDefault("port");
            var dbname = connParams.GetValueOrDefault("database");
            var user = connParams.GetValueOrDefault("user id") ?? connParams.GetValueOrDefault("username");
            var password = connParams.GetValueOrDefault("password");

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(dbname) ||
                string.IsNullOrEmpty(user) || password is null)
            {
                _logger.LogError(
                    "A string de conexão é inválida ou está incompleta. Verifique se contém Server, Port, Database, User Id e Password.");
                return null;
            }

            return
                $"\"{scriptPath}\" --host \"{host}\" --port \"{port}\" --dbname \"{dbname}\" --user \"{user}\" --password \"{password}\"";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao processar a string de conexão para gerar os argumentos do Python.");
            return null;
        }
    }
}