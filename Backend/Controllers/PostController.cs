using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public PostController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("title")]
        public IActionResult GetPostsByTitle(string title)
        {
             var posts = _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Where(p => p.Title.StartsWith(title))
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Content,
                    p.CreatedAt,
                    p.UserId,
                    Username = p.User.UserName,
                    Comments = p.Comments.Select(c => new
                    {
                        c.Id,
                        c.PostId,
                        c.Content,
                        c.CreatedAt,
                        c.UserId,
                        Username = c.User.UserName
                    }).ToList()
                }).ToListAsync();
            
            return Ok(posts);

        }

        [HttpGet("all")]
        public IActionResult GetAllPosts()
        {
            var posts = _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Content,
                    p.CreatedAt,
                    p.UserId,
                    Username = p.User.UserName,
                    Comments = p.Comments.Select(c => new
                    {
                        c.Id,
                        c.PostId,
                        c.Content,
                        c.CreatedAt,
                        c.UserId,
                        Username = c.User.UserName
                    }).ToList()
                })
                .ToList();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById(int id)
        {
            var post = _dbContext.Posts
                .Include(p => p.User)
                .Include(p => p.Comments).ThenInclude(c => c.User)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Title,
                    p.Content,
                    p.CreatedAt,
                    p.UserId,
                    Username = p.User.UserName,
                    Comments = p.Comments.Select(c => new
                    {
                        c.Id,
                        c.PostId,
                        c.Content,
                        c.CreatedAt,
                        c.UserId,
                        Username = c.User.UserName
                    }).ToList()
                })
                .FirstOrDefault();

            if (post == null)
                return NotFound($"Post with id {id} not found");

            return Ok(post);
        }

                [HttpPost]
               [Authorize]
                public IActionResult CreatePost(CreatePostRequest request)
                {
                    if (string.IsNullOrWhiteSpace(request.Title) || string.IsNullOrWhiteSpace(request.Content))
                        return BadRequest("Title and Content are required");

                    var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (userId == null)
                        return Unauthorized("User not authenticated");

                    var post = new Post
                    {
                        Title = request.Title,
                        Content = request.Content,
                        CreatedAt = DateTime.UtcNow,
                        UserId = userId
                    };

                    _dbContext.Posts.Add(post);
                    _dbContext.SaveChanges();

                    var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
                    var postResponse = new
                    {
                        post.Id,
                        post.Title,
                        post.Content,
                        post.CreatedAt,
                        post.UserId,
                        Username = user.UserName,
                        Comments = new List<object>()
                    };

                    return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, postResponse);
                }
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult UpdatePost(int id, [FromBody] UpdatePostRequest request)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound($"Post with id {id} not found");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != post.UserId)
                return Forbid("You can only update your own posts");

            if (!string.IsNullOrWhiteSpace(request.Title))
                post.Title = request.Title;
            if (!string.IsNullOrWhiteSpace(request.Content))
                post.Content = request.Content;

            _dbContext.SaveChanges();

            var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            var postResponse = new
            {
                post.Id,
                post.Title,
                post.Content,
                post.CreatedAt,
                post.UserId,
                Username = user.UserName,
                Comments = new List<object>()
            };

            return Ok(postResponse);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult DeletePost(int id)
        {
            var post = _dbContext.Posts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return NotFound($"Post with id {id} not found");

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userId != post.UserId)
                return Forbid("You can only delete your own posts");

            _dbContext.Posts.Remove(post);
            _dbContext.SaveChanges();
            return Ok("Post deleted successfully");
        }
    }

    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
