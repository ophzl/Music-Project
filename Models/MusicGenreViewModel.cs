﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcMusic.Models
{
    public class MovieGenreViewModel
    {
        public List<Music> Musics { get; set; }
        public SelectList Genres { get; set; }
        public string MusicGenre { get; set; }
        public string SearchString { get; set; }
    }
}