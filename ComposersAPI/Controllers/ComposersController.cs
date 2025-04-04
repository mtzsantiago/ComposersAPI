using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;
using ComposersAPI.Models;

namespace ClassicalComposersAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]  // Versioning in the route
    [ApiVersion("1.0")]  // Specify the version for this controller
    public class ComposersController : ControllerBase
    {
        private readonly string _connectionString;

        public ComposersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException(nameof(_connectionString));
        }

        // GET: api/composers
        [HttpGet]
        public ActionResult<IEnumerable<Composer>> GetComposers()
        {
            var composers = new List<Composer>();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM composers", conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    composers.Add(new Composer
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Era = reader.GetString(2),
                        ObrasNotables = reader.GetString(3)
                    });
                }
            }
            return Ok(composers);
        }

        // GET: api/composers/{id}
        [HttpGet("{id}")]
        public ActionResult<Composer> GetComposer(int id)
        {
            Composer? composer = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM composers WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    composer = new Composer
                    {
                        Id = reader.GetInt32(0),
                        Nombre = reader.GetString(1),
                        Era = reader.GetString(2),
                        ObrasNotables = reader.GetString(3)
                    };
                }
            }
            return composer != null ? Ok(composer) : NotFound();
        }

        // POST: api/composers
        [HttpPost]
        public IActionResult CreateComposer([FromBody] Composer composer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO composers (nombre, era, obras_notables) VALUES (@nombre, @era, @obrasNotables)", conn);
                cmd.Parameters.AddWithValue("@nombre", composer.Nombre);
                cmd.Parameters.AddWithValue("@era", composer.Era);
                cmd.Parameters.AddWithValue("@obrasNotables", composer.ObrasNotables);
                cmd.ExecuteNonQuery();
            }
            return CreatedAtAction(nameof(GetComposers), new { composer.Nombre }, composer);
        }

        // PUT: api/composers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateComposer(int id, [FromBody] Composer composer)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("UPDATE composers SET nombre = @nombre, era = @era, obras_notables = @obrasNotables WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nombre", composer.Nombre);
                cmd.Parameters.AddWithValue("@era", composer.Era);
                cmd.Parameters.AddWithValue("@obrasNotables", composer.ObrasNotables);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0) return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/composers/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteComposer(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM composers WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                int rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected == 0) return NotFound();
            }
            return NoContent();
        }
    }

    
}
