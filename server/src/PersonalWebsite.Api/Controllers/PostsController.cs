using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Interfaces;
using PersonalWebsite.Core.Models;
using PersonalWebsite.Infrastructure.Images;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : Controller
{
    private readonly IPostService _postService;
    private readonly IImageStorage _imageStorage;
    private readonly ImageStorageConfiguration _imageStorageConfiguration;

    public PostsController(IPostService postService, IImageStorage imageStorage, IOptions<ImageStorageConfiguration> imageStorageConfiguration)
    {
        _postService = postService;
        _imageStorage = imageStorage;
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
    [HttpPost("{postId}/image")]
    public async Task<IActionResult> UploadImageAsync(
        string postId,
        IFormFile imageFile
    )
    {
        if (imageFile is null || imageFile.Length == 0)
        {
            return BadRequest("No file uploaded or file is empty");
        }

        using var stream = imageFile.OpenReadStream();
        var fileExtension = Path.GetExtension(imageFile.FileName);
        var uniqueFileName = $"{postId}{fileExtension}";
        var imageDirectory = _imageStorageConfiguration.PictureImageDirectory.Replace(
            "{year}",
            year.ToString()
        );
        await _imageStorage.SaveImageAsync(stream, uniqueFileName, imageDirectory);

        var imageUrl = _imageStorage.GetImageUrl(uniqueFileName, imageDirectory);

        return Ok(new { ImageUrl = imageUrl });
    }
}
