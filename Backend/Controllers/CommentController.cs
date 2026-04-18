using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly CommentService _service;
    public CommentController(ILogger<CommentController> logger, CommentService service)
    {
        _logger = logger;
        _service = service;
    }


    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Post>>> GetCommentsAsync()
          => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<ActionResult<Post>> CreateCommentAsync(CommentRequest commentRequest)
    {
        if (string.IsNullOrWhiteSpace(commentRequest.Content) || commentRequest.Post.ID == Guid.Empty)
            return BadRequest("comment is missing content or title");

        Comment comment = new Comment(commentRequest.Content, commentRequest.Post);
        if (await _service.InsertAsync(comment))
        {
            _logger.LogDebug("saved comment {id}", comment.ID);
            return Ok(comment);
        }

        _logger.LogDebug("Failed to save comment {id}", comment.ID);
        return BadRequest("failed to save comment");
    }

    [HttpGet]
    public async Task<ActionResult<Post>> GetPostsAsync(Guid Id)
    {
        var result = _service.GetByIdAsync(Id);
        if (result == null)
            return BadRequest($"No such comment exists with ID {Id}");

        return Ok(result);
    }




    [HttpDelete("delete")]
    public async Task<ActionResult<Post>> DeleteCommentAsync(Comment comment)
    {
        if (await _service.DeleteAsync(comment))
            return Ok(comment);
        return BadRequest("No such comment exists");
    }


}
