using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Core.Services;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public PostService(IPostRepository postRepository, IDateTimeProvider dateTimeProvider)
    {
        _postRepository = postRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Post> AddPostAsync(string title)
    {
        var post = await _postRepository.AddAsync(
            new Post
            {
                Title = title,
                LastUpdatedUtc = _dateTimeProvider.UtcNow,
                CreatedAtUtc = _dateTimeProvider.UtcNow,
            }
        );

        return post;
    }

    public async Task<Post> GetPostAsync(string id) => await _postRepository.GetAsync(id);

    public async Task<IEnumerable<Post>> GetPostsAsync() => await _postRepository.GetAsync();

    public async Task RemovePostAsync(string id) => await _postRepository.RemoveAsync(id);

    public async Task<Post> UpdatePostAsync(
        string id,
        string title,
        string? contentUrl,
        string? imageUrl,
        DateTime createdAtUtc
    )
    {
        var updatedPost = await _postRepository.UpdateAsync(
            id,
            new Post
            {
                Id = id,
                Title = title,
                ContentUrl = contentUrl,
                ImageUrl = imageUrl,
                LastUpdatedUtc = _dateTimeProvider.UtcNow,
                CreatedAtUtc = createdAtUtc,
            }
        );

        return updatedPost;
    }
}
