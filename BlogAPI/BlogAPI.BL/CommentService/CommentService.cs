using System;
using System.Linq;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.DAL.Interfaces;
using BlogAPI.Domain.Response;
using System.Collections.Generic;
using BlogAPI.BL.JwtTokenService;
using BlogAPI.Domain.Entity.Table;
using BlogAPI.BL.DTOs.CommentDTOs;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BlogAPI.Domain.Entity.Connection;

namespace BlogAPI.BL.CommentService;

public class CommentService : ICommentService
{
    private readonly ILogger<CommentService> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IBaseRepository<ArticleEntity> _articleRepository;
    private readonly IBaseRepository<CommentEntity> _commentRepository;
    private readonly IBaseRepository<UserCommentEntity> _userCommentRepository;
    private readonly IBaseRepository<ArticleCommentEntity> _articleCommentRepository;

    public CommentService(ILogger<CommentService> logger,
        IJwtTokenService jwtTokenService,
        IBaseRepository<ArticleEntity> articleRepository,
        IBaseRepository<CommentEntity> commentRepository,
        IBaseRepository<UserCommentEntity> userCommentRepository,
        IBaseRepository<ArticleCommentEntity> articleCommentRepository)
    {
        _logger = logger;
        _jwtTokenService = jwtTokenService;
        _articleRepository = articleRepository;
        _commentRepository = commentRepository;
        _userCommentRepository = userCommentRepository;
        _articleCommentRepository = articleCommentRepository;
    }
    public async Task<IBaseResponse<IEnumerable<CommentDto>>> GetAllCommentsByArticleAsync(int articleId)
    {
        try
        {
            var comments = await _commentRepository.GetAll()
                .Where(x => x.ArticleComment
                    .Any(articleComments => articleComments.ArticleId == articleId))
                .Select(comment =>
                    new CommentDto
                    {
                        Comment = comment.Comment,
                        CreatedAt = comment.CreatedAt,
                        UpdatedAt = comment.UpdatedAt
                    })
                .ToListAsync();

            return new BaseResponse<IEnumerable<CommentDto>>
            {
                Data = comments,
                StatusCode = StatusCode.Ok
            };
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<CommentDto>().ServerResponseEnumerable("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<CommentDto>> DeleteCommentAsync(string token, int articleId, int commentId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.GetAll()
                .Where(find => find.ArticleId == articleId)
                .FirstOrDefaultAsync();
            var comment = await _commentRepository.GetAll()
                .Where(c => c.CommentId == commentId && c.ArticleComment
                    .Any(articleComments => articleComments.ArticleId == articleId))
                .FirstOrDefaultAsync();

            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized to delete this article");
                return new BaseResponse<CommentDto>().ServerResponse("User is not authorized to delete this comment", StatusCode.Unauthorized);
            }
            
            if (article == null!)
            {
                _logger.LogError("Article is not found!");
                return new BaseResponse<CommentDto>().ServerResponse("Article is not found", StatusCode.NotFound);
            }

            if (comment == null!)
            {
                _logger.LogError("Comment is not found!");
                return new BaseResponse<CommentDto>().ServerResponse("Comment is not found", StatusCode.NotFound);
            }

            await _commentRepository.DeleteAsync(comment);
            return new BaseResponse<CommentDto>().ServerResponse("Comment successfully deleted!", StatusCode.NoContent);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<CommentDto>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<CommentAddDto>> AddCommentAsync(CommentAddDto commentDto, string token, int articleId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.GetAll()
                .Where(find => find.ArticleId == articleId)
                .FirstOrDefaultAsync();
            if (!userId.HasValue)
            {
                _logger.LogError("User is not authorized to update this article");
                return new BaseResponse<CommentAddDto>().ServerResponse("User is not authorized to added this comment", StatusCode.Unauthorized);
            }

            if (article == null!)
            {
                _logger.LogError("Article is not found!");
                return new BaseResponse<CommentAddDto>().ServerResponse("Article is not found", StatusCode.NotFound);
            }

            var comment = new CommentEntity
            {
                Comment = commentDto.Comment,
                CreatedAt = commentDto.CreatedAt
            };

            var userComment = new UserCommentEntity
            {
                UserId = userId.Value,
                Comment = comment
            };

            var articleComment = new ArticleCommentEntity
            {
                ArticleId = articleId,
                Comment = comment
            };

            await _commentRepository.AddAsync(comment);
            await _userCommentRepository.AddAsync(userComment);
            await _articleCommentRepository.AddAsync(articleComment);

            _logger.LogInformation("Comment delivered successfully!");
            return new BaseResponse<CommentAddDto>().ServerResponse("Comment delivered successfully!", StatusCode.Created);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<CommentAddDto>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }

    public async Task<IBaseResponse<CommentUpdateDto>> UpdateCommentAsync(CommentUpdateDto commentDto, string token, int articleId, int commentId)
    {
        try
        {
            var userId = _jwtTokenService.GetUserIdFromToken(token);
            var article = await _articleRepository.GetAll()
                .Where(find => find.ArticleId == articleId)
                .FirstOrDefaultAsync();

            var comment = await _commentRepository.GetAll()
                .Where(c => c.CommentId == commentId && c.ArticleComment.Any(articleComments => articleComments.ArticleId == articleId))
                .FirstOrDefaultAsync();

            if (!userId.HasValue)
            {
                _logger.LogError("");
                return new BaseResponse<CommentUpdateDto>().ServerResponse("User is not authorized to update this comment", StatusCode.Unauthorized);
            }
            
            if (article == null!)
            {
                _logger.LogError("Article is not found!");
                return new BaseResponse<CommentUpdateDto>().ServerResponse("Article is not found", StatusCode.NotFound);
            }
            
            if (comment == null!)
            {
                _logger.LogError("Comment is not found!");
                return new BaseResponse<CommentUpdateDto>().ServerResponse("Comment is not found", StatusCode.NotFound);
            }

            comment.Comment = commentDto.Comment;
            comment.UpdatedAt = commentDto.UpdatedAt;

            await _commentRepository.UpdateAsync(comment);
            
            _logger.LogInformation("Comment successfully updated!");
            return new BaseResponse<CommentUpdateDto>().ServerResponse("Comment successfully updated!", StatusCode.Ok);
        }
        catch (Exception ex)
        {
            _logger.LogError("Internal server error: {ExMessage}", ex.Message);
            return new BaseResponse<CommentUpdateDto>().ServerResponse("Internal server error", StatusCode.InternalServerError);
        }
    }
}