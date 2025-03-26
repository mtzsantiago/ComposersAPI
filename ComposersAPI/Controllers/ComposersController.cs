using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ClassicalComposersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComposersController : ControllerBase
    {
        private static List<Composer> Composers = new List<Composer>
        {
            new Composer { Id = 1, Nombre = "Ludwig van Beethoven", Era = "Classical/Romantic", ObrasNotables = "Symphony No. 9, Moonlight Sonata" },
            new Composer { Id = 2, Nombre = "Johann Sebastian Bach", Era = "Baroque", ObrasNotables = "Toccata and Fugue in D Minor, Brandenburg Concertos" },
            new Composer { Id = 3, Nombre = "Wolfgang Amadeus Mozart", Era = "Classical", ObrasNotables = "Requiem, The Magic Flute" }
        };

        // GET: api/composers
        [HttpGet]
        public ActionResult<IEnumerable<Composer>> GetComposers()
        {
            return Ok(Composers);
        }

        // GET: api/composers/{id}
        [HttpGet("{id}")]
        public ActionResult<Composer> GetComposer(int id)
        {
            var composer = Composers.FirstOrDefault(c => c.Id == id);
            if (composer == null)
                return NotFound();
            return Ok(composer);
        }

        // POST: api/composers
        [HttpPost]
        public ActionResult<Composer> CreateComposer(Composer composer)
        {
            composer.Id = Composers.Count + 1;
            Composers.Add(composer);
            return CreatedAtAction(nameof(GetComposer), new { id = composer.Id }, composer);
        }

        // PUT: api/composers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateComposer(int id, Composer updatedComposer)
        {
            var composer = Composers.FirstOrDefault(c => c.Id == id);
            if (composer == null)
                return NotFound();

            composer.Nombre = updatedComposer.Nombre;
            composer.Era = updatedComposer.Era;
            composer.ObrasNotables = updatedComposer.ObrasNotables;

            return NoContent();
        }

        // DELETE: api/composers/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteComposer(int id)
        {
            var composer = Composers.FirstOrDefault(c => c.Id == id);
            if (composer == null)
                return NotFound();

            Composers.Remove(composer);
            return NoContent();
        }
    }

    public class Composer
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Era { get; set; }
        public string ObrasNotables { get; set; }
    }
}
