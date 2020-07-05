using Chroomsoft.Top2000.Data.ClientDatabase;
using Chroomsoft.Top2000.Features.AllEditions;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Chroomsoft.Top2000.Specs.Features
{
    [Binding]
    public class AllEditionsSteps
    {
        private ImmutableSortedSet<Edition> editions;

        [Given(@"All data scripts")]
        public async Task GivenAllDataScripts()
        {
            var assemblySource = App.ServiceProvider.GetService<Top2000AssemblyDataSource>();
            var update = App.ServiceProvider.GetService<IUpdateClientDatabase>();

            await update.RunAsync(assemblySource);
        }

        [When(@"the feature is executed")]
        public async Task WhenTheFeatureIsExecuted()
        {
            var request = new AllEditionsRequest();

            var mediator = App.ServiceProvider.GetService<IMediator>();

            editions = await mediator.Send(request);
        }

        [Then(@"a sorted set is returned started with the highest year")]
        public void ThenASortedSetIsReturnedStartedWithTheHighestYear()
        {
            var firstEdition = editions.First();

            foreach (var edition in editions)
            {
                edition.Year.Should().BeLessOrEqualTo(firstEdition.Year);
            }
        }

        [Then(@"the latest year is (.*)")]
        public void ThenTheLatestYearIs(int year)
        {
            var lastestEdition = editions.Last();

            lastestEdition.Year.Should().Be(year);
        }

        [Then(@"the Start- and EndDateTime is in local time")]
        public void ThenTheStart_AndEndDateTimeIsInLocalTime()
        {
            var offset = TimeZoneInfo.Local.BaseUtcOffset;

            foreach (var edition in editions)
            {
                TimeZoneInfo.Local.GetUtcOffset(edition.LocalStartDateAndTime).Should().Be(offset);
                TimeZoneInfo.Local.GetUtcOffset(edition.LocalEndDateAndTime).Should().Be(offset);
            }
        }

        [Then(@"the UTC Statdate is as follow:")]
        public void ThenTheUTCStatdateIsAsFollow(Table table)
        {
            var items = table.CreateSet<YearTimeCombo>();

            foreach (var item in items)
            {
                var edition = editions.Single(x => x.Year == item.Year);

                edition.LocalStartDateAndTime.ToUniversalTime().Should().Be(item.UTCStartdate);
            }
        }

        private class YearTimeCombo
        {
            public int Year { get; set; }

            public DateTime UTCStartdate { get; set; }
        }
    }
}
