using Chroomsoft.Top2000.Features;
using CommandDotNet;
using ConsoleApp;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Apps.ConsoleApp.AllEditions
{
    [Command(Name = "Editions")]
    public class EditionsCommand
    {
        private readonly IMediator mediator;

        public EditionsCommand(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [SubCommand]
        public ListCommand List { get; set; } = null!;

        [DefaultMethod]
        public async Task All()
        {
            var editions = await mediator.Send(new AllEditionsRequest());

            foreach (var edition in editions)
            {
                Console.WriteLine(edition.Year);
            }
        }
    }
}
