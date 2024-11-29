using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Interfaces;

public interface IPostRepository
{
    Task<IEnumerable<Post>> GetAsync();
    Task<Post> GetAsync(string id);
    Task<Post?> GetBySlugAsync(string slug);
    Task<Post> AddAsync(Post post);
    Task<Post> UpdateAsync(string id, Post updatedPost);
    Task RemoveAsync(string id);
}
