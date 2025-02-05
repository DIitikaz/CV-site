using Octokit;
using Service.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
   
    public interface IGitHubService
    {
        
        public Task<int> GetUsersFollowersAsync(string userName);
        public Task<List<Repository>> SearchRepositoriesInCSharp();

        public Task<string> GetRandomRepositoryNameAsync(string userName);
        public Task<List<RepositoryInfo>> GetPortfolio(string userName);
        public Task<List<Repository>> SearchRepositories(string repoName, string language, string userName);

    }
}
