using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Books.Domain.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<BookEntity>? Books { get; set; }
        public GenreEntity() { }
        public GenreEntity(string title)
        {
            this.Title = title;
        }

    }
}
