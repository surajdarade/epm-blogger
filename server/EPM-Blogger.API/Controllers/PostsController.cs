using EPM_Blogger.Application.DTOs.Parameters;
using EPM_Blogger.Application.DTOs.Posts;
using EPM_Blogger.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EPM_Blogger.API.Controllers
{
    /// <summary>
    /// API controller for managing blog posts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class.
        /// </summary>
        /// <param name="service">Post service to handle post operations.</param>
        public PostsController(IPostService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        /// <response code="200">Returns the list of posts.</response>
        /// 
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PostDto>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _service.GetAllAsync();
            return Ok(posts);
        }

        /// <summary>
        /// Retrieves all posts for particular user id.
        /// </summary>
        /// <returns>A list of all posts by their user id.</returns>
        /// <response code="200">Returns the list of posts based on their User Id.</response>
        /// 
        [HttpGet("user/{userId:int}")]
        [ProducesResponseType(typeof(IEnumerable<PostDto>), 200)]
        public async Task<IActionResult> GetAllByUserId(int userId)
        {
            var posts = await _service.GetAllByUserIdAsync(userId);
            return Ok(posts);
        }

        /// <summary>
        /// Retrieves a post by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the post to retrieve.</param>
        /// <returns>The post with the specified ID.</returns>
        /// <response code="200">Returns the requested post.</response>
        /// <response code="404">If the post is not found.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PostDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(int id)
        {
            var post = await _service.GetByIdAsync(id);
            if (post == null) return NotFound("Post not found.");
            return Ok(post);
        }

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="dto">The post data transfer object containing new post information.</param>
        /// <returns>The newly created post.</returns>
        /// <response code="201">Returns the newly created post.</response>
        /// <response code="400">If the post data is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PostDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto)
        {
            try
            {
                var post = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Updates an existing post.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="dto">The post data transfer object containing updated post information.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Indicates the post was updated successfully.</response>
        /// <response code="404">If the post to update is not found.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePostDto dto)
        {
            var success = await _service.UpdateAsync(id, dto);
            if (!success) return NotFound("Post not found.");
            return Ok("Post updated successfully.");
        }

        /// <summary>
        /// Deletes a post by its ID.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /// <returns>No content on success.</returns>
        /// <response code="204">Indicates the post was deleted successfully.</response>
        /// <response code="404">If the post to delete is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success) return NotFound("Post not found.");
            return Ok("Post Deleted Successfully");
        }

        /// <summary>
        /// Retrieves all posts within a range.
        /// </summary>
        /// <returns>A list of all posts within a range or limit.</returns>
        /// <response code="200">Returns the list of posts with pagination.</response>
        /// 
        [HttpGet("limit")]
        [ProducesResponseType(typeof(IEnumerable<PostDto>), 200)]
        public async Task<IActionResult> GetAllPostsWithinRange([FromQuery]PostQueryParameters pq)
        {
            var posts = await _service.GetAllPostsWithinRangeAsync(pq);
            return Ok(posts);
        }


    }
}
