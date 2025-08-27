using UNIR.TFE.Monorepo.WebApp.Models.Common;

namespace UNIR.TFE.Monorepo.WebApp.Infrastructure.External.GitHub
{
    public interface IGitRepositoryAnalyzerService
    {
        Task<GitModel> AnalyzeRepositoryAsync(string repoUrl, string branch, string? token = null);
    }
}
