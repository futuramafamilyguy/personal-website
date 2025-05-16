namespace PersonalWebsite.Core.Interfaces;

public interface IMarkdownStorage
{
    Task<string> SaveMarkdownAsync(string content, string fileName, string basePath);
    Task RemoveMarkdownAsync(string fileName, string basePath);
    string GetMarkdownFileNameFromUrl(string postUrl);
    Task<string> CopyMarkdownAsync(string fileName, string newFileName, string basePath);
}
