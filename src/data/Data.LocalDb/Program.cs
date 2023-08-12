using Chroomsoft.Top2000.Data.LocalDb;

var connectionString = @"Server=(localdb)\mssqllocaldb;Database=Top2000;";

EnsureDatabase.For
    .SqlDatabase(connectionString);

var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptEmbeddedInDataLibrary()
    .WithTransactionPerScript()
    .LogToConsole()
    .Build();

var result = upgrader.PerformUpgrade();

if (!result.Successful)
{
    ConsoleLogger.Print(result.Error.ToString(), ConsoleColor.Red, NewLine.Yes);
    return -1;
}

ConsoleLogger.Print("Success!", ConsoleColor.Green, NewLine.Yes);

return 0;