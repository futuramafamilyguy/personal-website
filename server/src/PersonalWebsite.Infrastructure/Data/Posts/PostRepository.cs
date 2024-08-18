using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Infrastructure.Data.Posts;

public class PostRepository : IPostRepository
{
    private readonly IMongoCollection<PostDocument> _posts;
    private readonly MongoDbConfiguration _configuration;

    public PostRepository(IMongoClient client, IOptions<MongoDbConfiguration> settings)
    {
        _configuration = settings.Value;
        var database = client.GetDatabase(_configuration.DatabaseName);
        _posts = database.GetCollection<PostDocument>(_configuration.PostsCollectionName);
    }

    public async Task<Post> AddAsync(Post post)
    {
        var postDocument = PostMapper.ToDocument(post);
        await _posts.InsertOneAsync(postDocument);
        post.Id = postDocument.Id;

        return post;
    }

    public async Task<IEnumerable<Post>> GetAsync()
    {
        var sort = Builders<PostDocument>.Sort.Descending(post => post.CreatedAtUtc);
        var posts = await _posts
            .Find(FilterDefinition<PostDocument>.Empty)
            .Sort(sort)
            .ToListAsync();

        return posts.Select(PostMapper.ToDomain);
    }

    public async Task<Post> GetAsync(string id)
    {
        var filter = Builders<PostDocument>.Filter.Eq(post => post.Id, id);
        var post = await _posts.Find(filter).FirstOrDefaultAsync();

        return PostMapper.ToDomain(post);
    }

    public async Task RemoveAsync(string id) => await _posts.DeleteOneAsync(post => post.Id == id);

    public async Task<Post> UpdateAsync(string id, Post updatedPost)
    {
        await _posts.ReplaceOneAsync(post => post.Id == id, PostMapper.ToDocument(updatedPost));

        return updatedPost;
    }
}
