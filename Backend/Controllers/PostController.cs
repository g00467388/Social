using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Backend.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IService<Post> _service;
    private readonly ApplicationDbContext _context;
    public PostController(ILogger<PostController> logger, PostService service, ApplicationDbContext context)
    {
        _logger = logger;
        _service = service;
        _context = context;
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<Post>> DeletePostAsync([FromHeader] Guid id)
        => await _service.DeleteItemByID(id) is true
            ? NoContent()
            : NotFound($"No such post exists: {id}");

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<PostRespose>>> GetPostsAsync()
    {
        var posts = await _service.GetAllAsync();
        var response = posts.Select(p => new PostRespose
        {
            Id = p.ID.ToString(),
            Title = p.Title,
            Body = p.Body,
            CreationDate = p.CreationDate,
            Username = p.User.UserName,
        });
        return Ok(response);
    }
    //        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var post = await _service.GetByIdAsync(id);

        if (post == null)
            return NotFound();

        return Ok(post);
    }
    [HttpPost("create")]
    public async Task<ActionResult<Post>> CreatePostAsync([FromBody] PostRequest postRequest)
    {
        if (string.IsNullOrWhiteSpace(postRequest.Body) || string.IsNullOrWhiteSpace(postRequest.Title))
            return BadRequest("Post is missing content or title");

        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userID is null)
            return Unauthorized();

        var post = new Post(postRequest.Title, postRequest.Body, userID);

        _logger.LogInformation("User ID: {id}", userID);

        var result = await _service.InsertAsync(post);

        if (result is null)
            return BadRequest($"Failed to save post {post.Title}");

var createdPost = await _context.Posts
    .Include(p => p.User)
    .FirstOrDefaultAsync(p => p.ID == result.ID);
        //return CreatedAtAction(nameof(GetPostById), new { id = result.ID }, createdPost);
        return Ok(createdPost);
    }

}
