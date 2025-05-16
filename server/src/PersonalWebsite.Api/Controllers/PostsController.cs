using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.ImageStorage;
using PersonalWebsite.Infrastructure.MarkdownStorage;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;
    private readonly MarkdownStorageConfiguration _markdownStorageConfiguration;

    public PostsController(
        IPostService postService,
        IOptions<ImageStorageConfiguration> imageStorageConfiguration,
        IOptions<MarkdownStorageConfiguration> markdownStorageConfiguration
    )
    {
        _postService = postService;
        _imageStorageConfiguration = imageStorageConfiguration.Value;
        _markdownStorageConfiguration = markdownStorageConfiguration.Value;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetPostsAsync()
    {
        var posts = await _postService.GetPostsAsync();

        return Ok(posts);
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetPostBySlugAsync(string slug)
    {
        try
        {
            var post = await _postService.GetPostBySlugAsync(slug);

            return Ok(post);
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Post with specified slug was not found");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("")]
    public async Task<IActionResult> CreatePostAsync([FromBody] CreatePostRequest request)
    {
        if (request is null)
            return BadRequest("Post data is null");

        var post = await _postService.AddPostAsync(request.Title);

        return Ok(post);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePostAsync(
        string id,
        [FromBody] UpdatePostRequest request
    )
    {
        if (request is null)
            return BadRequest("Post data is null");

        var updatedPost = await _postService.UpdatePostAsync(
            id,
            request.Title,
            request.ContentUrl,
            request.ImageUrl,
            request.CreatedAtUtc
        );

        return Ok(updatedPost);
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePostAsync(string id)
    {
        await _postService.RemovePostAsync(id);

        return NoContent();
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{id}/image")]
    public async Task<IActionResult> UploadImageAsync(string id, IFormFile imageFile)
    {
        if (imageFile is null || imageFile.Length == 0)
        {
            return BadRequest("No file uploaded or file is empty");
        }

        try
        {
            using var stream = imageFile.OpenReadStream();
            var extension = Path.GetExtension(imageFile.FileName);
            var imageUrl = await _postService.UploadPostImageAsync(
                stream,
                id,
                extension,
                _imageStorageConfiguration.BasePathPost
            );

            return Ok(new { ImageUrl = imageUrl });
        }
        catch (ValidationException)
        {
            return BadRequest("Image failed validation checks");
        }
        catch (StorageException)
        {
            return StatusCode(500, "An error occurred while uploading the image");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}/image")]
    public async Task<IActionResult> DeleteImageAsync(string id)
    {
        try
        {
            await _postService.DeletePostImageAsync(
                id,
                _imageStorageConfiguration.BasePathPost
            );

            return NoContent();
        }
        catch (ValidationException)
        {
            return BadRequest("Post image failed validation checks");
        }
        catch (StorageException)
        {
            return StatusCode(500, "An error occurred while deleting the image");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{id}/content")]
    public async Task<IActionResult> UploadContentAsync(
        string id,
        [FromBody] UploadContentRequest request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return BadRequest("No content uploaded or content is empty");
        }

        try
        {
            var contentUrl = await _postService.UploadPostContentAsync(
                request.Content,
                id,
                _markdownStorageConfiguration.BasePath
            );

            return Ok(new { ContentUrl = contentUrl });
        }
        catch (ValidationException)
        {
            return BadRequest("Post content failed validation checks");
        }
        catch (StorageException)
        {
            return StatusCode(500, "An error occurred while uploading post content");
        }
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpDelete("{id}/content")]
    public async Task<IActionResult> DeleteContentAsync(string id)
    {
        try
        {
            await _postService.DeletePostContentAsync(
                id,
                _markdownStorageConfiguration.BasePath
            );

            return NoContent();
        }
        catch (ValidationException)
        {
            return BadRequest("Post content failed validation checks");
        }
        catch (StorageException)
        {
            return StatusCode(500, "An error occurred while deleting post content");
        }
    }
}
