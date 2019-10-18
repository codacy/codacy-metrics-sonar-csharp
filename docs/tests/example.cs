// #Metrics: {"complexity": 1, "loc": 67, "cloc": 26, "nrMethods": 37, "nrClasses": 1}
using System;

namespace Octokit.Reactive
{
    public class ObservableGitHubClient : IObservableGitHubClient
    {
        readonly IGitHubClient _gitHubClient;

        // #LineComplexity: 1
        public ObservableGitHubClient(ProductHeaderValue productInformation)
            : this(new GitHubClient(productInformation))
        {
        }

        // #LineComplexity: 1
        public ObservableGitHubClient(ProductHeaderValue productInformation, ICredentialStore credentialStore)
            : this(new GitHubClient(productInformation, credentialStore))
        {
        }

        // #LineComplexity: 1
        public ObservableGitHubClient(ProductHeaderValue productInformation, Uri baseAddress)
            : this(new GitHubClient(productInformation, baseAddress))
        {
        }

        // #LineComplexity: 1
        public ObservableGitHubClient(ProductHeaderValue productInformation, ICredentialStore credentialStore, Uri baseAddress)
            : this(new GitHubClient(productInformation, credentialStore, baseAddress))
        {
        }

        // #LineComplexity: 1
        public ObservableGitHubClient(IGitHubClient gitHubClient)
        {
            Ensure.ArgumentNotNull(gitHubClient, "githubClient");

            _gitHubClient = gitHubClient;
            Authorization = new ObservableAuthorizationsClient(gitHubClient);
            Activity = new ObservableActivitiesClient(gitHubClient);
            Issue = new ObservableIssuesClient(gitHubClient);
            Miscellaneous = new ObservableMiscellaneousClient(gitHubClient);
            Oauth = new ObservableOauthClient(gitHubClient);
            Organization = new ObservableOrganizationsClient(gitHubClient);
            PullRequest = new ObservablePullRequestsClient(gitHubClient);
            Repository = new ObservableRepositoriesClient(gitHubClient);
            User = new ObservableUsersClient(gitHubClient);
            Git = new ObservableGitDatabaseClient(gitHubClient);
            Gist = new ObservableGistsClient(gitHubClient);
            Search = new ObservableSearchClient(gitHubClient);
            Enterprise = new ObservableEnterpriseClient(gitHubClient);
            Migration = new ObservableMigrationClient(gitHubClient);
            Reaction = new ObservableReactionsClient(gitHubClient);
        }

        public IConnection Connection
        {
            // #LineComplexity: 1
            get { return _gitHubClient.Connection; }
        }

        // #LineComplexity: 1
        public IObservableAuthorizationsClient Authorization { get; private set; }
        // #LineComplexity: 1
        public IObservableActivitiesClient Activity { get; private set; }
        // #LineComplexity: 1
        public IObservableIssuesClient Issue { get; private set; }
        // #LineComplexity: 1
        public IObservableMiscellaneousClient Miscellaneous { get; private set; }
        // #LineComplexity: 1
        public IObservableOauthClient Oauth { get; private set; }
        // #LineComplexity: 1
        public IObservableOrganizationsClient Organization { get; private set; }
        // #LineComplexity: 1
        public IObservablePullRequestsClient PullRequest { get; private set; }
        // #LineComplexity: 1
        public IObservableRepositoriesClient Repository { get; private set; }
        // #LineComplexity: 1
        public IObservableGistsClient Gist { get; private set; }
        // #LineComplexity: 1
        public IObservableUsersClient User { get; private set; }
        // #LineComplexity: 1
        public IObservableGitDatabaseClient Git { get; private set; }
        // #LineComplexity: 1
        public IObservableSearchClient Search { get; private set; }
        // #LineComplexity: 1
        public IObservableEnterpriseClient Enterprise { get; private set; }
        // #LineComplexity: 1
        public IObservableMigrationClient Migration { get; private set; }
        // #LineComplexity: 1
        public IObservableReactionsClient Reaction { get; private set; }

        /// <summary>
        /// Gets the latest API Info - this will be null if no API calls have been made
        /// </summary>
        /// <returns><seealso cref="ApiInfo"/> representing the information returned as part of an Api call</returns>
        // #LineComplexity: 1
        public ApiInfo GetLastApiInfo()
        {
            return _gitHubClient.Connection.GetLastApiInfo();
        }
    }
}
