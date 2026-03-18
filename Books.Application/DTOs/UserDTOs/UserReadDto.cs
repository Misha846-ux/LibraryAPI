using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Books.Domain.Enums;

namespace Books.Application.DTOs.UserDTOs
{
    public class UserReadDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; } 
    }
}
