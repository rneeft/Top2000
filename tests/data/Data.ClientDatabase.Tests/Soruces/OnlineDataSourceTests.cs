using System;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Chroomsoft.Top2000.Data.ClientDatabase.Sources;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace Chroomsoft.Top2000.Data.ClientDatabase.Tests.Soruces
{
    [TestClass]
    public class OnlineDataSourceTests
    {
        private Mock<IHttpClientFactory> factory;
        private OnlineDataSource sut;
        private Mock<HttpMessageHandler> messageMock;

        [TestInitialize]
        public void TestInitialize()
        {
            factory = new Mock<IHttpClientFactory>();
            sut = new OnlineDataSource(factory.Object);
            messageMock = new Mock<HttpMessageHandler>();
        }

        [TestMethod]
        public async Task Without_journals_this_online_source_should_not_be_used_thus_no_execuable_script_are_returned()
        {
            var noJournals = ImmutableSortedSet<string>.Empty;

            var scripts = await sut.ExecutableScriptsAsync(noJournals);

            scripts.Should().BeEmpty();
        }

        [TestMethod]
        public async Task Upon_error_execuabel_scripts_is_empty()
        {
            var journals = Create.ImmutableSortedSetFrom("001-Script.sql");

            using var response = new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Content = new StringContent("ERROR") };
            using var httpClient = SetupMocksWithResponse(response);
            factory.Setup(x => x.CreateClient("top2000")).Returns(httpClient);
            var scripts = await sut.ExecutableScriptsAsync(journals);

            scripts.Should().BeEmpty();
        }

        [TestMethod]
        public async Task The_last_journal_is_used_to_call_Top2000_api()
        {
            var journals = Create.ImmutableSortedSetFrom("001-Script.sql", "002-Script.sql");
            var upgrades = new[] { "003-Script3.sql" };
            var content = JsonConvert.SerializeObject(upgrades);

            using var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(content) };
            using var httpClient = SetupMocksWithResponse(response);
            factory.Setup(x => x.CreateClient("top2000")).Returns(httpClient);

            var scripts = await sut.ExecutableScriptsAsync(journals);

            var expectedUri = new Uri("http://unittest:2000/api/versions/002/upgrades");

            messageMock.Protected()
                .Verify("SendAsync", Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get &&
                    req.RequestUri == expectedUri),
                ItExpr.IsAny<CancellationToken>());

            scripts.Should().BeEquivalentTo(upgrades);
        }

        [TestMethod]
        public async Task Script_is_retrieved_from_data_endpoint()
        {
            using var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent("CREATE TABLE table(Id INT NOT NULL);") };
            using var httpClient = SetupMocksWithResponse(response);
            factory.Setup(x => x.CreateClient("top2000")).Returns(httpClient);

            var script = await sut.ScriptContentsAsync("002-Script2.sql");

            var expectedUri = new Uri("http://unittest:2000/data/002-Script2.sql");

            messageMock.Protected()
               .Verify("SendAsync", Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req =>
                   req.Method == HttpMethod.Get &&
                   req.RequestUri == expectedUri),
               ItExpr.IsAny<CancellationToken>());

            using (new AssertionScope())
            {
                script.ScriptName.Should().Be("002-Script2.sql");
                script.Contents.Should().Be("CREATE TABLE table(Id INT NOT NULL);");
            }
        }

        private HttpClient SetupMocksWithResponse(HttpResponseMessage response)
        {
            messageMock.Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response)
               .Verifiable();

            return new HttpClient(messageMock.Object)
            {
                BaseAddress = new Uri("http://unittest:2000/")
            };
        }
    }
}