﻿using Microsoft.AspNetCore.Authorization;
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
            request.MarkdownUrl,
            request.MarkdownObjectKey,
            request.ImageUrl,
            request.ImageObjectKey,
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
    [HttpPost("{id}/markdown-url")]
    public async Task<IActionResult> GenerateMarkdownUploadUrlAsync(string id)
    {
        var uploadUrl = await _postService.HandleMarkdownUploadAsync(
            id,
            _markdownStorageConfiguration.BasePath
        );

        return Ok(new { PresignedUploadUrl = uploadUrl });
    }

    [Authorize(Policy = "AdminPolicy")]
    [HttpPost("{id}/image-url")]
    public async Task<IActionResult> GenerateImageUploadUrlAsync(
        string id,
        [FromBody] GenerateImageUploadUrlRequest request
    )
    {
        var uploadUrl = await _postService.HandleImageUploadAsync(
            id,
            _imageStorageConfiguration.BasePathPost,
            request.FileExtension
        );

        return Ok(new { PresignedUploadUrl = uploadUrl });
    }
}
