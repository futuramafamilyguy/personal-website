namespace PersonalWebsite.Core.Interfaces;

public interface IMarkdownStorage
{
    Task<string> SaveMarkdownAsync(string content, string fileName, string directory);
    Task RemoveMarkdownAsync(string fileName, string directory);
    string GetMarkdownFileNameFromUrl(string postUrl);
}
