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

        public async Task<Album> GetAlbumAsync(int albumId)
        {
            var albums = await GetAlbumDataAsync();
            var album = albums.Where(x => x.Id == albumId).FirstOrDefault();
            return album;

        }

        public async Task<PagedResponse<IEnumerable<AlbumPhotosResponse>>> GetAlbumPhotosAsync(int albumId, PaginationFilter paginationFilter)
        {
            var data = await GetPhotoDataAsync();

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var photos = data.Skip(skip).Take(paginationFilter.PageSize).Where(x => x.AlbumId == albumId).ToList();
            var photoResponse = _mapper.Map<IEnumerable<AlbumPhotosResponse>>(photos);

            return new PagedResponse<IEnumerable<AlbumPhotosResponse>>(photoResponse, paginationFilter.PageNumber, paginationFilter.PageSize);

        }

        public async Task<PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>> GetAlbumPhotoStatsAsync(PaginationFilter paginationFilter)
        {
            List<AlbumPhotoStatsResponse> stats = new List<AlbumPhotoStatsResponse>();
            var albumData = await GetAlbumDataAsync();
            var photos = await GetPhotoDataAsync();

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var albums = albumData.Skip(skip).Take(paginationFilter.PageSize).ToList();
            albums.ToList().ForEach(x => stats.Add(processAlbumStats(x, photos.Where(y => y.AlbumId == x.Id))));

            return new PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>(stats, paginationFilter.PageNumber, paginationFilter.PageSize);
        }

        private AlbumPhotoStatsResponse processAlbumStats(Album album, IEnumerable<Photo> photos)
        {
            List<int> wordCounts = new List<int>();
            photos.ToList().ForEach(x => wordCounts.Add(WordCount.getWordCount(x.Title)));

            AlbumPhotoStatsResponse stats = new AlbumPhotoStatsResponse();
            stats.AlbumTitle = album.Title;
            stats.Id = album.Id;
            stats.TotalWords = wordCounts.Sum();
            stats.MaximumWords = wordCounts.Max(); 
            stats.MinimumWords = wordCounts.Min(); 

            return stats;
        }

      
        public async Task<PagedResponse<IEnumerable<AlbumResponse>>> GetAlbumsAsync(PaginationFilter paginationFilter)
        {
            var data = await GetAlbumDataAsync();
            

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var records = data.Count();
            var albums = data.Skip(skip).Take(paginationFilter.PageSize).ToList();

            var response = _mapper.Map<IEnumerable<AlbumResponse>>(albums);
            var rsp = new PagedResponse<IEnumerable<AlbumResponse>>(response, paginationFilter.PageNumber, paginationFilter.PageSize);
            rsp.TotalRecords = records;
            return rsp;
        }

        public async Task<PagedResponse<IEnumerable<AlbumPhotosResponse>>> GetUserAlbumPhotoAsync(int userId, PaginationFilter paginationFilter)
        {
            var albumData = await GetAlbumDataAsync();


            var userAlbums = albumData.Where(x => x.UserId == userId).Select(a => a.Id);
            var photoData = await GetPhotoDataAsync();
            var records = photoData.Count();

            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            var photos = photoData.Skip(skip).Take(paginationFilter.PageSize).Where(x => userAlbums.Contains(x.AlbumId)).ToList();
            var photoResponse = _mapper.Map<IEnumerable<AlbumPhotosResponse>>(photos);
            var response = new PagedResponse<IEnumerable<AlbumPhotosResponse>>(photoResponse, paginationFilter.PageNumber, paginationFilter.PageSize);
            response.TotalRecords = records;

            return response;
        }

        public async Task<IEnumerable<Album>> GetUserAlbumsAsync(int userId)
        {
            var albums = await GetAlbumDataAsync();
            var userAlbums = albums.Where(x => x.Id == userId).ToList();
            return userAlbums;
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
