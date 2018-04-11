using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspectCoreDemo.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ICustomService _service;

        public ValuesController(ICustomService service)
        {
            _service = service;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            _service.Call();
            return new string[] { "value1", "value2" };
        }
    }
}