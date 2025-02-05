using Microsoft.Extensions.Options;
using Octokit;
using Service.Entites;
using System.Threading.Channels;


namespace Service
{
    public class GitHubServive : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationOption _options;

        public GitHubServive(IOptions<GitHubIntegrationOption>options)
        {
            _client = new GitHubClient(new ProductHeaderValue("my-app"))
            {
                Credentials = new Credentials(options.Value.Token, AuthenticationType.Bearer)
            };
                
            _options = options.Value;
        }

        public async Task<int> GetUsersFollowersAsync(string userName)
        {
            var user = _client.User.Get(userName);
            return user.Result.Followers;
        }

        public async Task<List<Repository>> SearchRepositoriesInCSharp()
        {
            var repository = new SearchRepositoriesRequest("repo-name") { Language = Language.CSharp }; 
            var result = _client.Search.SearchRepo(repository);
           return result.Result.Items.ToList();
        }
        public async Task<int> GetUserPublicRepositories( string userName)
        {
            var user= await _client.User.Get(userName);
            return user.PublicRepos;
        }
        public async Task<List<Repository>> GetPrivateRepos(string userName)
        {
            var repositories = await _client.Repository.GetAllForUser(userName);
            return repositories.Where(repo => repo.Private).ToList();
        }
        public async Task<string> GetRandomRepositoryNameAsync(string userName)
        {
            var repositories = await _client.Repository.GetAllForUser(userName);
   

            return repositories.FirstOrDefault()?.Name;
        }
        public async Task<List<RepositoryInfo>> GetPortfolio(string userName)
        {
            var repositories = await _client.Repository.GetAllForUser(userName);
            var repositoryInfoList = new List<RepositoryInfo>();

            foreach (var repo in repositories)
            {
                var commits = await _client.Repository.Commit.GetAll(repo.Owner.Login, repo.Name);
                var pullRequests = await _client.PullRequest.GetAllForRepository(repo.Owner.Login, repo.Name);
                var languages = await _client.Repository.GetAllLanguages(repo.Owner.Login, repo.Name);
                var languageDictionary = languages.ToDictionary(lang => lang.Name, lang => lang.NumberOfBytes);

                repositoryInfoList.Add(new RepositoryInfo
                {
                    Name = repo.Name,
                    Languages = languageDictionary, // Use the dictionary here
                    LastCommit = commits.FirstOrDefault()?.Commit.Committer.Date,
                    Stars = repo.StargazersCount,
                    PullRequestCount = pullRequests.Count,
                    Url = repo.HtmlUrl
                });
            }

            return repositoryInfoList;
        }

        public async Task<List<Repository>> SearchRepositories(string repoName = null, string language = null, string userName = null)
        {
            // Initialize a list to hold search parameters
            var searchTerms = new List<string>();

            if (!string.IsNullOrEmpty(repoName))
            {
                searchTerms.Add(repoName);
            }

            if (!string.IsNullOrEmpty(language))
            {
                searchTerms.Add(language);
            }

            if (!string.IsNullOrEmpty(userName))
            {
                searchTerms.Add(userName);
            }

            // Combine search terms into a single query string
            string searchQuery = string.Join(" ", searchTerms);

            // Create the request with the combined search query
            var request = new SearchRepositoriesRequest(searchQuery)
            {
                Language = !string.IsNullOrEmpty(language) && Enum.TryParse(typeof(Language), language, true, out var parsedLanguage) ? (Language)parsedLanguage : null,

                User = userName
            };

            var result = await _client.Search.SearchRepo(request);
            return result.Items.ToList();
        }




    }
}
