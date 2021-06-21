using farzan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace farzan.Interfaces
{
   public  interface IMyDependency
    {
        dynamic GetAll(string sort, int? pageNumber, int? pageSize);
        dynamic SearchMovies(string movieName);
        Movie GetById(int id);
        void Insert(Movie movieObj);
        void Update(Movie movie, Movie movieObj);
        void Delete(Movie movie);
    }
}
