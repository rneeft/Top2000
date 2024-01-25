using Chroomsoft.Top2000.Features.AllListingsOfEdition;
using CommandDotNet;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp
{
    [Command(Name = "List")]
    public class ListCommand
    {
        private readonly IMediator mediator;

        public ListCommand(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [DefaultMethod]
        public async Task List(int year)
        {
            var tracks = (await mediator.Send(new AllListingsOfEditionRequest(year)).ConfigureAwait(false))
                .OrderBy(x => x.Position)
                .ToList();

            foreach (var track in tracks)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("{0,4}", track.Position);
                Console.Write(" ");
                Console.WriteLine(track.Title);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"     {track.Artist}");

                Console.ResetColor();
            }
        }
    }
}
