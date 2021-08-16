using AutoMapper;
using Domain.Models;
using Application.DTOs.Album;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Album, AlbumResponse>().ReverseMap();
            CreateMap<Photo, AlbumPhotosResponse>().ReverseMap();
        }
    }
}
