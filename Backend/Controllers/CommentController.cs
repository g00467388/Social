using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public CommentController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("post/{postId}")]
        public IActionResult GetCommentsByPostId(int postId)
        {
            var postComments = _dbContext.Comments
                .Where(c => c.PostId == postId)
                .Include(c => c.User)
                .Select(c => new { c.Id, c.PostId, c.Content, c.CreatedAt, c.UserId, Username = c.User.UserName })
                .ToList();
            return Ok(postComments);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateComment([FromBody] CreateCommentRequest request)
        {
            if (request.PostId <= 0 || string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("PostId and Content are required");

            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == request.PostId);
            if (post == null)
                return NotFound($"Post with id {request.PostId} not found");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not authenticated");

            var comment = new Comment
            {
                PostId = request.PostId,
                Content = request.Content,
                CreatedAt = DateTime.UtcNow,
                UserId = userId
            };

            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var commentResponse = new { comment.Id, comment.PostId, comment.Content, comment.CreatedAt, comment.UserId, Username = user.UserName };
            return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = comment.PostId }, commentResponse);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdateComment(int id, [FromBody] UpdateCommentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Content is required");

            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);
            if (comment == null)
                return NotFound($"Comment with id {id} not found");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != comment.UserId)
                return Forbid("You can only update your own comments");

            comment.Content = request.Content;
            _dbContext.SaveChanges();

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var commentResponse = new { comment.Id, comment.PostId, comment.Content, comment.CreatedAt, comment.UserId, Username = user.UserName };
            return Ok(commentResponse);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeleteComment(int id)
        {
            var comment = _dbContext.Comments.FirstOrDefault(c => c.Id == id);
            if (comment == null)
                return NotFound($"Comment with id {id} not found");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != comment.UserId)
                return Forbid("You can only delete your own comments");

            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
            return Ok("Comment deleted successfully");
        }
    }

    public class CreateCommentRequest
    {
        public int PostId { get; set; }
        public string Content { get; set; }
    }

    public class UpdateCommentRequest
    {
        public string Content { get; set; }
    }
}
