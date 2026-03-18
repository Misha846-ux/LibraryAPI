using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Domain.Entities
{
    public class RefreshTokenEntity
    {
        public int Id { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public bool IsRevoked { get; set; } = false; // true - значить токен відкликано

        public DateTime CreatedAt { get; set; }
        public string IpAddress { get; set; }

        public Guid UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
