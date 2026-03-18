using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;

namespace Books.Application.DTOs.GenreDTOs
{
    public class GenreReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public ICollection<int> Books { get; set; }
        public GenreReadDto() { }
        public GenreReadDto(GenreEntity genre)
        {
            this.Id = genre.Id;
            this.Title = genre.Title;
            if(genre.Books != null)
            {
                this.Books = genre.Books.Select(x => x.Id).ToList();
            }
        }
    }
}
