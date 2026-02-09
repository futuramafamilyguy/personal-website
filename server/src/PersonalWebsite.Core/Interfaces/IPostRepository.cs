using PersonalWebsite.Core.Enums;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAsync(bool includeDrafts);
    Task<Post?> GetAsync(string id);
    Task<Post?> GetBySlugAsync(string slug);
    Task<Post> AddAsync(Post post);
    Task<int> IncrementMarkdownVersionAsync(string id);
    Task<bool> UpdateAsync(string id, Post updatedPost);
    Task UpdateMarkdownInfoAsync(string id, string markdownObjectKey, string markdownUrl);
    Task UpdateImageInfoAsync(string id, string imageObjectKey, string imageUrl);
    Task RemoveAsync(string id);
    Task<PublishResult> PublishAsync(string id, DateTime publishedAtUtc);
}
