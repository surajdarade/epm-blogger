using EPM_Blogger.Application.DTOs.Parameters;
using EPM_Blogger.Application.DTOs.Posts;
using EPM_Blogger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EPM_Blogger.API.Controllers
{
    /// <summary>
    /// Controller for managing likes on posts.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LikesController"/> class.
        /// </summary>
        /// <param name="likeService">Service that handles like operations.</param>
        public LikesController(ILikeService likeService)
        {
            _likeService = likeService ?? throw new ArgumentNullException(nameof(likeService));
        }

        /// <summary>
        /// Adds a like for a given post by a user.
        /// </summary>
        /// <param name="dto">Like request containing the user and post identifiers.</param>
        /// <returns>The updated like information for the post.</returns>
        /// <response code="200">Returns the updated like info.</response>
        /// <response code="400">If the request is malformed or an unexpected error occurs.</response>
        /// <response code="404">If the specified post does not exist.</response>
        [HttpPost("add")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(LikeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LikeResponseDto>> AddLike([FromBody] LikeRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _likeService.AddLikeAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                // semantic: post not found
                return NotFound(new { Message = "Post not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Removes a like for a given post by a user.
        /// </summary>
        /// <param name="dto">Like request containing the user and post identifiers.</param>
        /// <returns>The updated like information for the post.</returns>
        /// <response code="200">Returns the updated like info.</response>
        /// <response code="400">If the request is malformed or an unexpected error occurs.</response>
        /// <response code="404">If the specified post does not exist.</response>
        [HttpPost("remove")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(LikeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<LikeResponseDto>> RemoveLike([FromBody] LikeRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _likeService.RemoveLikeAsync(dto);
                return Ok(result);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { Message = "Post not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        /// <summary>
        /// Gets the total like count for a post.
        /// </summary>
        /// <param name="postId">The post identifier.</param>
        /// <returns>Total number of likes for the post.</returns>
        /// <response code="200">Returns the like count.</response>
        /// <response code="400">If an unexpected error occurs.</response>
        /// <response code="404">If the specified post does not exist.</response>
        [HttpGet("count/{postId}")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetLikeCount(int postId)
        {
            try
            {
                var count = await _likeService.GetAllLikeCountByPostIdAsync(postId);
                return Ok(new { Likes = count });
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { Message = "Post not found." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("check")]
        public async Task<IActionResult> HasUserLikedThePost([FromQuery] LikeCheckQueryParameter Check)
        {
            try
            {
                var isLiked = await _likeService.HasUserLikedThePostAsync(Check);
                if (isLiked)
                {
                    return Ok(new { liked = true });
                }
                else
                {

                    return Ok(new { liked = false });
                }
            }
            catch(Exception ex)
            {
                return Ok(new { Message = ex.Message });
            }
        }
    }
}