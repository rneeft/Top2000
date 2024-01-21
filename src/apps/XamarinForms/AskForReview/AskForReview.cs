using Microsoft.Extensions.Options;
using System.Linq;
using Xamarin.Essentials.Interfaces;

namespace Chroomsoft.Top2000.Apps.AskForReview
{
    public interface IStoreReview
    {
        void OpenStoreReviewPage(string appId);
    }

    public class AskForReviewConfiguration
    {
        public bool AskForReview { get; set; }

        public string AppId { get; set; } = string.Empty;
    }

    public interface IAskForReview
    {
        void AskForReview();

        bool MustAskForReview();
    }

    public class ReviewModule : IAskForReview
    {
        private static int[] askForReviews = new[] { 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987, 1597 };
        private readonly IOptions<AskForReviewConfiguration> configuration;
        private readonly IPreferences preferences;
        private readonly IStoreReview storeReview;

        public ReviewModule(IOptions<AskForReviewConfiguration> configuration, IPreferences preferences, IStoreReview storeReview)
        {
            this.configuration = configuration;
            this.preferences = preferences;
            this.storeReview = storeReview;
        }

        public int StartupCountForReview
        {
            get { return preferences.Get(nameof(StartupCountForReview), 0); }
            set { preferences.Set(nameof(StartupCountForReview), value); }
        }

        private bool HasReviewed
        {
            get { return preferences.Get(nameof(HasReviewed), false); }
            set { preferences.Set(nameof(HasReviewed), value); }
        }

        public bool MustAskForReview()
        {
            if (!configuration.Value.AskForReview || HasReviewed)
                return false;

            var count = StartupCountForReview++;

            if (askForReviews.Contains(count))
                return true;

            return false;
        }

        public void AskForReview()
        {
            HasReviewed = true;
            storeReview.OpenStoreReviewPage(configuration.Value.AppId);
        }
    }
}
