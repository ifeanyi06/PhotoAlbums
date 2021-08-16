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
        Task<PagedResponse<IEnumerable<AlbumResponse>>> GetAlbumsAsync(PaginationFilter param);
        Task<Album> GetAlbumAsync(int albumId);
        Task<PagedResponse<IEnumerable<AlbumPhotosResponse>>> GetAlbumPhotosAsync(int albumId, PaginationFilter param);
        Task<PagedResponse<IEnumerable<AlbumPhotosResponse>>> GetUserAlbumPhotoAsync(int userId, PaginationFilter param);
        Task<PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>> GetAlbumPhotoStatsAsync(PaginationFilter param);
    }
}
