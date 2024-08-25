using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Exceptions;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Infrastructure.ImageStorage;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;

    public PostsController(
        IPostService postService,
        IOptions<ImageStorageConfiguration> imageStorageConfiguration
    )
    {
        _postService = postService;
        _imageStorageConfiguration = imageStorageConfiguration.Value;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetPostsAsync()
    {
        var posts = await _postService.GetPostsAsync();

        return Ok(posts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostAsync(string id)
    {
        var post = await _postService.GetPostAsync(id);

        return Ok(post);
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
                _imageStorageConfiguration.PostImageDirectory
            );

            return Ok(new { ImageUrl = imageUrl });
        }
        catch (ImageValidationException)
        {
            return BadRequest("Image failed validation checks");
        }
        catch (ImageStorageException)
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
                _imageStorageConfiguration.PostImageDirectory
            );

            return NoContent();
        }
        catch (ImageValidationException)
        {
            return BadRequest("Picture image failed validation checks");
        }
        catch (ImageStorageException)
        {
            return StatusCode(500, "An error occurred while deleting the image");
        }
    }
}
