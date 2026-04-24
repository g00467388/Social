
using System.Collections.ObjectModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ILogger<CommentController> _logger;
    private readonly CommentService _commentService;
    private readonly PostService _postService;
    private readonly ApplicationDbContext _context;

    private readonly UserManager<User> _userManager;
    public CommentController(ILogger<CommentController> logger,
    CommentService service, PostService postService, UserManager<User> userManager, ApplicationDbContext context)
    {
        _logger = logger;
        _commentService = service;
        _postService = postService;
        _userManager = userManager;
        _context = context;
    }


    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<Post>>> GetCommentsAsync()
          => Ok(await _commentService.GetAllAsync());

    [HttpPost("post/comments")]
    public async Task<ActionResult> CreateComment([FromBody] CommentRequest commentRequest)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return Unauthorized();
        Console.WriteLine($"Post ID: {commentRequest.PostID}\nContent: {commentRequest.Content}");

        if (!Guid.TryParse(commentRequest.PostID, out var postGuid))    
            return BadRequest("Invalid PostID format. Must be a GUID.");


        // Lookup post by GUID
        var post = await _context.Posts
            .FirstOrDefaultAsync(x => x.ID == postGuid);

        if (post is null)
            return NotFound($"No such post with id '{commentRequest.PostID}' exists");


        var comment = new Comment()
        {
            UserId = userId,
            PostID = post.ID.ToString(),
            Content = commentRequest.Content,
        };
        _context.Add(comment);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
