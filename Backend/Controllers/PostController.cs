using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : ControllerBase
{
    private readonly ILogger<PostController> _logger;
    private readonly IService<Post> _service;
    public PostController(ILogger<PostController> logger, IService<Post> service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpDelete("delete")]
    public async Task<ActionResult<Post>> DeletePostAsync([FromHeader] Guid id)
        => await _service.DeleteItemByID(id) is true
            ? NoContent()
            : NotFound($"No such post exists: {id}");

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostsAsync()
        => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPostById(Guid id)
    {
        var post = await _service.GetByIdAsync(id);

        if (post == null)
            return NotFound();

        return Ok(post);
    }

    [HttpPost("create")]
    public async Task<ActionResult<Post>> CreatePostAsync([FromHeader] PostRequest postRequest)
    {
        if (string.IsNullOrWhiteSpace(postRequest.Body) || string.IsNullOrWhiteSpace(postRequest.Title))
            return BadRequest("Post is missing content or title");

        var post = new Post(postRequest.Title, postRequest.Body);

        var result = await _service.InsertAsync(post);
        if (result is null)
            return BadRequest($"Failed to save post {post.Title}");

        return CreatedAtAction(nameof(GetPostById), new { id = post.ID }, post);
    }
}
