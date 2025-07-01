using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAsync();
    Task<Post> GetAsync(string id);
    Task<Post?> GetBySlugAsync(string slug);
    Task<Post> AddAsync(Post post);
    Task<int> IncrementMarkdownVersionAsync(string id);
    Task<Post> UpdateAsync(string id, Post updatedPost);
    Task UpdateMarkdownInfoAsync(string id, string markdownObjectKey, string markdownUrl);
    Task UpdateImageInfoAsync(string id, string imageObjectKey, string imageUrl);
    Task RemoveAsync(string id);
}
