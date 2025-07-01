using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post> GetPostAsync(string id);
    Task<Post> GetPostBySlugAsync(string slug);
    Task<Post> AddPostAsync(string title);
    Task<Post> UpdatePostAsync(
        string id,
        string title,
        string? markdownUrl,
        string? markdownObjectKey,
        string? imageUrl,
        string? imageObjectKey,
        DateTime createdAtUtc,
        int markdownVersion
    );
    Task RemovePostAsync(string id);
    Task<string> HandleMarkdownUploadAsync(string id, string markdownBasePath);
    Task<string> HandleImageUploadAsync(string id, string imageBasePath, string fileExtension);
}
