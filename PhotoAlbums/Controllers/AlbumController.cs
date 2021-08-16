﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Domain.Models;
using Application.DTOs.Album;
using Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Wrappers;

namespace PhotoAlbums.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AlbumController : ControllerBase
    {
        private readonly ILogger<AlbumController> _logger;
        private readonly IAlbumPhotoService _albumPhotoService;

        public AlbumController(ILogger<AlbumController> logger, IAlbumPhotoService albumPhotoService)
        {
            _logger = logger;
            _albumPhotoService = albumPhotoService;
        }

        [HttpGet]
        public async Task<PagedResponse<IEnumerable<AlbumResponse>>> Get([FromQuery] PaginationFilter filter)
        {
            return await _albumPhotoService.GetAlbumsAsync(filter);
        }


        [HttpGet]
        [Route("GetAlbumPhotos")]
        public async Task<PagedResponse<IEnumerable<AlbumPhotosResponse>>> GetAlbumPhotos([FromQuery] int albumId, [FromQuery] PaginationFilter filter)
        {
            return await _albumPhotoService.GetAlbumPhotosAsync(albumId, filter);
        }

        [HttpGet]
        [Route("GetUserAlbumPhotos")]
        public async Task<PagedResponse<IEnumerable<AlbumPhotosResponse>>> GetAlbumPhoto([FromQuery] int userId, [FromQuery] PaginationFilter filter)
        {
            var response = await _albumPhotoService.GetUserAlbumPhotoAsync(userId, filter);

            return response;

        }

        [HttpGet]
        [Route("GetAlbumPhotoStats")]
        public async Task<PagedResponse<IEnumerable<AlbumPhotoStatsResponse>>> GetAlbumPhotoStats([FromQuery] PaginationFilter filter)
        {
            return await _albumPhotoService.GetAlbumPhotoStatsAsync(filter);
        }
    }
}
