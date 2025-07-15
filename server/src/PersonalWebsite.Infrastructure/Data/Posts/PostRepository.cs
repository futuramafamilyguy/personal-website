using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;

namespace PersonalWebsite.Infrastructure.Data.Posts;

public class PostRepository : IPostRepository
{
    private readonly IMongoCollection<PostDocument> _posts;
    private readonly MongoDbConfiguration _configuration;

    public PostRepository(IMongoClient client, IOptions<MongoDbConfiguration> configuration)
    {
        _configuration = configuration.Value;
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

    public async Task<Post?> GetAsync(string id)
    {
        var filter = Builders<PostDocument>.Filter.Eq(post => post.Id, id);
        var post = await _posts.Find(filter).FirstOrDefaultAsync();
        if (post is null)
            return null;

        return PostMapper.ToDomain(post);
    }

    public async Task<Post?> GetBySlugAsync(string slug)
    {
        var filter = Builders<PostDocument>.Filter.Eq(post => post.Slug, slug);
        var post = await _posts.Find(filter).FirstOrDefaultAsync();
        if (post is null)
            return null;

        return PostMapper.ToDomain(post);
    }

    public async Task RemoveAsync(string id) => await _posts.DeleteOneAsync(post => post.Id == id);

    public async Task<bool> UpdateAsync(string id, Post updatedPost)
    {
        var result = await _posts.ReplaceOneAsync(
            post => post.Id == id,
            PostMapper.ToDocument(updatedPost)
        );

        return result.MatchedCount == 1;
    }

    public async Task UpdateMarkdownInfoAsync(
        string id,
        string markdownObjectKey,
        string markdownUrl
    )
    {
        var filter = Builders<PostDocument>.Filter.Eq(post => post.Id, id);
        var update = Builders<PostDocument>
            .Update.Set(post => post.MarkdownObjectKey, markdownObjectKey)
            .Set(post => post.MarkdownUrl, markdownUrl);

        await _posts.UpdateOneAsync(filter, update);
    }

    public async Task UpdateImageInfoAsync(string id, string imageObjectKey, string imageUrl)
    {
        var filter = Builders<PostDocument>.Filter.Eq(post => post.Id, id);
        var update = Builders<PostDocument>
            .Update.Set(post => post.ImageObjectKey, imageObjectKey)
            .Set(post => post.ImageUrl, imageUrl);

        await _posts.UpdateOneAsync(filter, update);
    }

    public async Task<int> IncrementMarkdownVersionAsync(string id)
    {
        var filter = Builders<PostDocument>.Filter.Eq(post => post.Id, id);
        var update = Builders<PostDocument>.Update.Inc(post => post.MarkdownVersion, 1);
        var options = new FindOneAndUpdateOptions<PostDocument, int>
        {
            ReturnDocument = ReturnDocument.After,
            Projection = Builders<PostDocument>.Projection.Expression(post => post.MarkdownVersion)
        };

        return await _posts.FindOneAndUpdateAsync(filter, update, options);
    }
}
