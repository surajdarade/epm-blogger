using EPM_Blogger.Application.DTOs.Parameters;
using EPM_Blogger.Application.DTOs.Posts;
using EPM_Blogger.Application.Interfaces;
using EPM_Blogger.Domain.Interfaces;
using EPM_Blogger.Domains.Models;


namespace EPM_Blogger.Application.Services
{
    public class LikeService :ILikeService
    {
        private readonly ILikeRepository _likeRepo;
        private readonly IPostRepository _postRepo;
        private readonly IUserRepository _userRepo ;


        public LikeService(ILikeRepository likeRepo, IPostRepository postRepo, IUserRepository userRepo)
        {
            _likeRepo = likeRepo;
            _postRepo = postRepo;
            _userRepo = userRepo;
        }

        public async Task<LikeResponseDto> AddLikeAsync(LikeRequestDto dto)
        {
            var IsPostExists = await _postRepo.GetByIdAsync(dto.PostId)!=null;
            if (!IsPostExists)
            {
                throw new InvalidOperationException("Post Id didn't exists.");
            }
            var existingLike = await _likeRepo.GetUserLikeAsync(dto.UserId, dto.PostId);

            if (existingLike == null)
            {
                var newLike = new Like
                {
                    UserId = dto.UserId,
                    PostId = dto.PostId,
                    CreatedAt = DateTime.UtcNow
                };
                await _likeRepo.AddLikeAsync(newLike);
            }
            // If already liked, do nothing

            return new LikeResponseDto
            {
                PostId = dto.PostId,
                LikeCount = await _likeRepo.GetLikeCountAsync(dto.PostId)
            };
        }

        public async Task<LikeResponseDto> RemoveLikeAsync(LikeRequestDto dto)
        {
            var IsPostExists = await _postRepo.GetByIdAsync(dto.PostId) != null;
            if (!IsPostExists)
            {
                throw new InvalidOperationException("Post Id didn't exists.");
            }
            var existingLike = await _likeRepo.GetUserLikeAsync(dto.UserId, dto.PostId);

            if (existingLike != null)
            {
                await _likeRepo.DeleteLikeAsync(existingLike);
            }

            return new LikeResponseDto
            {
                PostId = dto.PostId,
                LikeCount = await _likeRepo.GetLikeCountAsync(dto.PostId)
            };
        }
        public async Task<int> GetAllLikeCountByPostIdAsync(int postId)
        {
            var IsPostExists = await _postRepo.GetByIdAsync(postId) != null;
            if (!IsPostExists)
            {
                throw new InvalidOperationException("Post Id didn't exists.");
            }
            return await _likeRepo.GetLikeCountAsync(postId);
        }

        public async Task<bool> HasUserLikedThePostAsync(LikeCheckQueryParameter query)
        {
            bool isPostExists  = await _postRepo.GetByIdAsync(query.postId) != null;
            if (!isPostExists)
            {
                throw new InvalidOperationException("Post didn't exists");
            }
            bool isUserExists = await _userRepo.GetByIdAsync(query.userId) != null;
            if (!isUserExists)
            {
                throw new InvalidOperationException("User didn't exists");
            }

            return await _likeRepo.HasUserLikedThePostAsync(query.userId, query.postId);
        }
    }
}
