using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Entites
{
    public class RepositoryInfo
    {
        public string Name { get; set; }
        public IReadOnlyDictionary<string, long> Languages { get; set; }
        public DateTimeOffset? LastCommit { get; set; }
        public int Stars { get; set; }
        public int PullRequestCount { get; set; }
        public string Url { get; set; }
    }
}
