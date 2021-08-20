using Application.DTOs.Album;
using Application.Utility;
using Application.Wrappers;
using Microsoft.Extensions.Logging;
using Moq;
using PhotoAlbums.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web;
using Web.Services;
using Xunit;

namespace PhotoAlbums.Test
{
    public class AlbumControllerTest
    {
        readonly Mock<IAlbumPhotoService> _albumService;
        readonly Mock<ILogger<AlbumController>> _logger;
        readonly AlbumController _albumController;

        public AlbumControllerTest()
        {
            _albumService = new Mock<IAlbumPhotoService>();

            _logger = new Mock<ILogger<AlbumController>>();

            _albumController = new AlbumController(_logger.Object, _albumService.Object);
        }


        [Fact]
        public async void GetUserAlbumPhotos_ReturnsOnlyUsersDataOkResult()
        {

            // Act
            var request = new PaginationFilter() { PageNumber = 1, PageSize = 10 };
            _albumService.Setup(x => x.GetUserAlbumPhotoAsync(5, request)).ReturnsAsync(albumList(request));

            // Assert
            var response = await _albumController.GetAlbumPhoto(5,request);
            bool hasRecords = response.Data.Any();
            Assert.NotNull(response);
            Assert.IsType<PagedResponse<IEnumerable<AlbumResponse>>>(response);
            Assert.True(hasRecords);
        }


        [Fact]
        public async void GetAlbumPhotoStats_ReturnsOkResult()
        {

            // Act
            var request = new PaginationFilter() { PageNumber = 1, PageSize = 10 };
            _albumService.Setup(x => x.GetAlbumPhotoStatsAsync(request)).ReturnsAsync(albumStats(request));

            // Assert
            var response = await _albumController.GetAlbumPhotoStats(request);
            bool hasRecords = response.Data.Any();
            Assert.NotNull(response);
            Assert.IsType<PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>>(response);
            Assert.True(hasRecords);
        }

        [Fact]
        public void TestWordCount()
        {
            string words = "Hello world, I come in peace.";
            // Act
            int count = WordCount.getWordCount(words);

            //// Assert
            Assert.True(count==6);
        }



        private PagedResponse<IEnumerable<AlbumResponse>> albumList(PaginationFilter paginationFilter)
        {
            IList<AlbumResponse> albumResponse = new List<AlbumResponse>()
            {
                new AlbumResponse()
                {
                }
            };
            return new PagedResponse<IEnumerable<AlbumResponse>>(albumResponse, paginationFilter.PageNumber, paginationFilter.PageSize);
        }

        private PagedResponse<IEnumerable<AlbumPhotosResponse>> albumUserList(PaginationFilter paginationFilter)
        {
            IList<AlbumPhotosResponse> photoResponse = new List<AlbumPhotosResponse>()
            {
                new AlbumPhotosResponse()
                {
                }
            };
            return new PagedResponse<IEnumerable<AlbumPhotosResponse>>(photoResponse, paginationFilter.PageNumber, paginationFilter.PageSize);
        }

        private PagedResponse<IEnumerable<AlbumPhotoStatsResponse>> albumStats(PaginationFilter paginationFilter)
        {
            IList<AlbumPhotoStatsResponse> statsResponse = new List<AlbumPhotoStatsResponse>()
            {
                new AlbumPhotoStatsResponse()
                {
                }
            };
            return new PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>(statsResponse, paginationFilter.PageNumber, paginationFilter.PageSize);
        }

    }
}
