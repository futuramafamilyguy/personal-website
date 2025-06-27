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
        string? contentUrl,
        string? imageUrl,
        string? imageObjectKey,
        DateTime createdAtUtc
    );
    Task RemovePostAsync(string id);
    Task<string> UploadPostContentAsync(string content, string id, string contentDirectory);
    Task DeletePostContentAsync(string id, string contentDirectory);
    Task<string> HandleImageUploadAsync(string id, string imageBasePath, string fileExtension);
}
