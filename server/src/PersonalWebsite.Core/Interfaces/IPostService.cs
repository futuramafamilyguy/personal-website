using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPostService
{
    Task<IEnumerable<Post>> GetPostsAsync();
    Task<Post> GetPostAsync(string id);
    Task<Post> AddPostAsync(string title);
    Task<Post> UpdatePostAsync(
        string id,
        string title,
        string? contentUrl,
        string? imageUrl,
        DateTime createdAtUtc
    );
    Task RemovePostAsync(string id);
    Task<string> UploadPostImageAsync(
        Stream imageStream,
        string id,
        string imageExtension,
        string imageDirectory
    );
    Task DeletePostImageAsync(string id, string imageDirectory);
    Task<string> UploadPostContentAsync(string content, string id, string contentDirectory);
    Task DeletePostContentAsync(string id, string contentDirectory);
}
