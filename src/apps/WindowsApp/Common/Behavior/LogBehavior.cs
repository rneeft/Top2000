#nullable enable

using MediatR;
using Microsoft.AppCenter.Analytics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.WindowsApp.Common.Behavior
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var watch = Stopwatch.StartNew();

            var response = await next();

            watch.Stop();

            var properties = GetAdditionalLoggingProperties(request, watch, response);
            Analytics.TrackEvent(request.GetType().Name, properties);

            return response;
        }

        private static Dictionary<string, string> GetAdditionalLoggingProperties(TRequest request, Stopwatch watch, TResponse response)
        {
            var properties = new Dictionary<string, string>
            {
                { nameof(watch.ElapsedMilliseconds), "" + watch.ElapsedMilliseconds },
            };

            if (response is null) return properties;

            if (request is Features.Searching.SearchTrackRequest searching)
            {
                properties.Add(nameof(searching.QueryString), searching.QueryString);
                properties.Add(nameof(searching.Grouping), searching.Grouping.GetType().Name);
                properties.Add(nameof(searching.Sorting), searching.Sorting.GetType().Name);
            }

            if (request is Features.TrackInformation.TrackInformationRequest)
            {
                var track = (Features.TrackInformation.TrackDetails)(object)response;

                properties.Add(nameof(track.Title), track.Title);
                properties.Add(nameof(track.Artist), track.Artist);
                properties.Add(nameof(track.RecordedYear), "" + track.RecordedYear);
            }

            if (request is Features.AllListingsOfEdition.AllListingsOfEditionRequest listing)
            {
                properties.Add(nameof(listing.Year), "" + listing.Year);
            }

            return properties;
        }
    }
}
