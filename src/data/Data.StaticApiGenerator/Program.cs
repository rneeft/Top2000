using Chroomsoft.Top2000.Data;
using Chroomsoft.Top2000.Data.StaticApiGenerator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var services = new ServiceCollection()
    .AddLogging(configure => configure.AddConsole())
    .AddSingleton<ITop2000AssemblyData, Top2000Data>()
    .AddSingleton<ITransformSqlFiles, SqlFileTransformer>()
    .AddSingleton<IFileCreator, FileCreator>()
    .AddSingleton<IConfiguration>(configuration)
    ;

var publishOnlySection = configuration.GetSection("PublishOnly");

if (publishOnlySection.Exists())
{
    services.AddSingleton<IRunApplication, PublishOnlyApplication>();
}
else
{
    services.AddSingleton<IRunApplication, WebServiceApplication>();
}

using var serviceProvider = services.BuildServiceProvider();
var application = serviceProvider.GetRequiredService<IRunApplication>();
await application.RunAsync().ConfigureAwait(false);