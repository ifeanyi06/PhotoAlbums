using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Album
{
    public class AlbumPhotoStatsResponse
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string AlbumTitle { get; set; }
        public int TotalWords { get; set; }
        public int MaximumWords { get; set; }
        public int MinimumWords { get; set; }

    }
}
