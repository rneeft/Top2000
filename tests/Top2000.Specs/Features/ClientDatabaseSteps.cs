using Chroomsoft.Top2000.Data;
using Chroomsoft.Top2000.Data.ClientDatabase;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Chroomsoft.Top2000.Specs.Features
{
    public class Top2000AssemblyDataSourceSpecDecorator : ISource
    {
        private readonly Top2000AssemblyDataSource component;
        private readonly int skip;

        public Top2000AssemblyDataSourceSpecDecorator(Top2000AssemblyDataSource component, int skip)
        {
            this.component = component;
            this.skip = skip;
        }

        public async Task<ImmutableSortedSet<string>> ExecutableScriptsAsync(ImmutableSortedSet<string> journals)
        {
            return (await component.ExecutableScriptsAsync(journals))
                .SkipLast(skip)
                .ToImmutableSortedSet();
        }

        public Task<SqlScript> ScriptContentsAsync(string scriptName)
        {
            return component.ScriptContentsAsync(scriptName);
        }
    }

    [Binding]
    public class ClientDatabaseSteps
    {
        [BeforeScenario]
        public void DeleteClientDatabase()
        {
            if (File.Exists(App.DatabasePath))
                File.Delete(App.DatabasePath);
        }

        [AfterScenario]
        public void CloseDatabaseConnections()
        {
            SQLite.SQLiteAsyncConnection.ResetPool();
        }

        [Given(@"A new install of the application")]
        public void GivenANewInstallOfTheApplication()
        {
        }

        [When(@"the application starts")]
        public async Task WhenIStartTheApplication()
        {
            var assemblySource = App.ServiceProvider.GetService<Top2000AssemblyDataSource>();
            var update = App.ServiceProvider.GetService<IUpdateClientDatabase>();

            await update.RunAsync(assemblySource);
        }

        [Given(@"an installed application without the last (.*) SQL scripts")]
        public async Task GivenAnInstalledApplicationWithoutTheLastSQLScripts(int skipLast)
        {
            var assemblySource = App.ServiceProvider.GetService<Top2000AssemblyDataSource>();
            var update = App.ServiceProvider.GetService<IUpdateClientDatabase>();
            var specSourceDecorator = new Top2000AssemblyDataSourceSpecDecorator(assemblySource, skipLast);

            await update.RunAsync(specSourceDecorator);
        }

        [Then(@"the client database is created with the scripts from the top2000 data assembly")]
        public async Task ThenTheClientDatabaseIsCreatedWithTheScriptsFromTheTopDataAssembly()
        {
            var database = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var top2000AssemblyData = App.ServiceProvider.GetService<ITop2000AssemblyData>();

            var scripts = (await database.Table<Journal>().ToListAsync())
                .Select(x => x.ScriptName)
                .ToList();

            var expected = top2000AssemblyData.GetAllSqlFiles()
                .ToList();

            scripts.Should().BeEquivalentTo(expected);
        }

        [When(@"the application starts without the last SQL scripts")]
        public void WhenTheApplicationStartsWithoutTheLastSQLScripts()
        {
            // nothing to do here since the client database is already
            // installed with the last SQL scripts
        }

        [Then(@"the application checks online for updates")]
        public async Task ThenTheApplicationChecksOnlineForUpdates()
        {
            var onlineSource = App.ServiceProvider.GetService<OnlineDataSource>();
            var update = App.ServiceProvider.GetService<IUpdateClientDatabase>();

            await update.RunAsync(onlineSource);
        }

        [Then(@"the application updates the second-to-last script from the assembly")]
        public async Task ThenTheApplicationUpdatesTheSecond_To_LastScriptFromTheAssembly()
        {
            var assemblySource = App.ServiceProvider.GetService<Top2000AssemblyDataSource>();
            var update = App.ServiceProvider.GetService<IUpdateClientDatabase>();
            var specSourceDecorator = new Top2000AssemblyDataSourceSpecDecorator(assemblySource, 1);

            await update.RunAsync(specSourceDecorator);
        }

        [Then(@"the client database is updated")]
        public async Task ThenTheClientDatabaseIsUpdated()
        {
            // since the data on the website must be the same as on the Assembly
            // we can assert here

            var database = App.ServiceProvider.GetService<SQLiteAsyncConnection>();
            var top2000AssemblyData = App.ServiceProvider.GetService<ITop2000AssemblyData>();

            var scripts = (await database.Table<Journal>().ToListAsync())
                .Select(x => x.ScriptName)
                .ToList();

            var expected = top2000AssemblyData.GetAllSqlFiles()
                .ToList();

            scripts.Should().BeEquivalentTo(expected);
        }
    }
}
