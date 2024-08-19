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
}
