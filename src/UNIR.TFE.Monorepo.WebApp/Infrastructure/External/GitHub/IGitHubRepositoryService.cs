using UNIR.TFE.Monorepo.WebApp.Models.Common;

namespace UNIR.TFE.Monorepo.WebApp.Infrastructure.External.GitHub
{
    public interface IGitHubRepositoryService
    {
        Task<string> GetBranchCommitShaAsync(string owner, string repo, string branch, string? token);
        Task<string> GetDefaultBranchAsync(string owner, string repo, string? token);
        Task<List<SubmoduleInfo>> GetSubmodulesAsync(string owner, string repo, string commitSha, string? token);
    }

    public interface IGitHubUrlParser
    {
        (string Owner, string Repo) ParseGitHubUrl(string url);
    }

}
