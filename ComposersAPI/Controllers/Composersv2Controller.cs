using ComposersAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComposersAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/composers")]
    public class Composersv2Controller : ControllerBase
    {
        private static List<Composer> composers = new()
        {
            new Composer { Id = 1, Nombre = "Ludwig van Beethoven", Era = "Classical", ObrasNotables = "Symphony No. 5" },
            new Composer { Id = 2, Nombre = "Johann Sebastian Bach", Era = "Baroque", ObrasNotables = "Brandenburg Concertos" },
            new Composer { Id = 3, Nombre = "Wolfgang Amadeus Mozart", Era = "Classical", ObrasNotables = "The Magic Flute" }
        };

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAll()
        {
            // Nombre en mayúsculas (para distinguir v2)
            var result = composers.Select(c => new
            {
                c.Id,
                Name = c.Nombre.ToUpper(),
                c.Era,
                c.ObrasNotables
            });

            return Ok(result);
        }
    }
}
