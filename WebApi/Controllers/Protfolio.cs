using Microsoft.AspNetCore.Mvc;
using Octokit;
using Service;
using Service.Entites;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Protfolio : ControllerBase
    {
        private readonly IGitHubService _gitHubService;
        public Protfolio(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }
        // GET: api/<Protfolio>
        [HttpGet]
        public Task<List<RepositoryInfo>> GetProtfolio(string userName)
        {
           return _gitHubService.GetPortfolio(userName);
        }
        
        [HttpGet("id")]
        public Task<List<Repository>> GetRepositories(string repoName = null, string language = null, string userName = null)
        {
            return _gitHubService.SearchRepositories(repoName, language, userName);
        }


    }
}
