using System;
using BlogAPI.Domain.Enum;
using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using System.Collections.Generic;
using System.Linq;
using BlogAPI.DAL.UserRepository;
using BlogAPI.BL.DTOs.CommentDTOs;
using BlogAPI.BL.JwtTokenService;
using Microsoft.Extensions.Logging;
using BlogAPI.DAL.ArticleRepository;
using BlogAPI.DAL.CommentRepository;
using BlogAPI.DAL.UserCommentRepository;
using BlogAPI.DAL.ArticleCommentRepository;
using BlogAPI.Domain.Entity.Connection;
using BlogAPI.Domain.Entity.Table;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.BL.CommentService;

public class CommentService : ICommentService
{
    private readonly ILogger<CommentService> _logger;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IArticleRepository _articleRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IUserCommentRepository _userCommentRepository;
    private readonly IArticleCommentRepository _articleCommentRepository;

    public CommentService(ILogger<CommentService> logger,
        IJwtTokenService jwtTokenService,
        IArticleRepository articleRepository,
        ICommentRepository commentRepository,
        IUserCommentRepository userCommentRepository,
        IArticleCommentRepository articleCommentRepository)
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
            var comments = await _commentRepository.GetAllComments()
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
            var article = _articleRepository.FindArticleByIdAsync(articleId);
            var comment = await _commentRepository.GetAllComments()
                .Where(comment => comment.CommentId == commentId).FirstOrDefaultAsync();
            
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article");
            
            if (article == null!)
            {
                _logger.LogError("Article is not found!");
                return new BaseResponse<CommentDto>().ServerResponse("Article is not found", StatusCode.BadRequest);
            }

            if (comment == null!)
            {
                _logger.LogError("Comment is not found!");
                return new BaseResponse<CommentDto>().ServerResponse("Comment is not found", StatusCode.BadRequest);
            }

            await _commentRepository.DeleteCommentAsync(comment);
            return new BaseResponse<CommentDto>().ServerResponse("Comment successfully deleted!", StatusCode.Ok);
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
            var article = _articleRepository.FindArticleByIdAsync(articleId);
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article");

            if (article == null!)
            {
                _logger.LogError("Article is not found!");
                return new BaseResponse<CommentAddDto>().ServerResponse("Article is not found", StatusCode.BadRequest);
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

            await _commentRepository.AddCommentAsync(comment);
            await _userCommentRepository.AddUserCommentAsync(userComment);
            await _articleCommentRepository.AddArticleCommentAsync(articleComment);

            _logger.LogInformation("Comment delivered successfully!");
            return new BaseResponse<CommentAddDto>().ServerResponse("Comment delivered successfully!", StatusCode.Ok);
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
            var article = await _articleRepository.FindArticleByIdAsync(articleId);
            var comment = await _commentRepository.GetAllComments()
                .Where(c => c.CommentId == commentId)
                .FirstOrDefaultAsync();
            
            if (!userId.HasValue)
                throw new UnauthorizedAccessException("User is not authorized to update this article");
            
            if (article == null!)
            {
                _logger.LogError("Article is not found!");
                return new BaseResponse<CommentUpdateDto>().ServerResponse("Article is not found", StatusCode.BadRequest);
            }
            
            if (comment == null!)
            {
                _logger.LogError("Comment is not found!");
                return new BaseResponse<CommentUpdateDto>().ServerResponse("Comment is not found", StatusCode.BadRequest);
            }

            comment.Comment = commentDto.Comment;
            comment.UpdatedAt = commentDto.UpdatedAt;

            await _commentRepository.UpdateCommentAsync(comment);
            
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