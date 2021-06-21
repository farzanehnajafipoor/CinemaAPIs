using farzan.Data;
using farzan.Interfaces;
using farzan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace farzan.Services
{
    public class MyDependency : IMyDependency
    {
        private CinemaDbContext _dbContext ;
        public MyDependency(CinemaDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public dynamic GetAll(string sort ,int? pageNumber, int? pageSize)
        {
            var currentPageNumber = pageNumber ?? 1;
            var currentpageSize = pageSize ?? 5;
            var movies = from movie in _dbContext.Movies
                         select new
                         {
                             Id = movie.Id,
                             Name = movie.Name,
                             Duration = movie.Duration,
                             Language = movie.Language,
                             Rating = movie.Rating,
                             Genre = movie.Genre,
                             ImageUrl = movie.ImageUrl
                         };
            switch (sort)
            {
                case "asc":
                    return movies.Skip((currentPageNumber - 1) * currentpageSize).Take(currentpageSize).OrderBy(m => m.Rating);
                case "desc":
                    return movies.Skip((currentPageNumber - 1) * currentpageSize).Take(currentpageSize).OrderByDescending(m => m.Rating);
                default:
                    return movies.Skip((currentPageNumber - 1)* currentpageSize).Take(currentpageSize);
            }
        }
        public dynamic SearchMovies(string movieName)
        {
            var movies = from movie in _dbContext.Movies
                         where movie.Name.StartsWith(movieName)
                         select new
                         {
                             Id = movie.Id,
                             Name = movie.Name,
                             Duration = movie.Duration,
                             Language = movie.Language,
                             Rating = movie.Rating,
                             Genre = movie.Genre,
                             ImageUrl = movie.ImageUrl
                         };
            return movies;
        }
        public Movie GetById(int id)
        {
            return _dbContext.Movies.Find(id);
        }
        public void Insert(Movie movieObj)
        {
            _dbContext.Movies.Add(movieObj);
            _dbContext.SaveChanges();
        }
        public void Update(Movie movie, Movie movieObj)
        {
            movie.Name = movieObj.Name;
            movie.Description = movieObj.Description;
            movie.Language = movieObj.Language;
            movie.Duration = movieObj.Duration;
            movie.PlayingTime = movieObj.PlayingTime;
            movie.PlayingDate = movieObj.PlayingDate;
            movie.Rating = movieObj.Rating;
            movie.Genre = movieObj.Genre;
            movie.TrailorUrl = movieObj.TrailorUrl;
            movie.TicketPrice = movieObj.TicketPrice;
            _dbContext.SaveChanges();
        }
        public void Delete(Movie movie)
        {
            _dbContext.Movies.Remove(movie);
            _dbContext.SaveChanges();
        }
    }
}
