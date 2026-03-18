using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Application.DTOs.AuthorDTOs
{
    public class AuthorCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string SureName {  get; set; } = string.Empty;
    }
}
