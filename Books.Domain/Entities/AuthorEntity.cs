using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Books.Domain.Entities
{
    public class AuthorEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SureName { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<BookEntity>? Books { get; set; }
        public AuthorEntity() { }
        public AuthorEntity(string name, string sureName)
        {
            this.Name = name;
            this.SureName = sureName;
        }
    }
}
