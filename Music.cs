using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicProjectWinform
{
    class Music
    {
        public Music(int Id, string Title, DateTime ReleaseDate, decimal Price, string Genre, Boolean IsValidated)
        {
            this.Id = Id;
            this.Title = Title;
            this.ReleaseDate = ReleaseDate;
            this.Price = Price;
            this.Genre = Genre;
            this.IsValidated = IsValidated;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string Genre { get; set; }
        public Boolean IsValidated { get; set; }
    }
}
