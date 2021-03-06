using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Helpers;
using Web.Models;

namespace Web.Controllers
{
    [Route("api/Movies")]
    public class MoviesController : Controller
    {
        private readonly DataContext _context;

        public MoviesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public IEnumerable<Movie> GetMovie([FromQuery]MovieFilter filter)
        {
            var movies = _context.Movies.Select(movie => movie);
            if (filter.Year != null)
            {
                movies = movies.Where(movie => movie.Year == filter.Year);
            }
            if (filter.Genre != null)
            {
                movies = movies.Where(movie => movie.Genre.Contains(filter.Genre));
            }
            if (filter.Producer != null)
            {
                movies = movies.Where(movie => movie.Producer == filter.Producer);
            }
            if (filter.Size.HasValue)
            {
                movies = movies.Skip((filter.Page - 1) * filter.Size.Value).Take(filter.Size.Value);
            }
            return movies;
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                return NotFound();
            }
            
            return Ok(movie);
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<IActionResult> PostMovie([FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie([FromRoute] int id, [FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        
        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return Ok(movie);
        }

        // PATCH: api/Songs/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchMovie([FromRoute] int id, [FromBody]JsonPatchDocument<Movie> patch)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(m => m.Id == id);

            patch.ApplyTo(movie, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}