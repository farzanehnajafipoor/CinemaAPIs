using farzan.Data;
using farzan.Interfaces;
using farzan.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace farzan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MoviesController : ControllerBase
    {
        private readonly IMyDependency _myDependency;
        public MoviesController(IMyDependency myDependency)
        {
            _myDependency = myDependency;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Post([FromForm] Movie movieObj)
        {
            var guid = Guid.NewGuid();
            var filePath = Path.Combine("wwwroot", guid + ".jpg");
            if (movieObj.Image != null)
            {
                var fileStreem = new FileStream(filePath, FileMode.Create);
                movieObj.Image.CopyTo(fileStreem);
                movieObj.ImageUrl = filePath;
            }
            _myDependency.Insert(movieObj);
            return Ok("Added Successfully");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromForm] Movie movieObj)
        {
            var movie = _myDependency.GetById(id);
            if (movie == null)
            {
                return NotFound(id + " Does Not Exist");
            }
            else
            {
                var guid = Guid.NewGuid();
                var filePath = Path.Combine("wwwroot", guid + ".jpg");
                if (movieObj.Image != null)
                {
                    var fileStreem = new FileStream(filePath, FileMode.Create);
                    movieObj.Image.CopyTo(fileStreem);
                    movie.ImageUrl = filePath;
                }
                _myDependency.Update(movie, movieObj);
                return Ok(id + " Updated");
            }
        }
        [Authorize(Roles =  "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _myDependency.GetById(id);
            if (movie == null)
            {
                return NotFound(id + " Does Not Exist");
            }
            else
            {
                _myDependency.Delete(movie);
                return Ok(id + " Deleted");
            }
        }

        [Authorize]
        [HttpGet("[action]")]
        public IActionResult AllMovies(string sort , int? pageNumber, int? pageSize)
        {
            var movies = _myDependency.GetAll(sort, pageNumber, pageSize);
            return Ok(movies);

        }
        [Authorize]
        [HttpGet("[action]")]
        public IActionResult SearchMovies(string movieName)
        {
            var movies = _myDependency.SearchMovies(movieName);
            return Ok(movies);

        }
        [Authorize]
        [HttpGet("[action]/{id}")]
        public IActionResult MoviDetail(int id)
        {
            var movie = _myDependency.GetById(id);
            if (movie == null)
            {
                return NotFound(id + " Does Not Exist");
            }
            else
            {
                return Ok(movie);
            }
        }
    }
}
