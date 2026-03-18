using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;

namespace Books.Application.DTOs.BookDTOs
{
    public class BookReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int Year { get; set; }
        public ICollection<int>? AuthorsId { get; set; }
        public int GenreId { get; set; }
        public DateTime? CreatedAt { get; set; }

        public BookReadDto() { }
        public BookReadDto(BookEntity book)
        {
            this.Id = book.Id;
            this.Title = book.Title;
            this.Year = book.Year;
            if(book.Authors != null)
            {
                this.AuthorsId = book.Authors.Select(author => author.Id).ToList();
            }
            this.GenreId = book.GenreId;
        }
    }


}
