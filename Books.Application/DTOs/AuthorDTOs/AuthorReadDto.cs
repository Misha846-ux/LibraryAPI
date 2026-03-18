using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Entities;

namespace Books.Application.DTOs.AuthorDTOs
{
    public class AuthorReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SureName { get; set; } = string.Empty;
        public ICollection<int>? BooksId { get; set; }

        public AuthorReadDto() { }
        public AuthorReadDto(AuthorEntity author)
        {
            this.Id = author.Id;
            this.Name = author.Name;
            this.SureName = author.SureName;
            if (author.Books != null)
            {
                this.BooksId = author.Books.Select(book => book.Id).ToList();
            }
        }
    }
}
