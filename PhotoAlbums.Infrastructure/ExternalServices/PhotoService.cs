using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
namespace Infrastructure.ExternalServices
{
    public interface IPhotoService
    {
        Task<IEnumerable<Album>> GetAlbums();
        Task<IEnumerable<Photo>> GetAlbumPhotos();
    }

    public class PhotoService : IPhotoService
    {
        private HttpClient _httpClient;

        public PhotoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Album>> GetAlbums()
        {
            var response = await _httpClient.GetAsync("albums");
             return await _httpClient.GetFromJsonAsync<IEnumerable<Album>>("albums");
        }

        public async Task<IEnumerable<Photo>> GetAlbumPhotos()
        {
            var response = await _httpClient.GetAsync("photos");
            return await _httpClient.GetFromJsonAsync<IEnumerable<Photo>>("photos");
        }

    }
}
