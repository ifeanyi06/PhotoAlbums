using Microsoft.Extensions.Logging;
using Domain.Models;
using Application.DTOs.Album;
using Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Application.Wrappers;
using Application.Utility;
namespace Web.Services
{
    public class AlbumPhotoService : IAlbumPhotoService
    {

        private readonly ILogger<AlbumPhotoService> _logger;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public AlbumPhotoService(IMapper mapper, ILogger<AlbumPhotoService> logger, IPhotoService photoService)
        {
            _logger = logger;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async Task<IEnumerable<AlbumResponse>> GetAlbumsAsync()
        {
            var albumData = await GetAlbumDataAsync();
            var albums = _mapper.Map<IEnumerable<AlbumResponse>>(albumData);

            var photoData = await GetPhotoDataAsync();
            var photos = _mapper.Map<IEnumerable<AlbumPhotosResponse>>(photoData);

            albums.ToList().ForEach(x => x.Photos = photos.Where(y => y.AlbumId == x.Id));

            return albums;

        }

        public async Task<PagedResponse<IEnumerable<AlbumResponse>>> GetAlbumsPagedAsync(PaginationFilter paginationFilter)
        {
            var albumData = await GetAlbumDataAsync();
            var albums = _mapper.Map<IEnumerable<AlbumResponse>>(albumData);

            var photoData = await GetPhotoDataAsync();
            var photos = _mapper.Map<IEnumerable<AlbumPhotosResponse>>(photoData);

            int records = albums.Count();
            int totalPages = records / paginationFilter.PageSize;

            albums.ToList().ForEach(x => x.Photos = photos.Where(y => y.AlbumId == x.Id));
            var response = new PagedResponse<IEnumerable<AlbumResponse>>(albums, paginationFilter.PageNumber, paginationFilter.PageSize);
     
            response.TotalRecords = records;
            response.TotalPages = totalPages == 0 ? 1 : totalPages;
            return response;
        }

        public async Task<AlbumResponse> GetAlbumAsync(int albumId)
        {
            var albumData = await GetAlbumDataAsync();
            var albums = _mapper.Map<IEnumerable<AlbumResponse>>(albumData);

            var photoData = await GetPhotoDataAsync();
            var photos = _mapper.Map<IEnumerable<AlbumPhotosResponse>>(photoData);

            var album = albums.Where(x => x.Id == albumId).FirstOrDefault();
            album.Photos = photos.Where(z => z.AlbumId == albumId);
            
            return album;

        }

   
        public async Task<PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>> GetAlbumPhotoStatsAsync(PaginationFilter paginationFilter)
        {
            List<AlbumPhotoStatsResponse> stats = new List<AlbumPhotoStatsResponse>();
            var albumData = await GetAlbumDataAsync();
            var photoData = await GetPhotoDataAsync();

            var albums = _mapper.Map<IEnumerable<AlbumResponse>>(albumData);
            var photos = _mapper.Map<IEnumerable<AlbumPhotosResponse>>(photoData);

            int records = albums.Count();
            int totalPages = records / paginationFilter.PageSize;

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var albumsPaged = albums.Skip(skip).Take(paginationFilter.PageSize).ToList();

            albumsPaged.ToList().ForEach(x => stats.Add(processAlbumStats(x, photos.Where(y => y.AlbumId == x.Id))));

            var response = new PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>(stats, paginationFilter.PageNumber, paginationFilter.PageSize);

            response.TotalRecords = records;
            response.TotalPages = totalPages == 0 ? 1 : totalPages;
            return response;
        }

        private AlbumPhotoStatsResponse processAlbumStats(AlbumResponse album, IEnumerable<AlbumPhotosResponse> photos)
        {
            List<int> wordCounts = new List<int>();
            photos.ToList().ForEach(x => wordCounts.Add(WordCount.getWordCount(x.Title)));

            AlbumPhotoStatsResponse stats = new AlbumPhotoStatsResponse();
            stats.UserId = album.UserId;
            stats.AlbumTitle = album.Title;
            stats.Id = album.Id;
            stats.TotalWords = wordCounts.Sum();
            stats.MaximumWords = wordCounts.Max();
            stats.MinimumWords = wordCounts.Min();

            return stats;
        }

        public async Task<PagedResponse<IEnumerable<AlbumResponse>>> GetUserAlbumPhotoAsync(int userId, PaginationFilter paginationFilter)
        {
            var albumData = await GetAlbumDataAsync();
            var albums = _mapper.Map<IEnumerable<AlbumResponse>>(albumData);

            var photoData = await GetPhotoDataAsync();
            var photos = _mapper.Map<IEnumerable<AlbumPhotosResponse>>(photoData);

            var usersAlbum = albums.Where(x => x.UserId == userId);

            int records = usersAlbum.Count();
            int totalPages = records / paginationFilter.PageSize;

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var pagedData = usersAlbum.Skip(skip).Take(paginationFilter.PageSize).ToList();

            pagedData.ToList().ForEach(x => x.Photos = photos.Where(y => y.AlbumId == x.Id));

            var response = new PagedResponse<IEnumerable<AlbumResponse>>(pagedData, paginationFilter.PageNumber, paginationFilter.PageSize);
            response.TotalRecords = records;
            response.TotalPages = totalPages == 0 ? 1 : totalPages;
            return response;
        }

        private async Task<IEnumerable<Album>> GetAlbumDataAsync()
        {
            var albums = await _photoService.GetAlbums();
            return albums;
        }

        private async Task<IEnumerable<Photo>> GetPhotoDataAsync()
        {
            var photos = await _photoService.GetAlbumPhotos();
            return photos;
        }

    }
}
