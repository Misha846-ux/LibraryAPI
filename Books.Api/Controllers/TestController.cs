using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Books.Api.Controllers
{
    public class User
    {
        [Range(1,100, ErrorMessage ="Indentificator fail (1-100)")]
        public int Id { get; set; }
        [EmailAddress]
        public string UserName { get; set; }
    }
    [ApiController]
    [Route("api/[controller]")]
    public class TestController: ControllerBase
    {
        [HttpPost]
        public IActionResult CreateData([FromBody] User user)
        {
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetItemById([FromRoute] int id)
        {
            if(id < 5)
            {
                return Ok("Just ok");
            }
            throw new Exception("Fail");
        }
    }
}
