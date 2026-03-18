using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books.Application.Interfaces.Healpers
{
    public interface IHashHelper
    {
        string HashPassword(string password);
        bool Check(string password, string hash);
    }
}
