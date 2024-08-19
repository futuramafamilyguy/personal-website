using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Api.DTOs;
using PersonalWebsite.Core.Interfaces;

namespace PersonalWebsite.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController : Controller
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
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
}
