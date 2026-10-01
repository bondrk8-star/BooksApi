using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BooksApi.Server
{
    [ApiController]
    [Route("/Hello")]
    public class HelloController : ControllerBase
    {
        static private List<HelloVar> helloList =
        [
            new HelloVar
            {
                Id = 1,
                Name = "Hello",
            },

            new HelloVar
            {
                Id = 2,
                Name = "Good Morning",
            }

        ];

        [HttpGet]
        public IActionResult GetHello([FromQuery] int helloId)
        {
            var hello = helloList.FirstOrDefault(h => h.Id == helloId);
            return Ok(hello);
        }

        [HttpPost]
        public IActionResult CreateHello([FromBody] HelloVar helloVar)
        {
            helloList.Add(helloVar);
            return Ok();
        }
    }

    public class HelloVar
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
