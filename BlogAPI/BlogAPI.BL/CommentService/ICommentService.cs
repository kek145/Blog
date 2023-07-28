using System.Threading.Tasks;
using BlogAPI.Domain.Response;
using System.Collections.Generic;
using BlogAPI.Domain.DTOs.CommentDTOs;

namespace BlogAPI.BL.CommentService;

public interface ICommentService
{
    Task<IBaseResponse<IEnumerable<CommentDto>>> GetAllCommentsByArticleAsync(int articleId);
    Task<IBaseResponse<CommentDto>> DeleteCommentAsync(string token, int articleId, int commentId);
    Task<IBaseResponse<CommentAddDto>> AddCommentAsync(CommentAddDto commentDto, string token, int articleId);
    Task<IBaseResponse<CommentUpdateDto>> UpdateCommentAsync(CommentUpdateDto commentDto, string token, int articleId, int commentId);
}