using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.DTOs.Album;
using Application.Wrappers;

namespace Web.Services
{
    public interface IAlbumPhotoService
    {
        Task<IEnumerable<AlbumResponse>> GetAlbumsAsync();
        Task<PagedResponse<IEnumerable<AlbumResponse>>> GetAlbumsPagedAsync(PaginationFilter param);
        Task<AlbumResponse> GetAlbumAsync(int albumId);
        Task<PagedResponse<IEnumerable<AlbumResponse>>> GetUserAlbumPhotoAsync(int userId, PaginationFilter param);
        Task<PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>> GetAlbumPhotoStatsAsync(PaginationFilter param);
    }
}
