using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Infrastructure.Configuration
{
    sealed public class RabbitMqSettings
    {

        public string Host { get; set; } = null!;
        public int Port { get; set; }
    }
}
